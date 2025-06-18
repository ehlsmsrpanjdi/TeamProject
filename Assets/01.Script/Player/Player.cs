using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

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

    public event Action<int> OnGoldChanged; // 골드 변경 시 호출되는 이벤트
    public event Action<int> OnDiamondChanged; // 다이아 변경 시 호출되는 이벤트

    private string saveFilePath; // Player 데이터가 저장될 파일 경로

    private void Init()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        LoadPlayerData(); // 플레이어 데이터 로드
    }

    public void AddGold(int amount)
    {
        // 골드 획득 메서드
        if (amount > 0)
        {
            Data.gold += amount;
            OnGoldChanged?.Invoke(Data.gold);
            SavePlayerData();
        }
    }

    public bool UseGold(int amount)
    {
        // 골드 사용 메서드
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
        // 다이아 획득 메서드
        if (amount > 0)
        {
            Data.diamond += amount;
            OnDiamondChanged?.Invoke(Data.diamond);
            SavePlayerData();
        }
    }

    public bool UseDiamond(int amount)
    {
        // 다이아 사용 메서드
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
        // 플레이어 데이터 로드
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Data = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // 파일이 없으면 기본값의 PlayerData 생성
            Data = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        // 플레이어 데이터 저장
        SaveCharacters(CharacterManager.Instance.GetAllCharacters());
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(saveFilePath, json);
    }


    //플레이어 데이터를 전부 저장하지 않고 캐릭터 목록만 저장할 필요가 있을 경우 주석해제 후 사용
    public void SaveCharacters(List<CharacterInstance> characters)
    {
        // 현재 보유한 캐릭터 목록을 저장
        Data.characterInstances.Clear();
        foreach (var character in characters)
        {
            Data.characterInstances.Add(new CharacterSaveData(character));
        }
    }

    public List<CharacterInstance> LoadCharacters()
    {
        // 보유한 캐릭터 목록을 로드
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

    public void ResetCharacterData()
    {
        Data.characterInstances.Clear();
        SavePlayerData();
    }
}
