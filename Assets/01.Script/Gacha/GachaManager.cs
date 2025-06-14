using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DrawResult
{
    public CharacterDataSO character;
    public Rank rank;
}

public enum GachaFailReason
{
    NotEnoughDiamond,
    InventoryFull,
}

public class GachaManager
{
    private static GachaManager instance;

    public static GachaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GachaManager();
                instance.Init();
            }
            return instance;
        }
        set { instance = value; }
    }

    public CharacterDataBase gachaDataBase;
    public event Action<DrawResult> OnCharacterDraw; // 캐릭터 뽑기 시 호출되는 이벤트
    public event Action<GachaFailReason> OnGachaFail;

    public int costPerDraw = 100;


    public void Init()
    {
        gachaDataBase = Resources.Load<CharacterDataBase>("Gacha/CharacterDataBase");
    }


    // 가챠 버튼 등을 눌러서 호출
    public List<DrawResult> DrawCharacter(int times)
    {
        if (times <= 0)
        {
            return new List<DrawResult>();
        }

        int totalCost = times * costPerDraw;
        bool drawSuccess = Player.Instance.UseDiamond(totalCost);

        if (!drawSuccess)
        {
            OnGachaFail?.Invoke(GachaFailReason.NotEnoughDiamond);
            return new List<DrawResult>();
        }

        List<DrawResult> resultList = new List<DrawResult>();


        for (int i = 0; i < times; i++)
        {
            // 캐릭터의 랭크를 정함
            float random = UnityEngine.Random.Range(0f, 100f);
            Rank rankToDraw;

            if (random < 0.1f)
            {
                rankToDraw = Rank.SSS; // 0.1%
            }
            else if (random < 1f)
            {
                rankToDraw = Rank.SS; // 0.9%
            }
            else if (random < 5f)
            {
                rankToDraw = Rank.S; // 4%
            }
            else if (random < 15f)
            {
                rankToDraw = Rank.A; // 10%
            }
            else if (random < 40f)
            {
                rankToDraw = Rank.B; // 25%
            }
            else
            {
                rankToDraw = Rank.C; // 60%
            }
            // 각 랭크별 확률을 변수로 만들어서 처리할 예정

            List<CharacterDataSO> candidateList = null;
            switch (rankToDraw)
            {
                case Rank.SSS: candidateList = gachaDataBase.SSSCharacterList; break;
                case Rank.SS: candidateList = gachaDataBase.SSCharacterList; break;
                case Rank.S: candidateList = gachaDataBase.SCharacterList; break;
                case Rank.A: candidateList = gachaDataBase.ACharacterList; break;
                case Rank.B: candidateList = gachaDataBase.BCharacterList; break;
                case Rank.C: candidateList = gachaDataBase.CCharacterList; break;
            }

            if (candidateList == null || candidateList.Count == 0)
            {
                continue;
            }

            CharacterDataSO drawncharacterSO = candidateList[UnityEngine.Random.Range(0, candidateList.Count)];

            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.CreateCharacter(drawncharacterSO.key);
            }
            else
            {
            }

            DrawResult result = new DrawResult
            {
                character = drawncharacterSO,
                rank = rankToDraw
            };

            resultList.Add(result);
            DebugHelper.Log($"뽑기 결과: {result.character.characterName} {result.rank}", result.character);
            OnCharacterDraw?.Invoke(result);
        }
        return resultList;
    }

}
