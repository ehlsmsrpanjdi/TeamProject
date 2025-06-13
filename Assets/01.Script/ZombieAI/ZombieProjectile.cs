using UnityEngine;

public class ZombieProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    private int damage;
    private GameObject shooter;

    private Rigidbody rb;
    private ZombieProjectilePool pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // 풀 매니저 찾기 (비효율적일 수 있으니 필요 시 외부에서 셋팅 가능)
        pool = FindObjectOfType<ZombieProjectilePool>();
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
        rb.velocity = Vector3.zero;
        shooter = null;
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    public void SetShooter(GameObject shooterObj)
    {
        shooter = shooterObj;
    }

    public void Launch(Vector3 velocity)
    {
        if (rb != null)
            rb.velocity = velocity;
    }

    private void Deactivate()
    {
        if (pool != null)
            pool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooter)
            return; // 발사자 자신 무시

        // 좀비끼리 충돌 무시
        if (other.CompareTag("Zombie"))
            return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage, transform.position, 0f);
            Deactivate();
            return;
        }

        // 플레이어, 투사체, 좀비 제외 충돌 시 비활성화
        if (!other.CompareTag("Player") && !other.CompareTag("ZombieProjectile"))
        {
            Deactivate();
        }
    }
}
