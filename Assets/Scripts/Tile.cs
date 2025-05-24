using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
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
