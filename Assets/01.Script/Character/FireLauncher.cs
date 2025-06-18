using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLauncher : MonoBehaviour
{
    
    private LayerMask enemyLayer; // 데미지를 입을 적들

    private void Awake()
    {
        
        enemyLayer = LayerMask.GetMask("Zombie");
        //explosionEffect = Resources.Load<GameObject>("Effects/GrenadeEffect"); 경로 예시이고, 실제 이펙트 넣으려면 게임오브젝트 이름에 맞춰서 경로 지정해야함.
    }

    public void Lauancher(float skillDamage, float skillRange)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, skillRange, enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            IDamageable target = collider.GetComponent<IDamageable>();
            if(target != null)
            {
                target.TakeDamage((int)skillDamage, transform.position, knockbackForce: 0f);
            }
        }
    }

}
