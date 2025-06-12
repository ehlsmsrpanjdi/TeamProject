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

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackDelay { get; private set; }
    public float AttackRange { get; private set; }

    private void Awake()
    {
        MaxHealth = defaultMaxHealth;
        CurrentHealth = defaultMaxHealth;
        Damage = defaultDamage;
        MoveSpeed = defaultMoveSpeed;
        AttackDelay = defaultAttackDelay;
        AttackRange = defaultAttackRange;
    }

    public void SetStats(ZombieStats stats)
    {
        MaxHealth = stats.maxHealth;
        CurrentHealth = stats.maxHealth;
        Damage = stats.damage;
        MoveSpeed = stats.moveSpeed;
        AttackDelay = stats.attackDelay;
        AttackRange = stats.attackRange;
    }

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

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }
}
