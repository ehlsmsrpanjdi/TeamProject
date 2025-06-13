using UnityEngine;

public class DummyPlayer : MonoBehaviour, IDamageable
{
    [Header("체력")]
    public int maxHealth = 1000;
    private int currentHealth;

    //체력 초기화
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    // 대미지를 받는 함수
    public void TakeDamage(int amount, Vector3 attackerPosition, float knockbackForce)
    {
        currentHealth -= amount;
        Debug.Log($"[DummyPlayer] 대미지 {amount} 받음 → 남은 체력: {currentHealth}");

        // 체력이 0 이하일 경우 사망 처리
        if (currentHealth <= 0)
        {
            Debug.Log("[DummyPlayer] 사망 처리");
            gameObject.SetActive(false);
        }
    }
}
