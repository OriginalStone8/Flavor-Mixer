using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeShopItem : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private GameObject unlockedGroup, lockedGroup;
    [SerializeField] private string currency;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    public void LockItem()
    {
        lockedGroup.SetActive(true);
        unlockedGroup.SetActive(false);
    }

    public void UnlockItem()
    {
        lockedGroup.SetActive(false);
        unlockedGroup.SetActive(true);
        priceText.text = price.ToString();
    }

    public int GetIndex()
    {
        return index;
    }

    public int GetPrice()
    {
        return price;
    }

    public string GetCurrency()
    {
        return currency;
    }

    public void ToggleBuyButton(bool on)
    {
        buyButton.interactable = on;
    }
}
