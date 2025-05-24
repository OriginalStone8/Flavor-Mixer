using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance { get; private set; } 

    public event EventHandler<OnMoveFinishedEventArgs> OnInitialMoveFinished;
    public class OnMoveFinishedEventArgs : EventArgs {
        public Vector2 dir;
    }

    struct MergePair 
    {
        public Block target;
        public Block source;
    }
    List<MergePair> mergeQueue = new List<MergePair>();

    [SerializeField] private float moveSpeed;

    private int blocksInMotion = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        OnInitialMoveFinished += InitialMoveFinished;
    }

    private void Update() {
        //Debug.Log("Blocks in motion: " + blocksInMotion);
    }

    private void InitialMoveFinished(object sender, OnMoveFinishedEventArgs e)
    {
        List<List<Tile>> groups = (e.dir == Vector2.left || e.dir == Vector2.right)
            ? TileManager.Instance.GetRows()
            : TileManager.Instance.GetColumns();

        bool reverse = e.dir == Vector2.right || e.dir == Vector2.down;
        int blocksMoved = 0;

        foreach (var group in groups)
        {
            List<Block> blocks = ExtractBlocks(group, reverse);
            blocks = MergeBlocks(blocks);

            blocksMoved += ApplySecondMovementToTiles(group, blocks, reverse);
        }

        if (blocksMoved > 0)
        {
            Debug.Log("No second movement needed — unlocking swipe");
            GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canSwipe);
        }
    }


    public void MoveLeft()
    {
        foreach (var row in TileManager.Instance.GetRows())
        {
            List<Block> blocks = ExtractBlocks(row, false);

            PrepareMergeBlocks(blocks);

            ApplyMovementToTiles(row, blocks, false, Vector2.left);
        }
    }

    public void MoveRight()
    {
        foreach (var row in TileManager.Instance.GetRows())
        {
            List<Block> blocks = ExtractBlocks(row, true);

            PrepareMergeBlocks(blocks);

            ApplyMovementToTiles(row, blocks, true, Vector2.right);
        }
    }

    public void MoveUp()
    {
        foreach (var column in TileManager.Instance.GetColumns())
        {
            List<Block> blocks = ExtractBlocks(column, false);

            PrepareMergeBlocks(blocks);

            ApplyMovementToTiles(column, blocks, false, Vector2.up);
        }
    }

    public void MoveDown()
    {
        foreach (var column in TileManager.Instance.GetColumns())
        {
            List<Block> blocks = ExtractBlocks(column, true);
            
            PrepareMergeBlocks(blocks);

            ApplyMovementToTiles(column, blocks, true, Vector2.down);
        }
    }

    private void PrepareMergeBlocks(List<Block> blocks)
    {
        int i = 0;
        while (i < blocks.Count - 1) 
        {
            if (blocks[i].GetIceCreamType().index == blocks[i + 1].GetIceCreamType().index) 
            {
                mergeQueue.Add(new MergePair { target = blocks[i], source = blocks[i + 1] });
                i += 2;
            } 
            else
                i++;
        }
    }

    private List<Block> MergeBlocks(List<Block> blocks)
    {
        foreach (MergePair pair in mergeQueue)
        {
            pair.target.MergeWith();
            blocks.Remove(pair.source);
            Destroy(pair.source.gameObject);
        }
        mergeQueue.Clear(); 
        return blocks;
    }

    private void MoveBlockToTile(Block block, Tile tile, Vector2 direction)
    {
        if (block.transform.position == tile.transform.position)
        {
            block.transform.position = tile.transform.position;
            tile.SetCurrentBlock(block);
            tile.SetIsOccupied(true);
            block.setCurrentTile(tile);
            return;
        }
        blocksInMotion++;
        //GameManagerScript.Instance.IncrementBlocksMoved(); 
        //GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canNotSwipe);

        LeanTween.move(block.gameObject, tile.transform.position, moveSpeed)
        .setEaseInOutSine()
        .setOnComplete(() => {
            block.transform.position = tile.transform.position;
            tile.SetCurrentBlock(block);
            tile.SetIsOccupied(true);
            block.setCurrentTile(tile);

            blocksInMotion--;

            if (blocksInMotion == 0)
            {
                OnInitialMoveFinished?.Invoke(this, new OnMoveFinishedEventArgs { dir = direction });
            }
        });
    }

    private List<Block> ExtractBlocks(List<Tile> tiles, bool reverse)
    {
        List<Block> blocks = new List<Block>();
        if (!reverse)
        {
            foreach (var tile in tiles)
            {
                if (tile.IsOccupied())
                {
                    blocks.Add(tile.GetCurrentBlock());
                    tile.ClearTile();
                }
            }
            return blocks;
        }
        else
        {
            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                if (tiles[i].IsOccupied())
                {
                    blocks.Add(tiles[i].GetCurrentBlock());
                    tiles[i].ClearTile();
                }
            }
            return blocks;
        }
    }
        
    private void ApplyMovementToTiles(List<Tile> tiles, List<Block> blocks, bool reverse, Vector2 dir)
    {
        int insertIndex = reverse ? tiles.Count - 1 : 0;
        foreach (var block in blocks)
        {
            Tile targetTile = tiles[insertIndex];
            MoveBlockToTile(block, targetTile, dir);
            insertIndex = reverse ? insertIndex - 1 : insertIndex + 1;
        }
    }

    private int ApplySecondMovementToTiles(List<Tile> tiles, List<Block> blocks, bool reverse)
    {
        int count = 0;
        int insertIndex = reverse ? tiles.Count - 1 : 0;
        foreach (var block in blocks)
        {
            Tile targetTile = tiles[insertIndex];
            count += SecondMoveBlockToTile(block, targetTile);
            insertIndex = reverse ? insertIndex - 1 : insertIndex + 1;
        }
        return count;
    }

    private int SecondMoveBlockToTile(Block block, Tile tile)
    {
        blocksInMotion++;
        if (block.transform.position == tile.transform.position)
        {
            block.transform.position = tile.transform.position;
            tile.SetCurrentBlock(block);
            tile.SetIsOccupied(true);
            block.setCurrentTile(tile);

            blocksInMotion--;

            return 0;
        }
        LeanTween.move(block.gameObject, tile.transform.position, moveSpeed)
        .setEaseInOutSine()
        .setOnComplete(() => {
            block.transform.position = tile.transform.position;
            tile.SetCurrentBlock(block);
            tile.SetIsOccupied(true);
            block.setCurrentTile(tile);

            blocksInMotion--;
            if (blocksInMotion == 0)
            {
                Debug.Log("Blocks in motion finished");
                GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canSwipe);
            }
        });
        return 1;
    }

    private bool WillAnyBlockMove(List<Block> blocks, List<Tile> tiles, bool reverse)
    {
        int insertIndex = reverse ? tiles.Count - 1 : 0;

        foreach (var block in blocks)
        {
            Tile targetTile = tiles[insertIndex];
            if (block != null && block.transform.position != targetTile.transform.position)
                return true;

            insertIndex = reverse ? insertIndex - 1 : insertIndex + 1;
        }

        return false;
    }

}
