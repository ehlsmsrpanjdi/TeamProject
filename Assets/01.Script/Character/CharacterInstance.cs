using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


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
        this.rankInfo = data.rankInfo;

        learnedSkills = new List<Skill>(); //스킬 리스트초기화

        InitializeAllSkills();
    }

    /// <summary>
    /// 모든 스킬 습득
    /// </summary>
    private void InitializeAllSkills()
    {
        foreach (var so in SkillData.Instance.GetAllSkillSO())
        {
            var skill = new Skill(so, currentRank);
            learnedSkills.Add(skill);
        }
    }

    /// <summary>
    /// 랭크에 맞게 스킬 활성화 처리
    /// </summary>
    public void UpdateSkillActivation()
    {
        foreach (var skill in learnedSkills)
        {
            skill.UpdateSkillbyRank(currentRank);
        }
    }

    /// <summary>
    /// 현재 랭크에 맞는 스킬만 활성화 하기 위함.(CharacterBehaviour 에서 사용할 예정)
    /// </summary>
    public List<Skill> GetActiveSkills()
    {
        return learnedSkills.Where(s => s.isActive).ToList();
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

    // <summary>
    // 현재 랭크에 맞는 스킬 배우기
    // </summary>
    // public List<Skill> LearnSkillByRank(Rank rank)
    // {
    //     var skillList = new List<Skill>();
    //     var addedSkillKeys = new HashSet<int>(); // 중복 방지를 위한 키 체크
    //
    //     foreach (var info in rankInfo)
    //     {
    //         if (info.rank <= rank)
    //         {
    //             foreach (var skillKey in info.skillKey)
    //             {
    //                 if (!addedSkillKeys.Contains(skillKey))
    //                 {
    //                     var skillSO = SkillData.Instance.GetSkill(skillKey);
    //                     if (skillSO != null)
    //                     {
    //                         skillList.Add(new Skill(skillSO));
    //                         addedSkillKeys.Add(skillKey); // 중복 방지용
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //
    //     return skillList;
    // }

    /// <summary>
    /// 현재 보유중인 스킬 (시작 할 때 모든 스킬을 배우도록 하였음.)
    /// </summary>
   public List<Skill> HasSkill()
    {
        return learnedSkills;
    }

    /// <summary>
    /// 랭크가 올라갈 시 새롭게 스킬 획득 및 랭크 변환. 캐릭터 합성 시 업데이트를 위한 함수.
    /// </summary>
    public void UpdateRank(Rank newRank)
    {
        currentRank = newRank;
        //learnedSkills = LearnSkillByRank(newRank);
        UpdateSkillActivation();
    }


    /// <summary>
    /// 강화 >> CharacterManager에서 가지고 감.
    /// </summary>
    public void Enhance()
    {
        var info = GetCurrentRankInfo();

        if(info == null)
        {
            Debug.Log("강화 실패: 현재 랭크 정보가 없습니다.");
            return;
        }

        if(enhancementLevel >= info.maxenhancementLevel)
        {
            Debug.Log("최대 강화 수치 도달.");
            return;
        }

        enhancementLevel++;
        Debug.Log($"현재 강화 수치 : {enhancementLevel}");
        Debug.Log($"현재 공격력 : {GetCurrentAttack()}");
        Debug.Log($"현재 체력 : {GetCurrentHealth()}");
    }

    /// <summary>
    /// 캐릭터의 강화 수치 확인
    /// </summary>
    public int GetEnhancementLevel()
    {
        return enhancementLevel;
    }

    /// <summary>
    /// 랭크업 >> CharacterManager에서 가지고 감.
    /// </summary>
    public bool RankUp()
    {

        var rankIndex = rankInfo.FindIndex(r => r.rank == currentRank); //랭크정보를 리스트로 가지고 있어서 인덱스로 찾기

        if (rankIndex == -1 || rankIndex +1 >= rankInfo.Count) //랭크인데스 (랭크정보) 보다 많거나 적으면 랭크업 불가
        {
            Debug.Log("랭크업 불가능");
            return false;
        }

        var nextRank = rankInfo[rankIndex + 1]; //랭크 인덱스 +1 (랭크업)

        UpdateRank(nextRank.rank); // 만들어놨던 함수 사용 >> 현재 랭크에 새로운 랭크 할당 및 랭크에 맞는 스킬 활성화
        enhancementLevel = 0; // 이건 0으로 할지 아니면 기존 강화 수치를 유지 할지 논의 해봐야 함.

        return true;
    }

    CharacterBehaviour behaviour;

    public void SetBehaviour(CharacterBehaviour _behaviour)
    {
        behaviour = _behaviour;
    }

    public void ExcuteSkill(int _index)
    {
        behaviour.UseSkill(_index);
    }
}

