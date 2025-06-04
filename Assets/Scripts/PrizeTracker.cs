using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrizeTracker : MonoBehaviour
{
    public static PrizeTracker Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinRewardText, gemRewardText;

    [SerializeField] private List<IceCreamTypeSO> iceCreamTypes;
    private List<IceCreamTypeSO> reached;
    private int coinRewardSum, gemRewardSum;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MovementManager.Instance.OnMerge += OnMerge;
        reached = new List<IceCreamTypeSO>();
    }

    private void OnMerge(object sender, MovementManager.OnMergeEventArgs e)
    {
        if (!reached.Contains(e.MergedBlock.GetIceCreamType()))
        {
            reached.Add(e.MergedBlock.GetIceCreamType());
            Debug.Log("Reached new ice cream type: " + e.MergedBlock.GetIceCreamType().name);
            coinRewardSum += e.MergedBlock.GetIceCreamType().rewardCoins;
            gemRewardSum += e.MergedBlock.GetIceCreamType().rewardGems;
            CurrencyManager.Instance.ModifyCoins(e.MergedBlock.GetIceCreamType().rewardCoins);
            CurrencyManager.Instance.ModifyGems(e.MergedBlock.GetIceCreamType().rewardGems);
        }
    }

    public int GetCoinRewardSum()
    {
        int coinRewardSum = 0;
        foreach (IceCreamTypeSO iceCreamType in reached)
        {
            coinRewardSum += iceCreamType.rewardCoins;
        }
        if (this.coinRewardSum != coinRewardSum)
        {
            Debug.Log("Coin reward sum was wrong");
        }
        return coinRewardSum;
    }

    public int GetGemRewardSum()
    {
        int gemRewardSum = 0;
        foreach (IceCreamTypeSO iceCreamType in reached)
        {
            gemRewardSum += iceCreamType.rewardGems;
        }
        if (this.gemRewardSum != gemRewardSum)
        {
            Debug.Log("Gem reward sum was wrong");
        }
        return gemRewardSum;
    }

    public void SetRewardTexts()
    {
        coinRewardText.text = GetCoinRewardSum().ToString();
        gemRewardText.text = GetGemRewardSum().ToString();
    }
}
