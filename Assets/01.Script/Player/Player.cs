using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class Player
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
                instance.Init();
            }
            return instance;
        }
    }

    public PlayerData Data { get; private set; }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnDiamondChanged;

    private string saveFilePath;

    private void Init()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        LoadPlayerData();
    }

    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            Data.gold += amount;
            OnGoldChanged?.Invoke(Data.gold);
            SavePlayerData();
        }
    }

    public bool UseGold(int amount)
    {
        if (Data.gold >= amount)
        {
            Data.gold -= amount;
            OnGoldChanged?.Invoke(Data.gold);
            SavePlayerData();
            return true;
        }
        return false;
    }

    public void AddDiamond(int amount)
    {
        if (amount > 0)
        {
            Data.diamond += amount;
            OnDiamondChanged?.Invoke(Data.diamond);
            SavePlayerData();
        }
    }

    public bool UseDiamond(int amount)
    {
        if (Data.diamond >= amount)
        {
            Data.diamond -= amount;
            OnDiamondChanged?.Invoke(Data.diamond);
            SavePlayerData();
            return true;
        }
        return false;
    }

    public void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Data = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Data = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(saveFilePath, json);
    }

    public void SaveCharacters(List<CharacterInstance> characters)
    {
        Data.characterInstances.Clear();
        foreach (var character in characters)
        {
            Data.characterInstances.Add(new CharacterSaveData(character));
        }
        SavePlayerData();
    }

    public List<CharacterInstance> LoadCharacters()
    {
        List<CharacterInstance> loadedCharacters = new List<CharacterInstance>();
        foreach (var character in Data.characterInstances)
        {
            CharacterDataSO charSO = CharacterData.instance.GetData(character.key);
            if (charSO != null)
            {
                CharacterInstance charInstance = new CharacterInstance(charSO);

                charInstance.UpdateRank(character.currentRank);
                charInstance.enhancementLevel = character.enhancementlevel;

                loadedCharacters.Add(charInstance);
            }
        }
        return loadedCharacters;
    }

    public void SaveParticipateCharacter(List<CharacterInstance> participateCharacters)
    {
        Data.particpateCharacterKeys.Clear();
        foreach (var character in participateCharacters)
        {
            Data.particpateCharacterKeys.Add(character.key);
        }
        SavePlayerData();
    }

    public List<int> LoadParticipateCharacter()
    {
        return Data.particpateCharacterKeys;
    }
}
