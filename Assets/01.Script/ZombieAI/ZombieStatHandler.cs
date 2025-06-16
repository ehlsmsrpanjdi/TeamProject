using UnityEngine;

public class ZombieStatHandler : MonoBehaviour
{
    [Header("기본값 설정")]

    [Header("체력")]
    public int defaultMaxHealth = 100;
    [Header("대미지")]
    public int defaultDamage = 10;
    [Header("이동속도")]
    public float defaultMoveSpeed = 3f;
    [Header("공격속도")]
    public float defaultAttackDelay = 1.5f;
    [Header("사거리")]
    public float defaultAttackRange = 2f;

    public int MaxHealth { get; private set; }      // 최대 체력
    public int CurrentHealth { get; private set; }  // 현재 체력
    public int Damage { get; private set; }         // 공격력
    public float MoveSpeed { get; private set; }    // 이동 속도
    public float AttackDelay { get; private set; }  // 공격 딜레이
    public float AttackRange { get; private set; }  // 공격 사거리

    //기본값으로 스탯 세팅
    private void Awake()
    {
        MaxHealth = defaultMaxHealth;
        CurrentHealth = defaultMaxHealth;
        Damage = defaultDamage;
        MoveSpeed = defaultMoveSpeed;
        AttackDelay = defaultAttackDelay;
        AttackRange = defaultAttackRange;
    }

    // 외부에서 전달받은 스탯 데이터로 세팅
    public void SetStats(ZombieStats stats)
    {
        MaxHealth = stats.maxHealth;
        CurrentHealth = stats.maxHealth;
        Damage = stats.damage;
        MoveSpeed = stats.moveSpeed;
        AttackDelay = stats.attackDelay;
        AttackRange = stats.attackRange;
    }

    // 대미지 처리, 체력이 0 이하가 되면 true 반환 (사망 신호)
    public bool TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            return true;
        }
        return false;
    }

    // 체력 초기화 (최대 체력으로 복원)
    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }

    // 약화 모드: 스탯을 10%로 줄임
    public void ApplyWeakPenalty()
    {
        MaxHealth = Mathf.Max(1, Mathf.CeilToInt(defaultMaxHealth * 0.1f));
        CurrentHealth = MaxHealth;
        Damage = Mathf.Max(1, Mathf.CeilToInt(defaultDamage * 0.1f));
    }
}
