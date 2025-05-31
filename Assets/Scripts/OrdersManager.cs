using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public static OrdersManager Instance { get; private set; }
    [SerializeField] private List<OrderDisplay> orderDisplays;
    [SerializeField] private GameObject popup;
    [SerializeField] private int maxPerOrder;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        SceneManagement.Instance.OnGameStart += SetupAllDisplays;
        popup.SetActive(false);
    }

    private void SetupAllDisplays(object sender, EventArgs e)
    {
        foreach (OrderDisplay display in orderDisplays)
        {
            bool unlocked = PlayerPrefs.GetInt("Order" + display.GetIndex()) == 1 || display == orderDisplays[0];
            display.DisplaySetup(unlocked);
            if (unlocked)
            {
                Order order = GenerateOrder();
                display.DisplayOrder(order);
            }
            else
            {
                display.DisplayOrder(null);
            }
        }
        UpdateAllDisplayButtons();
        SceneManagement.Instance.OpenPopup(popup);
    }

    public void UpdateAllDisplayButtons()
    {
        foreach (OrderDisplay display in orderDisplays)
        {
            display.UpdateComeplteButton();
        }
    }

    public void CloseOrderPopup()
    {
        SceneManagement.Instance.ClosePopup(popup);
    }

    public Order GenerateOrder()
    {
        Order order = new();
        return order;
    }

    public int GetMaxPerOrder()
    {
        return maxPerOrder;
    }

    public bool AnyCompletedOrders()
    {
        foreach (OrderDisplay display in orderDisplays)
        {
            if (display.GetOrder() != null && display.GetOrder().IsCompleted())
            {
                return true;
            }
        }
        return false;
    }
}
