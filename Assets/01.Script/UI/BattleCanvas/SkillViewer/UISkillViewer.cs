using System.Collections.Generic;
using UnityEngine;

public class UISkillViewer : UIBase
{
    [SerializeField] GameObject SkillPrefab;

    List<UISkill> SkillList = new List<UISkill>();

    CharacterInstance SelectedCharacter;

    const string Btn_Skill = "Btn_Skill";

    private void Reset()
    {
        SkillPrefab = Resources.Load<GameObject>("UI/Btn_Skill");
    }

    private void Awake()
    {
        SpawnSkill(0);
        SpawnSkill(1);
    }

    public CharacterInstance GetCharacterInst()
    {
        return SelectedCharacter;
    }

    public void SpawnSkill(int _index)
    {
        GameObject SkillBtn_1 = Instantiate(SkillPrefab, transform);
        UISkill Skill_1 = SkillBtn_1.GetComponent<UISkill>();
        SkillList.Add(Skill_1);
        Skill_1.SetIndex(_index);
    }

    public void ResetSkill()
    {
        foreach (var item in SkillList)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ViewSkill(CharacterInstance _inst)
    {
        List<Skill> skills = _inst.GetActiveSkills(); // 현재스킬
        SelectedCharacter = _inst;
        for (int i = 0; i < skills.Count; i++)
        {
            UISkill skillUI = SkillList[i];
            Skill skill = skills[i];
            skillUI.SetCoolTime(skill.currentCooldown, skill.skillCooldown);
            skillUI.SetImage(skill.skillImage);
            skillUI.gameObject.SetActive(true);
        }
    }
}
