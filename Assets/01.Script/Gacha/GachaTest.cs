using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaTest : MonoBehaviour
{
    private void Start()
    {
        GachaManager.Instance.OnCharacterDraw += HandleGacha;
    }

    private void HandleGacha(DrawResult data)
    {
        // 테스트를 위해 뽑기가 실행될 때마다 해당 캐릭터의 이름과 등급을 콘솔로 출력
        Debug.Log($"이름: {data.character.Name}, 등급: {data.rank}");
    }

    public void TenTimesDraw()
    {
        // 가챠 10회 반복
        for (int i = 0; i < 10; i++)
        {
            GachaManager.Instance.DrawCharacter();
        }
    }



    private void OnDestroy()
    {
        GachaManager.Instance.OnCharacterDraw -= HandleGacha;
    }
}
