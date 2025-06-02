using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; } 

    public event EventHandler OnGameOver;

    [SerializeField] private List<IceCreamTypeSO> iceCreamTypes;
    [SerializeField] private GameObject blockPrefab;

    public enum GameState
    {
        Prestart,
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
        gameState = GameState.Prestart;
        SwipeManager.Instance.OnSwipe += HandleSwipe;
        SceneManagement.Instance.OnGameStart += InitializeGame;
    }

    private void HandleSwipe(object sender, SwipeManager.OnSwipeEventArgs e)
    {
        if (e.Direction == Vector2.left || e.Direction == Vector2.right || e.Direction == Vector2.up || e.Direction == Vector2.down)
        {
            SetGameState(GameState.canNotSwipe);
        }
        MovementManager.Instance.Move(e.Direction);
    }

    private void InitializeGame(object sender, EventArgs e)
    {
        gameState = GameState.Start;
        SpawnNewIceCream();
        SpawnNewIceCream();
        gameState = GameState.canSwipe;
        OrdersManager.Instance.UpdateAllDisplayButtons();
    }

    private void SpawnIceCream(IceCreamTypeSO iceCreamType)
    {
        TileManager.Instance.CheckAllTilesAreSet();
        Tile[] availableTiles = TileManager.Instance.GetAvailableTiles();
        if (availableTiles.Length > 0)
        {
            Tile selectedTile = RandomizeTile(availableTiles);
            Block newBlock = Instantiate(blockPrefab, selectedTile.transform.position, Quaternion.identity).GetComponent<Block>();
            newBlock.SetIceCreamType(iceCreamType);
            newBlock.setCurrentTile(selectedTile);
            selectedTile.SetIsOccupied(true);
            selectedTile.SetCurrentBlock(newBlock);
            newBlock.ScaleInAnimation();
        }
        OrdersManager.Instance.UpdateAllDisplayButtons();
        CheckForGameOver();
    }

    private void DeleteExtraBlocks()
    {
        List<Block> extraBlocks = new List<Block>(FindObjectsOfType<Block>());
        foreach (Block block in extraBlocks)
        {
            block.IsExtraBlock();
        }
    }

    private void CheckForGameOver()
    {
        Tile[] availableTiles = TileManager.Instance.GetAvailableTiles();
        if (availableTiles.Length == 0 && !TileManager.Instance.HasMergeOptions() && !OrdersManager.Instance.AnyCompletedOrders())
        {
            GameOver();
        }
    }

    public void SpawnNewIceCream()
    {
        IceCreamTypeSO iceCreamTypeSO = RandomizeIceCreamType();
        SpawnIceCream(iceCreamTypeSO);
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
        OnGameOver?.Invoke(this, EventArgs.Empty);
        gameState = GameState.GameOver;
        Debug.Log("Game Over!");
        // Handle game over logic here
    }

    public bool canSwipe()
    {
        return gameState == GameState.canSwipe;
    } 

    public bool isGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public void SetGameState(GameState state)
    {
        gameState = state;  
    }

    public bool InGameMode()
    {
        return gameState == GameState.canSwipe || gameState == GameState.canNotSwipe;
    }

    public int GetIceCreamTypeCount()
    {
        return iceCreamTypes.Count;
    }

    public IceCreamTypeSO GetRandomIceCreamType()
    {
        return iceCreamTypes[UnityEngine.Random.Range(0, iceCreamTypes.Count)];
    }

    public List<IceCreamTypeSO> GetIcecreamTypeList()
    {
        return iceCreamTypes;
    }
}
