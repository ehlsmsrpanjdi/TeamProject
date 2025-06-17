using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DrawResult // 뽑기 결과를 담는 구조체
{
    public CharacterDataSO character; // 뽑힌 캐릭터의 데이터 (체력 등)
    public Rank rank; // 뽑힌 캐릭터의 랭크
}

public enum GachaFailReason // 뽑기 실패 원인
{
    NotEnoughDiamond,
    InventoryFull,
    GachaTableIsNull,
}

public enum GachaType // 가챠 종류
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

    private CharacterDataBase gachaDataBase; // 가챠로 뽑을 수 있는 캐릭터 데이터를 모아놓은 DB
    private Dictionary<GachaType, GachaTableSO> gachaTableList; // 가챠 종류별 확률 테이블을 저장하는 딕셔너리
    public event Action<DrawResult> OnCharacterDraw; // 캐릭터 뽑기 시 호출되는 이벤트
    public event Action<DrawResult> OnOverSRankDraw; // S Rank 이상 캐릭터를 뽑았을 시 호출되는 이벤트
    public event Action<GachaFailReason> OnGachaFail; // 캐릭터 뽑기 실패 시 호출되는 이벤트

    public int costPerDraw = 100;


    public void Init()
    {
        gachaDataBase = Resources.Load<CharacterDataBase>("Gacha/CharacterDataBase");
        var gachaTable = Resources.Load<GachaTableDataBase>("Gacha/GachaTableList"); // 리소스에서 SO 로드

        gachaTableList = new Dictionary<GachaType, GachaTableSO>();
        if (gachaTable != null)
        {
            foreach (var mapping in gachaTable.gachaRateMappingList)
            {
                if (!gachaTableList.ContainsKey(mapping.type))
                {
                    gachaTableList.Add(mapping.type, mapping.rateTable); // 각 가챠 타입에 해당하는 확률 테이블을 딕셔너리에 추가
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
            // 가챠 종류에 따라 1회 뽑기 비용 설정
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
            OnGachaFail?.Invoke(GachaFailReason.NotEnoughDiamond); // 다이아가 부족하면 실패 이벤트 호출
            return new List<DrawResult>();
        }

        List<DrawResult> resultList = new List<DrawResult>(); // 뽑기 결과를 저장할 리스트


        for (int i = 0; i < times; i++) // times만큼 가챠 반복
        {
            // 캐릭터의 랭크를 정함
            float random = UnityEngine.Random.Range(0f, 100f);
            float rateSum = 0f;
            Rank rankToDraw = Rank.C;

            if (gachaTable.gachaRateList.Count > 0)
            {
                rankToDraw = gachaTable.gachaRateList[0].rank;
            }

            foreach (var rate in gachaTable.gachaRateList) // 가챠 확률 리스트를 순회하며
            {
                rateSum += rate.rate; // 현재 랭크의 확률을 누적하여 더하고
                if (random < rateSum) // 0부터 100 사이의 랜덤값이 확률 누적값보다 작으면 해당 랭크 당첨
                {
                    rankToDraw = rate.rank;
                    break;
                }
            }

            List<CharacterDataSO> candidateList = null; // 앞서 뽑힌 랭크에 해당하는 캐릭터 리스트

            switch (rankToDraw) // 뽑힌 랭크에 따라 해당 랭크의 캐릭터 리스트를 가져와서
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

            CharacterDataSO drawncharacterSO = candidateList[UnityEngine.Random.Range(0, candidateList.Count)]; // 해당 랭크의 캐릭터 리스트 중 랜덤으로 하나를 선택하여

            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.CreateCharacter(drawncharacterSO.key); // 그 캐릭터의 인스턴스를 생성
            }

            DrawResult result = new DrawResult // 뽑기 결과 구조체를 생성하고
            {
                character = drawncharacterSO,
                rank = rankToDraw
            };

            resultList.Add(result); // 결과 리스트에 추가한다.
            DebugHelper.Log($"{type} 뽑기 결과: {result.character.characterName} {result.rank}", result.character);
            OnCharacterDraw?.Invoke(result);

            if (IsOverSRank(result)) // 만약 뽑힌 캐릭터가 S Rank 이상이라면
            {
                OnOverSRankDraw?.Invoke(result); // S Rank 이상 캐릭터 뽑기 이벤트를 호출한다.
            }
        }
        SoundManager.Instance.PlaySFX(SfxType.Gacha, -1); // 가챠 효과음 재생
        return resultList; // 최종 뽑기 결과 리스트를 반환한다.
    }

    public bool IsOverSRank(DrawResult result)
    {
        // 캐릭터 랭크가 S, SS, SSS 중 하나라면 true 반환
        if (result.character == null)
        {
            return false;
        }
        return result.character.startRank == Rank.S || result.character.startRank == Rank.SS || result.character.startRank == Rank.SSS;
    }
}
