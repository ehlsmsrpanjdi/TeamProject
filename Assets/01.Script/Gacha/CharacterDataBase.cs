using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "ScriptableObjects/CharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
    public List<CharacterData> CharacterList; // CharacterData로 구성된 데이터베이스
}
