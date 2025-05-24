using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private IceCreamTypeSO iceCreamType;
    private Tile currentTile;

    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanMerge(Block otherBlock)
    {
        if (otherBlock == null) return false;
        return iceCreamType.index == otherBlock.iceCreamType.index;
    }

    public void MergeWith()
    {
        if (GameManagerScript.Instance.GetIceCreamTypeCount() - 1 == iceCreamType.index)
        {
            Debug.Log("No Ice Cream Types left to merge with");
            SetIceCreamType(iceCreamType);

            LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0) * 1.2f, 0.1f).setEaseOutBack().setOnComplete(() =>
            {
                LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0), 0.1f);
            });
            return;
        }
        IceCreamTypeSO newType = GameManagerScript.Instance.GetNextIceCreamType(iceCreamType);
        SetIceCreamType(newType);

        LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0) * 1.2f, 0.1f).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0), 0.1f);
        });
    }

    public void SetIceCreamType(IceCreamTypeSO iceCreamType)
    {
        this.iceCreamType = iceCreamType;
        spriteRenderer.sprite = iceCreamType.sprite;
    }

    public IceCreamTypeSO GetIceCreamType()
    {
        return iceCreamType;
    }

    public void setCurrentTile(Tile tile)
    {
        currentTile = tile;
    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }
}
