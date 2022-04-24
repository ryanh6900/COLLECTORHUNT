using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryItemWrapper
{
    [SerializeField] private myInventoryItem item;
    [SerializeField] private int count;

    public myInventoryItem GetItem()
    {
        return item;
    }
    public int GetItemCount()
    {
        return count;
    }
}
