using UnityEngine;

public class DummyPlayer : MonoBehaviour, IDamageable
{
    public int maxHealth = 1000;
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector3 attackerPosition, float knockbackForce)
    {
        currentHealth -= amount;
        Debug.Log($"[DummyPlayer] 대미지 {amount} 받음 → 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("[DummyPlayer] 사망 처리");
            gameObject.SetActive(false);
        }
    }
}
