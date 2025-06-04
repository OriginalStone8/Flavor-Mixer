using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateButtonElements : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttonElements;

    private void Update()
    {
        UpdateElements();
    }

    public void UpdateElements()
    {
        bool on = GetComponent<Button>().interactable;
        foreach (GameObject obj in buttonElements)
        {
            Image image = obj.GetComponent<Image>();
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            Color fadedColor;
            if (image != null)
            {
                fadedColor = new Color(1, 1, 1, 0.5f);
                image.color = on ? Color.white : fadedColor;
            }
            else if (text != null)
            {
                fadedColor = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
                Color normalColor = new Color(text.color.r, text.color.g, text.color.b, 1f);
                text.color = on ? normalColor : fadedColor;
            }
        }
    }
}
