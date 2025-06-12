using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Rank Info")]
public class RankInfoSO : ScriptableObject
{
    public Rank rank;
    public float attackMultiplier; //랭크업 당 증가 되는 기본 공격력 배율
    public float hpMultiplier; //랭크업 당 증가 되는 기본 체력 배율
    public List<Skill> Skills;
    public int requiredOwnedCount; //랭크업 시 필요한 개수

}
