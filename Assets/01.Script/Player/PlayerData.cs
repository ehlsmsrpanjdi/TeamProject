using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold;
    public int diamond;
    public int currentWave;
    public List<CharacterSaveData> characterInstances;
    public List<int> particpateCharacterKeys;

    public PlayerData()
    {
        gold = 0;
        diamond = 0;
        currentWave = 1;
        characterInstances = new List<CharacterSaveData>();
        particpateCharacterKeys = new List<int>();
    }
}

[System.Serializable]
public class CharacterSaveData
{
    public int key;
    public Rank currentRank;
    public int enhancementlevel;

    public CharacterSaveData(CharacterInstance character)
    {
        key = character.key;
        currentRank = character.currentRank;
        enhancementlevel = character.enhancementLevel;
    }
}
