using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    private static SkillData instance;
    public static SkillData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillData();
                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<int, SkillSO> dicSkills = new Dictionary<int, SkillSO>();

    public void Init()
    {
        var skillList = Resources.LoadAll<SkillSO>("Skills");

        foreach (var skill in skillList)
        {
            if (!dicSkills.ContainsKey(skill.skillKey))
            {
                dicSkills.Add(skill.skillKey, skill);
            }
        }
    }

    public SkillSO GetAllSkill(int key)
    {
        dicSkills.TryGetValue(key,out var so);
        return so;
    }

    // 모든 스킬SO 반환
    public IEnumerable<SkillSO> GetAllSkillSO()
    {
        return dicSkills.Values;
    }
}

public class Skill
{
    public int skillKey;
    public string skillName;
    public string skillDescription;
    public float skillDamage;
    public float skillCooldown;
    public Rank requiredRank; //스킬의 랭크요구치
    public bool isActive; //현재 랭크에 따라서 스킬 활성화 또는 비활성화

    public Skill(SkillSO so, Rank currentRank)
    {
        this.skillKey = so.skillKey;
        this.skillName = so.skillName;
        this.skillDescription = so.skillDescription;
        this.skillDamage = so.skillDamage;
        this.skillCooldown = so.skillCooldown;
        this.requiredRank = so.requiredRank;
        isActive = currentRank >= requiredRank;
    }

    public void UpdateSkillbyRank(Rank currentRank)
    {
        isActive = currentRank >= requiredRank;
    }
}
