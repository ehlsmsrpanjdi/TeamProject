using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLauncher : MonoBehaviour
{
    private float duration = 2f;
    private float interval = 0.5f;
    private LayerMask enemyLayer; // 데미지를 입을 적들

    private void Awake()
    {

        enemyLayer = LayerMask.GetMask("Zombie");
        //explosionEffect = Resources.Load<GameObject>("Effects/GrenadeEffect"); 경로 예시이고, 실제 이펙트 넣으려면 게임오브젝트 이름에 맞춰서 경로 지정해야함.
    }

    
    public void Launch(float skillDamage, float skillRange)
    {
        StartCoroutine(ApplyDamage(skillDamage, skillRange));
    }


    public IEnumerator ApplyDamage(float damage, float range)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
            foreach (Collider collider in hitEnemies)
            {
                IDamageable target = collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage((int)damage, transform.position, knockbackForce: 0f);
                }
            }
            yield return CoroutineHelper.GetTime(interval);
            elapsed += interval;
        }
        Destroy(gameObject);
    }

}
