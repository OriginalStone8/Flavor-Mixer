using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemManager : MonoBehaviour
{
    public static ShopItemManager Instance { get; private set; }
    [SerializeField] private List<OneTimeShopItem> shopItems;

    private void Awake() 
    {
        Instance = this;
    }

    public void UpdateItems()
    {
        foreach (OneTimeShopItem item in shopItems)
        {
            if (PlayerPrefs.GetInt("OneTimeShopItem" + item.GetIndex()) == 1)
            {
                item.LockItem();
            }
            else
            {
                item.UnlockItem();
            }
        }
    }

    public void UpdateItemsButton()
    {
        foreach (OneTimeShopItem item in shopItems)
        {
            bool canAfford = CurrencyManager.Instance.canAfford(item.GetPrice(), item.GetCurrency());
            item.ToggleBuyButton(canAfford);
        }
    }

    public int GetItemPrice(int index)
    {
        return shopItems[index].GetPrice();
    }

    public string GetItemCurrency(int index)
    {
        return shopItems[index].GetCurrency();
    }

    public void LockItemState(int index)
    {
        PlayerPrefs.SetInt("OneTimeShopItem" + index, 1);
    }
}
