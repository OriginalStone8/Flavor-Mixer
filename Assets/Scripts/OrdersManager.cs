using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public static OrdersManager Instance { get; private set; }
    [SerializeField] private List<OrderDisplay> orderDisplays;

    private void Awake() 
    {
        Instance = this;
    }

    public Order GenerateOrder()
    {
        return new Order();
    }
}
