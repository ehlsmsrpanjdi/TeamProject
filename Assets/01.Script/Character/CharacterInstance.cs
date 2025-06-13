using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 실제 캐릭터가 가지는 데이터 >> 여기서 강화 후 수치 변경이 일어남.
/// </summary>
public class CharacterInstance
{
    public int key;
    public string charcterName;
    public Sprite characterImage;
    public float baseAttack;
    public float baseHealth;
    public Rank currentRank;
    public int enhancementLevel;
    public List<RankInfo> rankInfo;
    public List<Skill> learnedSkills;
    public GameObject charPrefab;

    public CharacterInstance(CharacterDataSO data)
    {
        this.key = data.key;
        this.charcterName = data.characterName;
        this.characterImage = data.characterImage;
        this.baseAttack = data.baseAttack;
        this.baseHealth = data.baseHealth;
        this.currentRank = data.startRank;
        this.enhancementLevel = data.enhancementLevel;
        charPrefab = data.characterPrefab;
        rankInfo = new List<RankInfo>(data.rankInfo);

        learnedSkills = LearnSkillByRank(currentRank);
    }

    /// <summary>
    /// 현재 랭크에 해당하는 RankInfo 가져오기
    /// </summary>
    private RankInfo GetCurrentRankInfo()
    {
        return rankInfo.FirstOrDefault(r => r.rank == currentRank);
    }

    /// <summary>
    /// 강화 보너스 계산 (예: 강화 1당 5% 증가)
    /// </summary>
    private float GetEnhancementBonus()
    {
        return 1f + 0.05f * enhancementLevel; // 5%씩 증가
    }

    /// <summary>
    /// 현재 공격력 계산
    /// </summary>
    public float GetCurrentAttack()
    {
        var rankInfo = GetCurrentRankInfo();
        if (rankInfo == null) return baseAttack;

        return Mathf.RoundToInt(baseAttack * rankInfo.attackMultiplier * GetEnhancementBonus());
    }

    /// <summary>
    /// 현재 체력 계산
    /// </summary>
    public float GetCurrentHealth()
    {
        var rankInfo = GetCurrentRankInfo();
        if (rankInfo == null) return baseHealth;

        return Mathf.RoundToInt(baseHealth * rankInfo.hpMultiplier * GetEnhancementBonus());
    }

    /// <summary>
    /// 현재 랭크에 맞는 스킬 배우기
    /// </summary>
    public List<Skill> LearnSkillByRank(Rank rank)
    {
        var allSkills = new List<Skill>();

        foreach (var info in rankInfo)
        {
            if (info.rank <= currentRank)
            {
                foreach (var skill in info.Skills)
                {
                    if (!allSkills.Contains(skill))
                        allSkills.Add(skill);
                }
            }
        }
        return allSkills;
    }

    /// <summary>
    /// 현재 보유중인 스킬
    /// </summary>
   public List<Skill> HasSkill()
    {
        return learnedSkills;
    }

    /// <summary>
    /// 랭크가 올라갈 시 새롭게 스킬 획득 및 랭크 변환
    /// </summary>
    public void UpdateRank(Rank newRank)
    {
        currentRank = newRank;
        learnedSkills = LearnSkillByRank(newRank);
    }
}

