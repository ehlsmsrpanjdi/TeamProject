using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "ScriptableObjects/CharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
    public List<CharacterDataSO> SSSCharacterList;
    public List<CharacterDataSO> SSCharacterList;
    public List<CharacterDataSO> SCharacterList;
    public List<CharacterDataSO> ACharacterList;
    public List<CharacterDataSO> BCharacterList;
    public List<CharacterDataSO> CCharacterList;
}
