using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] private List<Image> iceCreamIcons;
    [SerializeField] private GameObject unlockedGroup;
    [SerializeField] private GameObject lockedGroup;
    [SerializeField] private GameObject completedGroup;
    [SerializeField] private Button completeButton;
    [SerializeField] private TextMeshProUGUI prizeText;

    private Order currentOrder;

    public void UnlockDisplay()
    {
        PlayerPrefs.SetInt("Order" + index, 1);
        PlayerPrefs.Save();

        if (GameManagerScript.Instance.InGameMode())
        {
            //unlock while in game
            DisplaySetup(true);
            Order order = OrdersManager.Instance.GenerateOrder();
            DisplayOrder(order);
            UpdateComeplteButton();
        }
    }

    public void DisplaySetup(bool unlocked)
    {
        if (unlocked)
        {
            lockedGroup.SetActive(false);
            unlockedGroup.SetActive(true);
            completedGroup.SetActive(false);
        }
        else
        {
            lockedGroup.SetActive(true);
            unlockedGroup.SetActive(false);
            completedGroup.SetActive(false);
        }
    }

    public void DisplayOrder(Order order)
    {
        if (order == null)
        {
            foreach (Image icon in iceCreamIcons)
            {
                icon.gameObject.SetActive(false);
            }
            return;
        }

        currentOrder = order;
        for (int i = 0; i < iceCreamIcons.Count; i++)
        {
            if (i < currentOrder.GetIceCreams().Count)
            {
                iceCreamIcons[i].sprite = currentOrder.GetIceCreams()[i].icon;
                iceCreamIcons[i].color = new Color(1, 1, 1, 1);
                iceCreamIcons[i].gameObject.SetActive(true);
            }
            else
            {
                iceCreamIcons[i].sprite = OrdersManager.Instance.GetEmptyIcon();
                iceCreamIcons[i].color = new Color(1, 1, 1, 0.3f);
                iceCreamIcons[i].gameObject.SetActive(true);
            }
        }
        UpdatePrizeText();
    }

    public void UpdatePrizeText()
    {
        if (currentOrder == null)
        {
            prizeText.text = "0";
            return;
        }
        currentOrder.CalculatePrize();
        prizeText.text = currentOrder.GetPrize().ToString();
    }

    public void UpdateComeplteButton()
    {
        if (currentOrder == null)
        {
            completeButton.interactable = false;
            return;
        }
        completeButton.interactable = currentOrder.IsCompleted();
        Color fadedColor = new Color(1, 1, 1, 0.5f);
        completeButton.transform.GetChild(0).GetComponent<Image>().color = completeButton.interactable ? Color.white : fadedColor;
    }

    public void CompleteOrder()
    {
        if (currentOrder == null) return;
        MarkAsCompleted();
        List<IceCreamTypeSO> typeSOs = currentOrder.GetIceCreams();

        List<Block> blocks = new List<Block>(UnityEngine.GameObject.FindObjectsOfType<Block>());
        foreach (Block block in blocks)
        {
            if (typeSOs.Contains(block.GetIceCreamType()))
            {
                int index = typeSOs.IndexOf(block.GetIceCreamType());
                typeSOs.RemoveAt(index);
                block.ScaleOutAnimationAndDestroy(0.1f);
            }
        }
        ScoreManager.Instance.ModifyScore(currentOrder.GetPrize());
        currentOrder = null;
    }

    public int GetIndex()
    {
        return index; 
    }

    public void MarkAsCompleted()
    {
        LeanTween.scale(unlockedGroup, Vector3.zero, 0.2f).setEaseOutBack().setOnComplete(() => {
            unlockedGroup.SetActive(false);
            completedGroup.SetActive(true);
            completedGroup.transform.localScale = Vector3.zero;
            LeanTween.scale(completedGroup, Vector3.one, 0.2f).setEaseInBack();
        });
    }

    public Order GetOrder()
    {
        return currentOrder;
    }
}
