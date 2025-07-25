using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character/Skill")]
public class SkillSO : ScriptableObject
{
    public int skillKey;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float skillDamage;
    public float skillCooldown;
    public GameObject skillPrefab;
    public float skillRange;


    public Rank requiredRank;
    //스킬 범위 추가 가능
}
