using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;
    public CurrencyManager currency;
    public GachaManager gacha;
    public UIManager ui;
    public WaveManager wave;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            player = FindObjectOfType<Player>();
            currency = FindObjectOfType<CurrencyManager>();
            gacha = FindObjectOfType<GachaManager>();
            ui = FindObjectOfType<UIManager>();
            wave = FindObjectOfType<WaveManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
