using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Character/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public float skillDamage;
    public float skillCooldown;
}
