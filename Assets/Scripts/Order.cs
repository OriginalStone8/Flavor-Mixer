using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private List<IceCreamTypeSO> iceCreamTypes;
    private List<int> amounts;
    private int prize;

    public Order()
    {
        iceCreamTypes = new List<IceCreamTypeSO>();
        amounts = new List<int>();
        GenerateRandomOrder();
    }

    private void GenerateRandomOrder()
    {
        int orderSize = UnityEngine.Random.Range(1, OrdersManager.Instance.GetMaxPerOrder() + 1);
        for (int i = 0; i < orderSize; i++)
        {
            iceCreamTypes.Add(GameManagerScript.Instance.GetRandomIceCreamType());
            /*if (iceCreamTypes[i].index == GameManagerScript.Instance.GetIceCreamTypeCount() - 1)
            {
                amounts.Add(1);
            }
            else if (iceCreamTypes[i].index >= GameManagerScript.Instance.GetIceCreamTypeCount() - 3)
            {
                amounts.Add(UnityEngine.Random.Range(1, 3));
            }
            else
            {
                amounts.Add(UnityEngine.Random.Range(1, 4));
            }*/
        }
    }

    public bool IsCompleted()
    {
        List<Block> blocks = new List<Block>(UnityEngine.GameObject.FindObjectsOfType<Block>());
        List<IceCreamTypeSO> foundIceCreams = new List<IceCreamTypeSO>();
        foreach (Block block in blocks)
        {
            foundIceCreams.Add(block.GetIceCreamType());
        }

        foreach (IceCreamTypeSO iceCreamType in iceCreamTypes)
        {
            if (!foundIceCreams.Contains(iceCreamType))
            {
                return false;
            }
            else
            {
                int index = foundIceCreams.IndexOf(iceCreamType);
                foundIceCreams.RemoveAt(index);
            }
        }
        return true;
    }

    public List<IceCreamTypeSO> GetFoundTypes()
    {
        List<Block> blocks = new List<Block>(UnityEngine.GameObject.FindObjectsOfType<Block>());
        List<IceCreamTypeSO> foundIceCreams = new List<IceCreamTypeSO>();
        foreach (Block block in blocks)
        {
            if (!foundIceCreams.Contains(block.GetIceCreamType()))
                foundIceCreams.Add(block.GetIceCreamType());
        }

        return foundIceCreams;
    }

    public void CalculatePrize()
    {
        prize = 0;
        List<IceCreamTypeSO> types = GameManagerScript.Instance.GetIcecreamTypeList();
        foreach (IceCreamTypeSO iceCreamType in iceCreamTypes)
        {
            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].index < iceCreamType.index)
                {
                    prize += types[i].scorePerMerge;
                }
            }
            prize += iceCreamType.scorePerMerge;
        }
        prize += iceCreamTypes.Count * 50;
    }

    public List<IceCreamTypeSO> GetIceCreams()
    {
        return iceCreamTypes;
    }

    public List<int> GetAmounts()
    {
        return amounts;
    }

    public int GetPrize()
    {
        return prize;
    }
}
