using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item; 
    public int amount;
    public bool isFull;

    public InventorySlot()
    {
        item = null;
        ID = 0;
        amount = 0;
    }
    public InventorySlot(Item item, int ID,int amount)
    {
        this.item = item;
        this.ID = ID;
        this.amount = amount;
    }

    public void UpdateSlot(Item item, int ID, int amount)
    {
        if (amount > 2)
            isFull = true;
        this.item = item;
        this.ID = ID;
        this.amount = amount;  
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void RemoveAmount(int value)
    {
        amount -= value;
    }
}
