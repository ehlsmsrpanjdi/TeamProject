using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody rb;

    private float throwForce = 10f; // 던지는 힘
    private float explosionDelay = 2f; // 폭발까지의 지연시간
    private LayerMask enemyLayer; // 데미지를 입을 폭발 범위 내 적들

    //private GameObject explosionEffect; // 폭발효과 (파티클 이펙트 등)

    private float explosionRange;
    private float grenadeDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyLayer = LayerMask.GetMask("Zombie");
        //explosionEffect = Resources.Load<GameObject>("Effects/GrenadeEffect"); 경로 예시이고, 실제 이펙트 넣으려면 게임오브젝트 이름에 맞춰서 경로 지정해야함.
    }

    public void GrenadeThrow(Vector3 direction, float range, float skillDamage)
    {
        explosionRange = range;
        grenadeDamage = skillDamage;

        rb.AddForce(direction.normalized * throwForce, ForceMode.Impulse);

        StartCoroutine(ExplosionTime());
    }

    private IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(explosionDelay);
        /*if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }*/
        SoundManager.Instance.PlaySFX(SfxType.Skill, 0);

        Explosion();
    }

    private void Explosion()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);

        foreach (Collider enemyCollider in hitEnemies)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage((int)grenadeDamage, transform.position, 0.5f);
            }
        }

        Destroy(gameObject);
    }
}
