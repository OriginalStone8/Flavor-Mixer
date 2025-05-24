using System.Collections;
using System.Collections.Generic;
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
}
