using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour
{
    public static ContinueGame Instance { get; private set; }
    [SerializeField] private int destroyAmount;
    [SerializeField] private int coinStartPrice, gemStartPrice;
    [SerializeField] private Button continueButton;
    private int coinCurrentPrice, gemCurrentPrice;
    private bool isCoins = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coinCurrentPrice = coinStartPrice;
        gemCurrentPrice = gemStartPrice;
    }

    public void DestroyForContinue()
    {
        StartCoroutine(DestroyMinimumValue(destroyAmount));
    }

    private IEnumerator DestroyMinimumValue(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            List<Block> blocks = new List<Block>(FindObjectsOfType<Block>());

            Block minBlock = GetMinimumValue(blocks);
            if (minBlock != null)
            {
                minBlock.ScaleOutAnimationAndDestroy(0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        StartCoroutine(SceneManagement.Instance.DelayedContinue(0.2f));
    }

    private Block GetMinimumValue(List<Block> blocks)
    {
        if (blocks.Count == 0) return null;

        int min = blocks[0].GetIceCreamType().scorePerMerge;
        Block minBlock = blocks[0];
        foreach (Block block in blocks)
        {
            if (block.GetIceCreamType().scorePerMerge < min)
            {
                min = block.GetIceCreamType().scorePerMerge;
                minBlock = block;
            }
        }
        return minBlock;
    }

    public void UpdateContinueButtonInteractability()
    {
        continueButton.GetComponent<ContinueButton>().UpdateButtonInteractability();
    }

    public int GetCoinCurrentPrice()
    {
        return coinCurrentPrice;
    }

    public int GetGemCurrentPrice()
    {
        return gemCurrentPrice;
    }

    public bool IsCoins()
    {
        return isCoins;
    }

    public void ToggleIsCoins()
    {
        isCoins = !isCoins;
    }

    public void IncreaseCoinPrice()
    {
        coinCurrentPrice *= 2;
    }

    public void IncreaseGemPrice()
    {
        gemCurrentPrice *= 2;
    }
}
