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

    private State currentState;

    private NavMeshAgent agent; // 네비메시 에이전트
    private Animator animator; // 애니메이터
    private Rigidbody rb; // 리지드바디
    private Renderer zombieRenderer; // 렌더러 (피격 시 색변경용)
    private Transform target; // 플레이어 Transform
    private ZombieStatHandler statHandler; // 좀비 스탯 핸들러
    private ZombiePool pool; // 좀비 풀
    private WaveManager waveManager; // 웨이브 매니저

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

        // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("", false);

        transform.LookAt(target); // 플레이어를 바라봄

        attackTimer += Time.deltaTime;

        if (attackTimer >= statHandler.AttackDelay)
        {
            Debug.Log("[ZombieAI] 공격 시도");

            // 공격 대상이 IDamageable 인터페이스 구현 여부 확인 후 대미지 전달
            IDamageable damageTarget = target.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                Debug.Log("[ZombieAI] 대미지 전달");
                damageTarget.TakeDamage(statHandler.Damage, Vector3.zero, 0f);
            }
            else
            {
                Debug.LogWarning("[ZombieAI] 대상이 IDamageable 아님");
            }

            attackTimer = 0; // 타이머 초기화
        }
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
            StartCoroutine(ApplyKnockback(attackerPosition, knockbackForce));
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
        if (rb == null) yield break;

        isKnockback = true;
        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 dir = (transform.position - attackerPosition).normalized;
        dir.y = 0.1f;
        rb.AddForce(dir * force, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackRecoverTime);

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
        if (currentState == State.Die) return;

        ChangeState(State.Die);

        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger("");

        // 웨이브 매니저에 좀비 사망 알림
        waveManager?.OnZombieDied();

        // 풀에 반환 대기 시작
        StartCoroutine(ReturnToPoolAfterDelay(2f));
    }

    // 지정 시간 후 풀에 좀비 반환
    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnZombie(gameObject);

        // 안전을 위해 웨이브 매니저에 추가 사망 알림
        FindObjectOfType<WaveManager>()?.OnZombieDied();
    }
}
