using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public struct DrawResult
{
    public CharacterDataSO character;
    public Rank rank;
}

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;
    public CharacterDataBase characterDataBase;
    public event Action<DrawResult> OnCharacterDraw; // 캐릭터 뽑기 시 호출되는 이벤트

    private DrawResult lastDrawResult;

    public List<DrawResult> drawnCharacter = new List<DrawResult>(); // DrawResult 구조체로 구성된 새로운 리스트 선언, 뽑은 캐릭터가 저장됨.

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
    public DrawResult DrawCharacter()
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

        List<CharacterDataSO> candidateList = null;
        switch (rank)
        {
            case Rank.SSS:
                candidateList = characterDataBase.SSSCharacterList;
                break;
            case Rank.SS:
                candidateList = characterDataBase.SSCharacterList;
                break;
            case Rank.S:
                candidateList = characterDataBase.SCharacterList;
                break;
            case Rank.A:
                candidateList = characterDataBase.ACharacterList;
                break;
            case Rank.B:
                candidateList = characterDataBase.BCharacterList;
                break;
            case Rank.C:
                candidateList = characterDataBase.CCharacterList;
                break;
        }

        if (candidateList == null || candidateList.Count == 0)
        {
            return default;
        }
        CharacterDataSO drawncharacterSO = candidateList[UnityEngine.Random.Range(0, candidateList.Count)];

        DrawResult result = new DrawResult
        {
            character = drawncharacterSO,
            rank = rank
        };
        this.lastDrawResult = result;

        OnCharacterDraw?.Invoke(result);
        return result;
    }

    public List<DrawResult> TenTimesDraw()
    {
        List<DrawResult> resultList = new List<DrawResult>();

        // 가챠 10회 반복
        for (int i = 0; i < 10; i++)
        {
            resultList.Add(DrawCharacter());
        }
        return resultList;
    }

}
