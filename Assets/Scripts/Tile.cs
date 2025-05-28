using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayerMask;
    [SerializeField] private int index;
    private bool isOccupied;
    private Block block;
    private float x, y;

    private void Awake() 
    {
        isOccupied = false;
        block = null;
        SetGridPosition(transform.position.x, transform.position.y);
    }

    public bool PropertiesAreSet()
    {
        bool triggered = Physics2D.OverlapCircle(transform.position, 0.2f, blockLayerMask);
        if (triggered && !isOccupied)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, blockLayerMask);
            if (collider != null)
            {
                Block newBlock = collider.GetComponent<Block>();
                if (newBlock != null)
                {
                    SetCurrentBlock(newBlock);
                    SetIsOccupied(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else if (!triggered && isOccupied)
        {
            ClearTile();
            return true;
        }
        else if ((triggered && isOccupied) || (!triggered && !isOccupied))
        {
            // No change in state, do nothing
            return true;
        }
        return false;
    }

    public void SetGridPosition(float x, float y)
    {   
        this.x = x;
        this.y = y;
    }

    public float GetX() { return x; }
    public float GetY() { return y; }

    public void SetCurrentBlock(Block block)
    {
        this.block = block;
    }

    public Block GetCurrentBlock()
    {
        return block;
    }

    public void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void ClearTile()
    {
        block = null;
        isOccupied = false;
    }

    public int GetIndex()
    {
        return index;
    }
}
