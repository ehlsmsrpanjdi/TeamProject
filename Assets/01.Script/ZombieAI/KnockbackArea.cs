using UnityEngine;
using System.Collections;

public class KnockbackArea : MonoBehaviour
{
    [Header("넉백 범위")]
    public float radius = 3f;
    [Header("넉백 간격")]
    public float interval = 0.5f;
    [Header("넉백 힘")]
    public float knockbackForce = 5f;
    [Header("대미지")]
    public int knockbackDamage = 0;
    [Header("넉백 대상")]
    public LayerMask targetMask;

    //넉백 반복 코루틴
    private void OnEnable()
    {
        StartCoroutine(KnockbackLoop());
    }

    // 일정 간격으로 반경 내 대상에게 넉백 및 대미지 적용하는 코루틴
    private IEnumerator KnockbackLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            // 반경 내 콜라이더 감지
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetMask);

            foreach (var hit in hits)
            {
                // IDamageable 인터페이스가 구현된 대상에만 처리
                IDamageable target = hit.GetComponent<IDamageable>();
                if (target != null)
                {
                    // 넉백 방향 계산
                    Vector3 dir = hit.transform.position - transform.position;
                    // 대미지 및 넉백 적용
                    target.TakeDamage(knockbackDamage, transform.position, knockbackForce);
                }
            }

            yield return wait; // 대기 후 반복
        }
    }

    // 넉백 범위 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}


