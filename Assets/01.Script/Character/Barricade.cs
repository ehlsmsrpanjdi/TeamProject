using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    
    private int currentHealth;

    //체력 초기화. 현재 참전 중인 캐릭터들의 체력을 넘겨 받음.
    //모든 체력을 하나의 바라게이트가 받을 것인가?
    //하나의 바리게이트만 있다면 피격 시 어떤 캐릭터의 체력이 감소할 것인가?
    private void OnEnable()
    {
        //currentHealth = CharacterManager.Instance.
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
