using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold;
    public int diamond;
    public int currentStage;

    public List<CharacterInstance> ownedCharacters;
    public List<CharacterInstance> combatCharacters;

    public PlayerData()
    {
        gold = 0;
        diamond = 0;
        currentStage = 0;
        ownedCharacters = new List<CharacterInstance>();
        combatCharacters = new List<CharacterInstance>();
    }
}
