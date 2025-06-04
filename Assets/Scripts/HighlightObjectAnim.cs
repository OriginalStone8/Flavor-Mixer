using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightObjectAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float size;
    [SerializeField] private float growBy;
    [SerializeField] private float animTime = 0.2f;
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private bool hasElement;

    private void Start()
    {
        if (hasElement)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasElement)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if (GetComponent<Button>() != null)
        {
            if (!GetComponent<Button>().interactable)
            {
                return; // Do not animate if the button is not interactable
            }
        }
        gameObject.transform.localScale = new Vector3(size, size, 0);
        LeanTween.scale(gameObject, new Vector3(size, size, 1) * growBy, animTime).setEase(easeType).setOnComplete(() =>
        {
            gameObject.transform.localScale = new Vector3(size, size, 0) * growBy;
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hasElement)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (GetComponent<Button>() != null)
        {
            if (!GetComponent<Button>().interactable)
            {
                return; // Do not animate if the button is not interactable
            }
        }

        gameObject.transform.localScale = new Vector3(size, size, 0) * growBy;
        LeanTween.scale(gameObject, new Vector3(size, size, 0), animTime).setEase(easeType).setOnComplete(() => {
            gameObject.transform.localScale = new Vector3(size, size, 0);
        });
    }
}