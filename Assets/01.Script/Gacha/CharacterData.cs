using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    // 캐릭터를 정의하는 데이터(테스트 용)
    public string Name;
    public Sprite Sprite;
    // public string Description 등
}
