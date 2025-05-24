using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance { get; private set; }

    private int blocksInMotion = 0;

    private void Awake() => Instance = this;

    public void Move(Vector2 direction)
    {
        bool movedAny = false;

        List<List<Tile>> groups = (direction == Vector2.left || direction == Vector2.right)
            ? TileManager.Instance.GetRows()
            : TileManager.Instance.GetColumns();

        bool reverse = direction == Vector2.right || direction == Vector2.down;

        foreach (List<Tile> group in groups)
        {
            List<Block> blocks = ExtractBlocks(group, reverse);

            bool moved = MergeAndCompress(group, blocks, reverse);
            if (moved) movedAny = true;
        }

        if (!movedAny)
        {
            GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canSwipe);
        }
        else
        {
            StartCoroutine(WaitBeforeSpawnNewIceCream(0.3f));
        }
    }

    private List<Block> ExtractBlocks(List<Tile> tiles, bool reverse)
    {
        List<Block> blocks = new List<Block>();
        int count = tiles.Count;

        for (int i = 0; i < count; i++)
        {
            Tile tile = reverse ? tiles[count - 1 - i] : tiles[i];
            if (tile.IsOccupied())
            {
                blocks.Add(tile.GetCurrentBlock());
                tile.ClearTile();
            }
        }

        return blocks;
    }

    private bool MergeAndCompress(List<Tile> tiles, List<Block> blocks, bool reverse)
    {
        bool moved = false;
        int insertIndex = reverse ? tiles.Count - 1 : 0;
        int step = reverse ? -1 : 1;

        for (int i = 0; i < blocks.Count; i++)
        {
            Block current = blocks[i];

            // Merge if possible
            if (i + 1 < blocks.Count &&
                blocks[i].GetIceCreamType().index == blocks[i + 1].GetIceCreamType().index)
            {
                blocks[i].MergeWith();
                Destroy(blocks[i + 1].gameObject);
                blocks.RemoveAt(i + 1); // Remove merged block
                moved = true;
            }

            // Move to tile
            Tile targetTile = tiles[insertIndex];
            if (current.transform.position != targetTile.transform.position)
            {
                moved = true;
            } 
            else
            {
                current.transform.position = targetTile.transform.position;
                targetTile.SetCurrentBlock(current);
                targetTile.SetIsOccupied(true);
                current.setCurrentTile(targetTile);
            }

            blocksInMotion++;

            LeanTween.move(current.gameObject, targetTile.transform.position, 0.3f)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                current.transform.position = targetTile.transform.position;
                targetTile.SetCurrentBlock(current);
                targetTile.SetIsOccupied(true);
                current.setCurrentTile(targetTile);

                blocksInMotion--;

                if (blocksInMotion == 0)
                {
                    //TileManager.Instance.PrintTileStates();
                    if (!GameManagerScript.Instance.isGameOver())
                        GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canSwipe);
                }
            });


            insertIndex += step;
        }

        return moved;
    }

    private IEnumerator WaitBeforeSpawnNewIceCream(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManagerScript.Instance.SpawnNewIceCream();
    }
}

