using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IDamageable
{
    
    private float currentHealth;

    public static Barricade instance;

    public void Awake()
    {
        instance = this;
    }


    //체력 초기화. 현재 참전 중인 캐릭터들의 체력을 넘겨 받음.
    //모든 체력을 하나의 바라게이트가 받을 것인가?
    //하나의 바리게이트만 있다면 피격 시 어떤 캐릭터의 체력이 감소할 것인가?

    //바리게이트는 하나
    //웨이브 마다 체력이 세팅이 되도록
    //체력은 그냥 캐릭터의 체력 합산
    //무한 반복 웨이브가 오면 체력은 무한으로?
    public void SetHealth()
    {
        currentHealth = CharacterManager.Instance.GetTotalHealt();
        Debug.Log($"{currentHealth}");
    }

    // 대미지를 받는 함수
    public void TakeDamage(int amount, Vector3 attackerPosition, float knockbackForce)
    {
        currentHealth -= amount;
        Debug.Log($"대미지 {amount} 받음 → 남은 체력: {currentHealth}");

        // 체력이 0 이하일 경우 스테이지 실패
        if (currentHealth <= 0)
        {
            Debug.Log("스테이지 실패");
            // 이전 스테이지로 돌아가기
        }
    }

#if UNITY_EDITOR
    public void EditFunctionSetHealth()
    {
         SetHealth();
    }


#endif
}
