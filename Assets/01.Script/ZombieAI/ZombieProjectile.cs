using UnityEngine;

// 좀비가 발사하는 투사체 로직
public class ZombieProjectile : MonoBehaviour
{
    public float lifeTime = 5f; // 자동 비활성화까지의 시간
    private int damage;         // 투사체가 입힐 피해량
    private GameObject shooter; // 발사한 주체 (자기 자신에게 맞지 않기 위해)

    private Rigidbody rb;

    private void Awake()
    {
        // Rigidbody 캐싱
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // 일정 시간이 지나면 자동 반환
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void OnDisable()
    {
        // 활성화 상태 종료 시 초기화
        CancelInvoke();
        rb.velocity = Vector3.zero;
        shooter = null;
    }

    // 피해량 설정
    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    // 발사한 주체 설정 (자기 자신 피격 방지용)
    public void SetShooter(GameObject shooterObj)
    {
        shooter = shooterObj;
    }

    // 투사체에 속도 적용 (발사)
    public void Launch(Vector3 velocity)
    {
        if (rb != null)
            rb.velocity = velocity;
    }

    // 비활성화 및 풀로 반환
    private void Deactivate()
    {
        ObjectPool.Return("Projectile", this.gameObject);
    }


    // 트리거 구분
    private void OnTriggerEnter(Collider other)
    {
        // 자신이 쏜 투사체면 무시
        if (other.gameObject == shooter)
            return;

        // 다른 좀비에 맞으면 무시
        if (other.CompareTag("Zombie"))
            return;

        // 대상이 피해를 받을 수 있다면 대미지 전달
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage, transform.position, 0f);
            Deactivate();
            return;
        }

        // 벽, 지형 등 기타에 충돌 시 비활성화
        if (!other.CompareTag("Player") && !other.CompareTag("ZombieProjectile"))
        {
            Deactivate();
        }
    }
}
