using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DrawResult
{
    public CharacterData character;
    public Rank rank;
}

public class GachaManager : MonoBehaviour
{

    public static GachaManager Instance;

    public CharacterDataBase characterDataBase;

    public event Action<DrawResult> OnCharacterDraw; // 캐릭터 뽑기 시 호출되는 이벤트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 가챠 버튼 등을 눌러서 호출
    public void DrawCharacter()
    {
        //TODO: 뽑기에 필요한 재화 소모 로직

        // 캐릭터의 랭크를 정함
        float random = UnityEngine.Random.Range(0f, 100f);
        Rank rank;

        if (random < 0.1f)
        {
            rank = Rank.SSS; // 0.1%
        }
        else if (random < 1f)
        {
            rank = Rank.SS; // 0.9%
        }
        else if (random < 5f)
        {
            rank = Rank.S; // 4%
        }
        else if (random < 15f)
        {
            rank = Rank.A; // 10%
        }
        else if (random < 40f)
        {
            rank = Rank.B; // 25%
        }
        else
        {
            rank = Rank.C; // 60%
        }
        // 각 랭크별 확률을 변수로 만들어서 처리할 예정

        CharacterData character = characterDataBase.CharacterList[UnityEngine.Random.Range(0, characterDataBase.CharacterList.Count)]; // 캐릭터DB안에 있는 캐릭터데이터 중 랜덤으로 하나 뽑고

        DrawResult result = new DrawResult
        {
            // 뽑은 데이터와 랭크를 가진 결과값 구조체를 만들어서
            character = character,
            rank = rank
        };

        OnCharacterDraw?.Invoke(result); // 이벤트를 구독하고 있는 외부에 결과 전달
    }
}
