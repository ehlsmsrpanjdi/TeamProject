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
    GachaTableIsNull,
}

public enum GachaType
{
    Normal,
    Premium,
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

    private CharacterDataBase gachaDataBase;
    private Dictionary<GachaType, GachaTableSO> gachaTableList;
    public event Action<DrawResult> OnCharacterDraw; // 캐릭터 뽑기 시 호출되는 이벤트
    public event Action<GachaFailReason> OnGachaFail;

    public int costPerDraw = 100;


    public void Init()
    {
        gachaDataBase = Resources.Load<CharacterDataBase>("Gacha/CharacterDataBase");
        var gachaTable = Resources.Load<GachaTableDataBase>("Gacha/GachaTableList");

        gachaTableList = new Dictionary<GachaType, GachaTableSO>();
        if (gachaTable != null)
        {
            foreach (var mapping in gachaTable.gachaRateMappingList)
            {
                if (!gachaTableList.ContainsKey(mapping.type))
                {
                    gachaTableList.Add(mapping.type, mapping.rateTable);
                }
            }
        }
        else
        {
            DebugHelper.Log("gachaTable is null", gachaTable);;
        }
    }


    // 가챠 버튼 등을 눌러서 호출
    public List<DrawResult> DrawCharacter(GachaType type, int times)
    {
        if (times <= 0)
        {
            return new List<DrawResult>();
        }

        if (!gachaTableList.TryGetValue(type, out GachaTableSO gachaTable))
        {
            DebugHelper.Log("gachaTable is null", gachaTable);
            OnGachaFail?.Invoke(GachaFailReason.GachaTableIsNull);
            return new List<DrawResult>();
        }

        int currentCostPerDraw = costPerDraw;
        
        switch (type)
        {
            case GachaType.Normal:
                currentCostPerDraw = costPerDraw;
                break;
            case GachaType.Premium:
                currentCostPerDraw = costPerDraw * 2;
                break;
        }
        int totalCost = times * currentCostPerDraw;

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
            float rateSum = 0f;
            Rank rankToDraw = Rank.C;

            if (gachaTable.gachaRateList.Count > 0)
            {
                rankToDraw = gachaTable.gachaRateList[0].rank;
            }

            foreach (var rate in gachaTable.gachaRateList)
            {
                rateSum += rate.rate;
                if (random < rateSum)
                {
                    rankToDraw = rate.rank;
                    break;
                }
            }

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
                DebugHelper.Log("candidateList is null", gachaTable);
                continue;
            }

            CharacterDataSO drawncharacterSO = candidateList[UnityEngine.Random.Range(0, candidateList.Count)];

            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.CreateCharacter(drawncharacterSO.key);
            }

            DrawResult result = new DrawResult
            {
                character = drawncharacterSO,
                rank = rankToDraw
            };

            resultList.Add(result);
            DebugHelper.Log($"{type} 뽑기 결과: {result.character.characterName} {result.rank}", result.character);
            OnCharacterDraw?.Invoke(result);
        }
        SoundManager.Instance.PlaySFX(SfxType.Gacha, -1);
        return resultList;
    }

}
