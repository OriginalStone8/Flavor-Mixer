using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; } 
    public List<IceCreamTypeSO> iceCreamTypes;

    [SerializeField] private GameObject blockPrefab;
    private int blocksMoved;

    public enum GameState
    {
        Start,
        canSwipe,
        canNotSwipe,
        GameOver
    }

    private GameState gameState;

    private void Awake() 
    {
        Instance = this;
    }

    private void Update() {
        //Debug.Log("Game State: " + gameState);
    }

    private void Start() 
    {
        SwipeManager.Instance.OnSwipe += HandleSwipe;
        InitializeGame();
    }

    private void HandleSwipe(object sender, SwipeManager.OnSwipeEventArgs e)
    {
        ResetBlocksMoved();

        if (e.Direction == Vector2.left || e.Direction == Vector2.right || e.Direction == Vector2.up || e.Direction == Vector2.down)
        {
            SetGameState(GameState.canNotSwipe);
        }
        
        if (e.Direction == Vector2.left) MovementManager.Instance.MoveLeft();
        else if (e.Direction == Vector2.right) MovementManager.Instance.MoveRight();
        else if (e.Direction == Vector2.up) MovementManager.Instance.MoveUp();
        else if (e.Direction == Vector2.down) MovementManager.Instance.MoveDown();
    }

    private void InitializeGame()
    {
        gameState = GameState.Start;
        IceCreamTypeSO iceCreamTypeSO = RandomizeIceCreamType();
        SpawnIceCream(iceCreamTypeSO);
        gameState = GameState.canSwipe;
    }

    private void SpawnIceCream(IceCreamTypeSO iceCreamType)
    {
        Tile[] availableTiles = TileManager.Instance.GetAvailableTiles();
        if (availableTiles.Length > 0)
        {
            Tile selectedTile = RandomizeTile(availableTiles);
            Block newBlock = Instantiate(blockPrefab, selectedTile.transform.position, Quaternion.identity).GetComponent<Block>();
            newBlock.SetIceCreamType(iceCreamType);
            newBlock.setCurrentTile(selectedTile);
            selectedTile.SetIsOccupied(true);
            selectedTile.SetCurrentBlock(newBlock);
        }
        else
        {
            GameOver();
        }
    }

    private IceCreamTypeSO RandomizeIceCreamType()
    {
        return iceCreamTypes[UnityEngine.Random.Range(0, 2)];
    }

    private Tile RandomizeTile(Tile[] availableTiles)
    {
        return availableTiles[UnityEngine.Random.Range(0, availableTiles.Length)];
    }

    public IceCreamTypeSO GetNextIceCreamType(IceCreamTypeSO iceCreamTypeSO)
    {
        return iceCreamTypes[iceCreamTypeSO.index + 1];
    }

    private void GameOver()
    {
        gameState = GameState.GameOver;
        Debug.Log("Game Over!");
        // Handle game over logic here
    }

    public bool canSwipe()
    {
        return gameState == GameState.canSwipe;
    } 

    public void SetGameState(GameState state)
    {
        Debug.Log("Game State changed from: " + gameState + " to: " + state);
        gameState = state;
        if (state == GameState.canSwipe && blocksMoved > 0)
        {
            IceCreamTypeSO iceCreamTypeSO = RandomizeIceCreamType();
            SpawnIceCream(iceCreamTypeSO);
        }   
    }

    public void IncrementBlocksMoved()
    {
        blocksMoved++;
    }

    public void ResetBlocksMoved()
    {
        blocksMoved = 0;
    }
}
