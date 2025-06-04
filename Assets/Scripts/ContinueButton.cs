using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private Sprite coinIcon, gemIcon;

    public void UpdateButtonInteractability()
    {
        string currencyType = ContinueGame.Instance.IsCoins() ? "Coins" : "Gems";
        int price = currencyType == "Coins" ? ContinueGame.Instance.GetCoinCurrentPrice() : ContinueGame.Instance.GetGemCurrentPrice();
        bool canAfford = CurrencyManager.Instance.CanAfford(price, currencyType);
        GetComponent<Button>().interactable = canAfford;
        UpdateButtonCurrencyDisplay(price, currencyType);
    }

    private void UpdateButtonCurrencyDisplay(int price, string currencyType)
    {
        priceText.text = price.ToString();
        if (currencyType == "Coins")
        {
            currencyIcon.sprite = coinIcon;
        }
        else if (currencyType == "Gems")
        {
            currencyIcon.sprite = gemIcon;
        }
        else
        {
            Debug.LogError("Unknown currency type: " + currencyType);
        }
    }

    public void Continue()
    {
        if (ContinueGame.Instance.IsCoins())
        {
            CurrencyManager.Instance.ModifyCoins(-ContinueGame.Instance.GetCoinCurrentPrice());
            ContinueGame.Instance.IncreaseCoinPrice();
        }
        else
        {
            CurrencyManager.Instance.ModifyGems(-ContinueGame.Instance.GetGemCurrentPrice());
            ContinueGame.Instance.IncreaseGemPrice();
        }
        ContinueGame.Instance.ToggleIsCoins();
        SceneManagement.Instance.ContinuePlaying();
    }
}
