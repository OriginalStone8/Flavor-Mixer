using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private List<IceCreamTypeSO> iceCreamTypes;
    private int prize;

    public Order()
    {
        iceCreamTypes = new List<IceCreamTypeSO>();
        GenerateRandomOrder();
    }

    private void GenerateRandomOrder()
    {
        int orderSize = UnityEngine.Random.Range(1, OrdersManager.Instance.GetMaxPerOrder() + 1);
        for (int i = 0; i < orderSize; i++)
        {
            IceCreamTypeSO iceCreamType = GameManagerScript.Instance.GetRandomIceCreamType();
            while (GetCountOf(iceCreamType) > iceCreamType.maxPerOrder - 1)
            {
                iceCreamType = GameManagerScript.Instance.GetRandomIceCreamType();
            }
            iceCreamTypes.Add(iceCreamType);
        }
    }

    private int GetCountOf(IceCreamTypeSO type)
    {
        int count = 0;
        for (int i = 0; i < iceCreamTypes.Count; i++)
        {
            if (iceCreamTypes[i] == type)
            {
                count++;
            }
        }
        return count;
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
    
    public int GetPrize()
    {
        return prize;
    }
}
