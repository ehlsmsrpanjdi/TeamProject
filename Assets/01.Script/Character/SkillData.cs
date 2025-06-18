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
    public Sprite skillImage;
    public float skillDamage;
    public float skillCooldown;
    public Rank requiredRank; //스킬의 랭크요구치
    public bool isActive; //현재 랭크에 따라서 스킬 활성화 또는 비활성화
    public GameObject skillPrefab;
    public float skillRange;

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

        //currentCooldown = 0f;
    }

    public void UpdateSkillbyRank(Rank currentRank)
    {
        isActive = currentRank >= requiredRank;
    }

    //public void ReduceCooldown(float cool)
    //{
    //    if (currentCooldown > 0)
    //    {
    //        currentCooldown -= cool;
    //    }
    //}

    public void UseSkill(int index, Vector3 chrPosition)
    {
        //전달받은 인덱스를 가지고 스킬을 사용함
        //스킬키 2001, 2002가 있음.
        //전달받은 스킬키에 맞는 스킬 프리팹 소환
        //던진다
        //프리팹에 달려있는 스크립트에서 물리작용 처리

        //if(!IsReady()) return;

        SkillSO so = SkillData.Instance.GetAllSkill(index);
        if(so == null)
        {
            Debug.Log($"스킬 {index} SkillSO를 찾을 수 없습니다.");
            return;
        }
        if(so.skillPrefab == null)
        {
            Debug.Log($"스킬 {index} 프리팹을 찾을 수 없습니다.");
        }

        GameObject go = GameObject.Instantiate(so.skillPrefab, chrPosition, Quaternion.identity); //프리팹 소환
        Vector3 throwDirection = go.transform.forward + Vector3.up * 0.5f; //던지는 방향. 어떻게 던져지는지 확인안됨

        Grenade grenade = go.GetComponent<Grenade>(); //스킬 프리팹 컴포넌트 획득
        if( grenade != null)
        {
            grenade.GrenadeThrow(throwDirection, so.skillRange, so.skillDamage); //던지고 터지는건 grenade에서 처리
        }

        //currentCooldown = skillCooldown;

    }


}
