using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float size;
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
        return iceCreamType.index == otherBlock.iceCreamType.index && iceCreamType.index != GameManagerScript.Instance.GetIceCreamTypeCount() - 1;
    }

    public void IsExtraBlock()
    {
        if (currentTile.GetCurrentBlock() != this)
        {
            Debug.Log("Block is not on the current tile, destroying");
            Destroy(gameObject);
        }
    }

    public void MergeWith()
    {
        if (GameManagerScript.Instance.GetIceCreamTypeCount() - 1 == iceCreamType.index)
        {
            Debug.Log("No Ice Cream Types left to merge with");
            SetIceCreamType(iceCreamType);

            ScaleAnimation(1.2f);
            return;
        }
        IceCreamTypeSO newType = GameManagerScript.Instance.GetNextIceCreamType(iceCreamType);
        SetIceCreamType(newType);

        ScaleAnimation(1.2f);
    }

    public void ScaleAnimation(float scale)
    {
        LeanTween.scale(gameObject, new Vector3(size, size, 0) * scale, 0.1f).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, new Vector3(size, size, 0), 0.1f);
        });
    }

    public void ScaleInAnimation()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(size, size, 0), 0.1f).setEaseOutBack();
    }

    public void ScaleOutAnimation(float time)
    {
        transform.localScale = new Vector3(size, size, 0);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), time).setEaseOutBack();
    }

    public void ScaleOutAnimationAndDestroy(float time)
    {
        transform.localScale = new Vector3(size, size, 0);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), time).setEaseOutBack().setOnComplete(() =>
        {
            currentTile.ClearTile();
            Destroy(gameObject);
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
