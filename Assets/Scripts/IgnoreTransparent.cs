using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IgnoreTransparent : MonoBehaviour
{
    [SerializeField] private List<Image> images;

    private void Start()
    {
        foreach (Image image in images)
        {
          image.alphaHitTestMinimumThreshold = 0.5f;  
        }
    }   
}
