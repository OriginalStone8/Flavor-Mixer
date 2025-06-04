using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceCreamType", menuName = "ScriptableObjects/IceCreamTypeSO")]
public class IceCreamTypeSO : ScriptableObject
{
    public int index;
    public int scorePerMerge;
    public Sprite icon;
    public Sprite sprite;
    public int maxPerOrder;
    public int rewardCoins;
    public int rewardGems;
}
