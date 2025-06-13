using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public PlayerData Data { get; private set; }

    //private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
            //LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
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

    //private void OnApplicationQuit()
    //{
    //    SavePlayerData();
    //}
}
