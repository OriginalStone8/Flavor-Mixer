using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightObjectAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float size;
    [SerializeField] private float growBy;
    [SerializeField] private float animTime = 0.2f;
    [SerializeField] private LeanTweenType easeType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(size, size, 0);
        LeanTween.scale(gameObject, new Vector3(size, size, 1) * growBy, animTime).setEase(easeType).setOnComplete(() => {
            gameObject.transform.localScale = new Vector3(size, size, 0) * growBy;
        });
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(size, size, 0) * growBy;
        LeanTween.scale(gameObject, new Vector3(size, size, 0), animTime).setEase(easeType).setOnComplete(() => {
            gameObject.transform.localScale = new Vector3(size, size, 0);
        });
    }
}