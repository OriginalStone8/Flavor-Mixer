using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] private List<Image> iceCreamIcons;
    [SerializeField] private GameObject unlockedGroup;
    [SerializeField] private GameObject lockedGroup;

    private Order currentOrder;

    private bool unlocked;

    private void Awake() 
    {
        
    }

    private void UnlockDisplay()
    {
        unlockedGroup.SetActive(true);
        lockedGroup.SetActive(false);
        Order newOrder = OrdersManager.Instance.GenerateOrder();
        currentOrder = newOrder;
    }

    public bool isUnlocked() 
    { 
        return unlocked; 
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
        if (unlocked)
        {
            UnlockDisplay();
        }
    }
}
