using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public event Action<int> OnGoldChanged;
    public event Action<int> OnDiamondChanged;

    private PlayerData _playerData => Player.Instance.Data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            _playerData.gold += amount;
            OnGoldChanged?.Invoke(_playerData.gold);
        }
    }

    public bool UseGold(int amount)
    {
        if (_playerData.gold >= amount)
        {
            _playerData.gold -= amount;
            OnGoldChanged?.Invoke(_playerData.gold);
            return true;
        }
        return false;
    }

    public void AddDiamond(int amount)
    {
        if (amount > 0)
        {
            _playerData.diamond += amount;
            OnDiamondChanged?.Invoke(_playerData.diamond);
        }
    }

    public bool UseDiamond(int amount)
    {
        if (_playerData.diamond >= amount)
        {
            _playerData.diamond -= amount;
            OnDiamondChanged?.Invoke(_playerData.diamond);
            return true;
        }
        return false;
    }
}
