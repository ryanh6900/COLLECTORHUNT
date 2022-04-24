using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //public InventorySystem inventorySystem;
    public InventoryItemData referenceItem;
    //public InventorySlot slot;
    public virtual void Awake()
    {
        gameObject.layer = 9;
    }
    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnHandlePickupItem();
    public abstract void OnLoseFocus();
}
