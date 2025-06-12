using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(ZombieStatHandler))]
public class ZombieAI : MonoBehaviour, IDamageable
{
    public enum State { Idle, Chase, Attack, Die }
    private State currentState;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private Renderer zombieRenderer;
    private Transform target;
    private ZombieStatHandler statHandler;
    private ZombiePool pool;

    [Header("디버그용 공격 범위")]
    [SerializeField] private float debugAttackRange = 2f;

    [Header("접근거리")]
    public float stopDistance = 2f;

    [Header("대미지 점멸")]
    public float flashDuration = 0.1f;

    [Header("넉백회복")]
    public float knockbackRecoverTime = 0.3f;

    private float attackTimer;
    private Color originalColor;
    private bool isKnockback = false;

    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (zombieRenderer == null) zombieRenderer = GetComponentInChildren<Renderer>();
        if (zombieRenderer != null)
            originalColor = zombieRenderer.material.color;

        if (statHandler == null) statHandler = GetComponent<ZombieStatHandler>();
        statHandler.ResetHealth();
        agent.speed = statHandler.MoveSpeed;


        rb.isKinematic = true;
        target = GameObject.FindWithTag("Player")?.transform;
        ChangeState(State.Chase);
    }

    private void Awake()
    {
        pool = FindObjectOfType<ZombiePool>();
        statHandler = GetComponent<ZombieStatHandler>();
    }

    private void Update()
    {
        if (currentState == State.Die || isKnockback) return;

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

    private void Chase()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= statHandler.AttackRange)
        {
            ChangeState(State.Attack);
            return;
        }

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

            Vector3 lookDir = target.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
        }
    }


    private void Attack()
    {
        if (target == null) return;

        agent.ResetPath();

        // 애니메이터가 있고, 컨트롤러가 할당된 경우에만 실행
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("", false);

        transform.LookAt(target);

        attackTimer += Time.deltaTime;

        if (attackTimer >= statHandler.AttackDelay)
        {
            Debug.Log("[ZombieAI] 공격 시도");

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

            attackTimer = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * (debugAttackRange * 0.5f);
        Gizmos.DrawWireSphere(center, debugAttackRange);
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        attackTimer = 0;
    }

    public void TakeDamage(int amount, Vector3 attackerPosition, float knockbackForce)
    {
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

    private IEnumerator FlashRed()
    {
        if (zombieRenderer == null) yield break;

        zombieRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        zombieRenderer.material.color = originalColor;
    }

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
        rb.isKinematic = true;
        agent.enabled = true;
        isKnockback = false;
    }
    public void Die()
    {
        if (currentState == State.Die) return;

        ChangeState(State.Die);
        agent.isStopped = true;
        agent.enabled = false;
        rb.isKinematic = true;
        animator.SetTrigger("die");

        StartCoroutine(ReturnToPoolAfterDelay(2f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnZombie(gameObject);
    }
}
