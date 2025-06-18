using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        dicSkills.TryGetValue(key, out var so);
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
    public Sprite skillImage;
    public float skillDamage;
    public readonly float skillCooldown;
    public Rank requiredRank; //스킬의 랭크요구치
    public bool isActive; //현재 랭크에 따라서 스킬 활성화 또는 비활성화
    public GameObject skillPrefab;
    public float skillRange;
    public float currentCooldown = 0;

    //private float currentCooldown = 0f;

    //public bool IsReady() => currentCooldown <= 0f;

    public Skill(SkillSO so, Rank currentRank)
    {
        this.skillKey = so.skillKey;
        this.skillName = so.skillName;
        this.skillDescription = so.skillDescription;
        this.skillImage = so.skillIcon;
        this.skillDamage = so.skillDamage;
        this.skillCooldown = so.skillCooldown;
        this.requiredRank = so.requiredRank;
        isActive = currentRank >= requiredRank;
        this.skillPrefab = so.skillPrefab;
        this.skillRange = so.skillRange;

    }

    public void UpdateSkillbyRank(Rank currentRank)
    {
        isActive = currentRank >= requiredRank;
    }


    public void UseSkill(int index, Vector3 chrPosition)
    {
        SkillSO so = SkillData.Instance.GetAllSkill(index);

        switch (index)
        {
            case (0):
                {
                    GameObject go = GameObject.Instantiate(so.skillPrefab, chrPosition + new Vector3(0, 1.5f, 0), Quaternion.identity); //프리팹 소환
                    Vector3 throwDirection = go.transform.forward + Vector3.up * 0.5f; //던지는 방향. 어떻게 던져지는지 확인안됨

                    Grenade grenade = go.GetComponent<Grenade>(); //스킬 프리팹 컴포넌트 획득
                    if (grenade != null)
                    {
                        grenade.GrenadeThrow(throwDirection, so.skillRange, so.skillDamage); //던지고 터지는건 grenade에서 처리
                    }
                    currentCooldown = skillCooldown;
                    break;
                }
            case (1):
                {
                    Vector3 spawnPosition = chrPosition + Vector3.up * 1.5f + Vector3.forward *1.5f;
                    GameObject go = GameObject.Instantiate(so.skillPrefab);
                    go.transform.position = spawnPosition;

                    FireLauncher launcher = go.GetComponent<FireLauncher>();
                    if (launcher != null)
                    {
                        launcher.Lauancher(skillDamage, skillRange);
                    }
                    currentCooldown = skillCooldown;
                }
                break;
        }
    }
}
