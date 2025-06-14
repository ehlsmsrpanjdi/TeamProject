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

    public event Action<int> OnGoldChanged;
    public event Action<int> OnDiamondChanged;

    //private string saveFilePath;

    private void Init()
    {
        //saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        //LoadPlayerData();
        Data = new PlayerData();
    }

    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            Data.gold += amount;
            OnGoldChanged?.Invoke(Data.gold);
        }
    }

    public bool UseGold(int amount)
    {
        if (Data.gold >= amount)
        {
            Data.gold -= amount;
            OnGoldChanged?.Invoke(Data.gold);
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
        }
    }

    public bool UseDiamond(int amount)
    {
        if (Data.diamond >= amount)
        {
            Data.diamond -= amount;
            OnDiamondChanged?.Invoke(Data.diamond);
            return true;
        }
        return false;
    }

    /*public void LoadPlayerData()
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
    }*/

    /*public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(saveFilePath, json);
    }*/
}
