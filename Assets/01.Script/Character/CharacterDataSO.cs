using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Rank
{
    C,
    B,
    A,
    S,
    SS,
    SSS
}

[CreateAssetMenu(fileName = "New Character", menuName = "Character/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public Sprite characterImage; // 캐릭터 이미지

    public int baseAttack; // 기본 공격력
    public int baseHealth; // 기본 체력
    public Rank startRank; // 시작 랭크

    public List<RankInfoSO> rankInfo;
}
