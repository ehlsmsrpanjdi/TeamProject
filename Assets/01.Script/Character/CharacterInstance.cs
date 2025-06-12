using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance
{
    public CharacterDataSO characterData;

    public int enhancementLevel = 0; // 강화 레벨 (랭크와 별도)

    public Rank currentRank = Rank.C; // 초기 시작 랭크

    public int CurrentAttack => Mathf.RoundToInt(
        characterData.baseAttack * GetRankInfo().attackMultiplier + GetEnhancementBonus()
    );

    public int CurrentHP => Mathf.RoundToInt(
        characterData.baseHealth * GetRankInfo().hpMultiplier
    );

    public List<Skill> CurrentSkills => GetRankInfo().Skills;


    public RankInfoSO GetRankInfo()
    {
        //랭크 정보 가지고 오기.
        return characterData.rankInfo.Find(info => info.rank == currentRank);
    }

    public void SetRank(Rank newRank)
    {
        currentRank = newRank; //currentRank에 새로운 랭크 넣어주기
    }

    private float GetEnhancementBonus()
    {
        // 강화 1레벨당 5 공격력 증가. 고정 수치지만 조정 가능
        return enhancementLevel * 5f;
    }

    public void Enhance()
    {
        enhancementLevel++;
        // 강화 효과 애니메이션/사운드 등은 외부에서 처리 (여기에 Action 사용?)
    }


}
