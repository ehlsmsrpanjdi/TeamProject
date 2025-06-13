using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;
    public GachaManager gacha;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            player = FindObjectOfType<Player>();
            gacha = FindObjectOfType<GachaManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
