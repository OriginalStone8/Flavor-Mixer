using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public static CurrencyDisplay Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText, gemText;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
        UpdateDisplay();
    }

    private void UpdateCurrencyDisplay(object sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        coinText.text = CurrencyManager.Instance.GetCoinCount().ToString();
        gemText.text = CurrencyManager.Instance.GetGemCount().ToString();
    }
}
