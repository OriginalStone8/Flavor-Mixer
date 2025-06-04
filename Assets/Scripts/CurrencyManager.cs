using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public const string COIN_COUNT_KEY = "CoinCount";
    public const string GEM_COUNT_KEY = "GemCount";

    public event EventHandler OnCurrencyChanged;

    private void Awake() 
    {
        Instance = this;
    }

    public int GetCoinCount()
    {
        return PlayerPrefs.GetInt(COIN_COUNT_KEY);
    }

    public int GetGemCount()
    {
        return PlayerPrefs.GetInt(GEM_COUNT_KEY);
    }

    public void ModifyCoins(int amount)
    {
        int coinCount = GetCoinCount() + amount;
        PlayerPrefs.SetInt(COIN_COUNT_KEY, coinCount);
        OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ModifyGems(int amount)
    {
        int gemCount = GetGemCount() + amount;
        PlayerPrefs.SetInt(GEM_COUNT_KEY, gemCount);
        OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanAfford(int price, string currency)
    {
        if (currency.Equals("Coins"))
        {
            return GetCoinCount() >= price;
        }
        else if (currency.Equals("Gems"))
        {
            return GetGemCount() >= price;
        }
        Debug.LogError("Unknown currency type: " + currency);
        return false;
    }
}
