using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(ZombieStatHandler))]
public class ZombieAI : MonoBehaviour, IDamageable
{
    // 좀비 행동 상태
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Die
    }

    // 공격 타입 구분용 열거형 (enum)
    public enum AttackType
    {
        Melee,
        Projectile
    }

    private State currentState;

    private NavMeshAgent agent; // 네비메시 에이전트
    private Animator animator; // 애니메이터
    private Rigidbody rb; // 리지드바디
    private Renderer zombieRenderer; // 렌더러 (피격 시 색변경용)
    private Transform target; // 플레이어 Transform
    private ZombieStatHandler statHandler; // 좀비 스탯 핸들러
    private ZombiePool pool; // 좀비 풀
    private WaveManager waveManager; // 웨이브 매니저
    private Coroutine knockbackCoroutine;

    [Header("공격타입")]
    public AttackType attackType = AttackType.Melee;

    [Header("발사위치")]
    public Transform firePoint;

    [Header("디버그용 공격 범위")] [SerializeField]
    private float debugAttackRange = 2f;

    [Header("접근거리")] public float stopDistance = 2f;

    [Header("대미지 점멸")] public float flashDuration = 0.1f;

    [Header("넉백회복")] public float knockbackRecoverTime = 0.3f;

    private float attackTimer; // 공격 딜레이 타이머
    private Color originalColor; // 원래 색상 저장
    private bool isKnockback = false; // 넉백 중인지 여부

    // 오브젝트 활성화 시 초기화
    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
        if (statHandler == null) statHandler = GetComponent<ZombieStatHandler>();

        target = GameObject.FindWithTag("Player")?.transform; // 플레이어 찾기

        statHandler.ResetHealth(); // 체력 초기화

        ChangeState(State.Chase); // 초기 상태를 추적으로 설정

        // velocity는 isKinematic이 false일 때만 세팅 가능하도록 수정
        rb.isKinematic = false;    // 먼저 물리 적용 상태로 변경
        rb.velocity = Vector3.zero; // 속도 초기화
        rb.isKinematic = true;     // 다시 키네매틱 모드로 변경

        // 현재 위치를 NavMesh 위로 보정
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.enabled = true;
            agent.Warp(hit.position);
        }

        animator.Rebind();

        // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("", true);

        agent.speed = statHandler.MoveSpeed;
    }

    private void Awake()
    {
        // Renderer 컴포넌트 가져오기
        zombieRenderer = GetComponentInChildren<Renderer>();
        // Renderer가 있으면 원래 색상을 저장
        if (zombieRenderer != null)
            originalColor = zombieRenderer.material.color;

        // 시작 시 참조 캐싱
        pool = FindObjectOfType<ZombiePool>();
        statHandler = GetComponent<ZombieStatHandler>();
    }

    // 시작 시 웨이브 매니저 참조 캐싱
    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    // 매 프레임 상태에 따른 행동 실행
    private void Update()
    {
        if (currentState == State.Die || isKnockback) return; // 죽었거나 넉백 중이면 행동 중지

        switch (currentState)
        {
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    // 플레이어를 추적하는 상태
    private void Chase()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        // 공격 범위 내에 들어오면 공격 상태로 전환
        if (dist <= statHandler.AttackRange)
        {
            ChangeState(State.Attack);
            return;
        }

        // 플레이어와 일정 거리 이상 떨어져 있으면 이동
        if (dist > stopDistance)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            else
            {
                Debug.LogWarning("[Chase] agent가 NavMesh 위에 없음");
            }

            // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetBool("", true);
        }
        else
        {
            agent.isStopped = true;

            // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetBool("", false);

            // 플레이어를 향해 부드럽게 회전
            Vector3 lookDir = target.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir),
                    10f * Time.deltaTime);
        }
    }
    // 플레이어 공격
    private void Attack()
    {
        if (target == null || isKnockback) return;

        // 플레이어와의 거리 계산
        float dist = Vector3.Distance(transform.position, target.position);

        // 공격 범위 밖이면 추적 상태로 전환하고 공격 중단
        if (dist > statHandler.AttackRange)
        {
            ChangeState(State.Chase);
            return;
        }

        agent.ResetPath(); // 이동 경로 초기화

        // 애니메이터가 있고, 컨트롤러가 할당된 경우 애니메이션 이동 중지 처리
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("IsMoving", false);

        transform.LookAt(target); // 플레이어를 바라봄

        attackTimer += Time.deltaTime;

        // 공격 딜레이가 지나면 공격 실행
        if (attackTimer >= statHandler.AttackDelay)
        {
            Debug.Log("[ZombieAI] 공격 시도");

            // 공격 타입에 따라 근접 또는 투사체 공격 분기 처리
            if (attackType == AttackType.Melee)
            {
                // 근접 공격 처리
                IDamageable damageTarget = target.GetComponent<IDamageable>();
                if (damageTarget != null)
                {
                    Debug.Log("[ZombieAI] 근접 대미지 전달");
                    damageTarget.TakeDamage(statHandler.Damage, transform.position, 0f);
                }
                else
                {
                    Debug.LogWarning("[ZombieAI] 대상이 IDamageable 아님");
                }
            }
            // 원거리 공격
            if (firePoint != null && target != null)
            {
                Vector3 velocity = CalculateProjectileVelocity(firePoint.position, target.position);

                if (velocity == Vector3.zero)
                {
                    Debug.LogWarning("[ProjectileAttack] CalculateProjectileVelocity 반환값이 0벡터입니다. 직선 발사로 대체합니다.");
                    velocity = (target.position - firePoint.position).normalized * 20f;
                }

                GameObject proj = ZombieProjectilePool.Instance.GetProjectile(
                    firePoint.position,
                    Quaternion.LookRotation(velocity),
                    statHandler.Damage,
                    gameObject,
                    velocity
                );

                ZombieProjectile projScript = proj.GetComponent<ZombieProjectile>();
                if (projScript != null)
                {
                    projScript.gameObject.SetActive(true);
                    projScript.Launch(velocity);
                }
            }
            else
            {
                Debug.LogWarning("[ZombieAI] 투사체 풀 또는 발사 위치 또는 타겟이 설정되지 않음");
            }
            attackTimer = 0f; // 타이머 초기화
        }
    }
    // 원거리 공격 포물선 계산
    private Vector3 CalculateProjectileVelocity(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target - origin;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float distance = directionXZ.magnitude;
        float yOffset = direction.y;
        float gravity = Physics.gravity.y;

        float speed = Mathf.Clamp(distance * 1.2f, 10f, 30f);

        float speedSq = speed * speed;
        float underRoot = speedSq * speedSq - gravity * (gravity * distance * distance + 2 * yOffset * speedSq);

        if (underRoot < 0)
            return Vector3.zero;

        float root = Mathf.Sqrt(underRoot);
        float angle1 = Mathf.Atan((speedSq + root) / (Mathf.Abs(gravity) * distance));
        float angle2 = Mathf.Atan((speedSq - root) / (Mathf.Abs(gravity) * distance));
        float angle = Mathf.Min(angle1, angle2);

        Vector3 velocity = directionXZ.normalized * speed * Mathf.Cos(angle);
        velocity.y = speed * Mathf.Sin(angle);

        return velocity;
    }




    // 에디터에서 공격 범위를 시각화하는 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * (debugAttackRange * 0.5f);
        Gizmos.DrawWireSphere(center, debugAttackRange);
    }

    // 상태 변경
    private void ChangeState(State newState)
    {
        currentState = newState;
        attackTimer = 0;
    }

    // 대미지를 받음
    public void TakeDamage(int amount, Vector3 attackerPosition, float knockbackForce)
    {
        if (isKnockback)
        {
            return;
        }

        bool isDead = statHandler.TakeDamage(amount);
        if (isDead)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
            StartKnockback(attackerPosition, knockbackForce);
        }
    }

    // 붉은색 점멸 효과 코루틴
    private IEnumerator FlashRed()
    {
        if (zombieRenderer == null) yield break;

        zombieRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        zombieRenderer.material.color = originalColor;
    }

    // 넉백 효과 코루틴
    private IEnumerator ApplyKnockback(Vector3 attackerPosition, float force)
    {
        if (rb == null || currentState == State.Die) yield break;

        isKnockback = true;
        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 dir = (transform.position - attackerPosition).normalized;
        dir.y = 0.1f;
        rb.AddForce(dir * force, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackRecoverTime);
        if (currentState == State.Die) yield break;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; // 회전 문제 방지
        rb.isKinematic = true;
        agent.enabled = true;

        isKnockback = false;
        // 상태 강제 초기화
        ChangeState(State.Chase);

        // 공격 타이머 초기화
        attackTimer = 0f;
    }
    //넉백 상태 초기화
    private void ResetStateAfterKnockback()
    {
        ChangeState(State.Chase);
    }
    // 좀비 사망 처리
    public void Die()
    {
        StopKnockback();
        if (currentState == State.Die) return;

        ChangeState(State.Die);

        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger("");

        // 웨이브 매니저에 좀비 사망 알림
        waveManager?.OnZombieDied();

        // 풀에 반환 대기 시작
        int type = (attackType == AttackType.Projectile) ? 2 : 1;
        StartCoroutine(ReturnToPoolAfterDelay(2f, type));
    }

    // 지정 시간 후 풀에 좀비 반환
    private IEnumerator ReturnToPoolAfterDelay(float delay, int type)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnZombie(type, gameObject);

        // 안전을 위해 웨이브 매니저에 추가 사망 알림
        FindObjectOfType<WaveManager>()?.OnZombieDied();
    }


    public void StartKnockback(Vector3 attackerPosition, float force)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);
        knockbackCoroutine = StartCoroutine(ApplyKnockback(attackerPosition, force));
    }

    public void StopKnockback()
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }
    }
}
