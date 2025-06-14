using System;
using System.Collections.Generic;
using UnityEngine;

public class UISkillViewer : UIBase
{
    [SerializeField] GameObject SkillPrefab;

    List<UISkill> SkillList = new List<UISkill>();

    const string Btn_Skill = "Btn_Skill";

    private void Reset()
    {
        SkillPrefab = Resources.Load<GameObject>("UI/Btn_Skill");
    }

    public void AddSkill()
    {
        GameObject SkillBtn = Instantiate(SkillPrefab, transform);
        UISkill Skill = SkillBtn.GetComponent<UISkill>();
        SkillList.Add(Skill);
    }
}
