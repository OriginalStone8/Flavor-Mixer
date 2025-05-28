using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; } 
    [SerializeField] private Tile[] tiles;

    private List<List<Tile>> rows = new List<List<Tile>>();
    private List<List<Tile>> columns = new List<List<Tile>>();

    private void Awake() 
    {
        Instance = this;
        GroupTiles();
    }

    private void GroupTiles()
    {
        for (int y = 0; y < 4; y++)
        {
            List<Tile> row = new List<Tile>();
            List<Tile> column = new List<Tile>();
            for (int x = 0; x < 4; x++)
            {
                Tile tile = tiles[y * 4 + x];
                row.Add(tile);
                column.Add(tiles[x * 4 + y]);
            }
            rows.Add(row);
            columns.Add(column);
        }
    }

    public Tile[] GetAvailableTiles()
    {
        List<Tile> availableTiles = new List<Tile>();
        foreach (Tile tile in tiles)
        {
            if (!tile.IsOccupied())
            {
                availableTiles.Add(tile);
            }
        }
        return availableTiles.ToArray();
    }

    public bool HasMergeOptions()
    {
        foreach (List<Tile> row in rows)
        {
            for (int i = 0; i < row.Count - 1; i++)
            {
                if (row[i].IsOccupied() && row[i + 1].IsOccupied() && row[i].GetCurrentBlock().CanMerge(row[i + 1].GetCurrentBlock()))
                {
                    return true;
                }
            }
        }

        foreach (List<Tile> column in columns)
        {
            for (int i = 0; i < column.Count - 1; i++)
            {
                if (column[i].IsOccupied() && column[i + 1].IsOccupied() && column[i].GetCurrentBlock().CanMerge(column[i + 1].GetCurrentBlock()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void CheckAllTilesAreSet()
    {
        foreach (Tile tile in tiles)
        {
            if (!tile.PropertiesAreSet())
            {
                Debug.LogError($"Tile at index {tile.GetIndex()} is not set.");
            }
        }
        Debug.Log("All tiles checked.");
    } 

    public Tile[] GetAllTiles()
    {
        return tiles;
    }

    public List<List<Tile>> GetRows()
    {
        return rows;
    }

    public List<List<Tile>> GetColumns()
    {
        return columns;
    }

    public void PrintTileStates()
    {
        foreach (Tile tile in tiles)
        {
            Debug.Log($"Tile Index: {tile.GetIndex()}, Occupied: {tile.IsOccupied()}, Block: {tile.GetCurrentBlock()}");
        }
    }
}
