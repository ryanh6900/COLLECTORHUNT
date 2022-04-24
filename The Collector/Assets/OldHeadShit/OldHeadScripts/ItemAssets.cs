using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance; 

    private void Awake()
    {
        Instance = this;
    }
    public Transform prefabItemWorld;
    public Sprite batterySprite;
    public Sprite medkitSprite;
    public Sprite empty;
}
