using UnityEngine;
using System.Collections;

public class KnockbackArea : MonoBehaviour
{
    public float radius = 3f;
    public float interval = 0.5f;
    public float knockbackForce = 5f;
    public int knockbackDamage = 0;
    public LayerMask targetMask;

    private void OnEnable()
    {
        StartCoroutine(KnockbackLoop());
    }

    private IEnumerator KnockbackLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetMask);

            foreach (var hit in hits)
            {
                IDamageable target = hit.GetComponent<IDamageable>();
                if (target != null)
                {
                    Vector3 dir = hit.transform.position - transform.position;
                    target.TakeDamage(knockbackDamage, transform.position, knockbackForce);
                }
            }

            yield return wait;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}

