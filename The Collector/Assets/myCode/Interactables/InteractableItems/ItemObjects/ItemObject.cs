using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType //Actually not being used. //requires items to know everything. You can write your code in a way where any type of subclass can be used in 
{
    medkit,
    battery,
    binoculars,
    flashlight,
    Empty
}
public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite iconImage;
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public bool isEquipmentObject;
    public float timeToUse;
    public float itemHealth;
    public float itemDrain;
    public bool itemDepleted;
    public int handID;
    public Vector3 itemLocalPos;
    public Vector3 itemLocalRot;
    public Vector3 itemLocalScale;
    //public Rigidbody rb;
    public AudioClip itemUseSound;
    public GameObject partnerObject; //not being used right now
    public abstract void AssignItemToPlayer(PlayerItemController equipmentController);
    public abstract void UseItem(PlayerItemController itemUseController,FirstPersonPlayer player);
    public abstract void StopUsingItem(PlayerItemController itemUseController); //not entirely sure yet if I need to make this function.
    public abstract void DepleteItem(PlayerItemController itemUseController, float deltaTime);
    public int GetID()
    {
        return ID;
    }
    public Sprite GetSprite()
    {
        return iconImage;
    }
    public GameObject GetPrefab()
    {
        return prefab;
    }
    public ItemType GetItemType() //item type not being used really. 
    {
        return type;
    }
    public Vector3 GetLocalPosition()
    {
        return itemLocalPos;
    }
    public Quaternion GetLocalRotatition()
    {
        return Quaternion.Euler(itemLocalRot);
    }
    public Vector3 GetLocalScale()
    {
        return itemLocalScale;
    }
    public void SetHandID(int ID)
    {
        handID = ID;
    }
    public Item CreateItem() //not being used right now
    {
        Item newItem = new Item(this);
        return newItem;
    } 
}
