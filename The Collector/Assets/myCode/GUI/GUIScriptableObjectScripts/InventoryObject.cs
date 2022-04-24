using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject //this class will need cleanup eventually. Alot of things can be handled better
{
    public string savePath;
    public ItemDatabase itemDatabase;
    //public Item currentItem;
    //public Item currentItem1;
    public Inventory inventory;
    private PlayerItemController playerItemController;
    [SerializeField] private AudioClip swapSound;
    [SerializeField] private AudioClip equipSound;
    public Vector3 hiddenUIScale = new Vector3(0,0,0);
    public Vector3 visibleUIScale = new Vector3(7, 7, 7);
    public AudioClip removeStackSound;
    public AudioClip displaySound;
    public AudioClip hideSound;
    public void ConnectToItemController(PlayerItemController playerItemEquipController) //this is how I connected another to class to a scriptable object the right way.
    {
       playerItemController = playerItemEquipController;
    }

    public void AddItem(Item item, int amount)
    {
        for(int i = 0; i< inventory.inventoryItemSlots.Length; i++)
        {
            if (!inventory.inventoryItemSlots[i].isFull && inventory.inventoryItemSlots[i].item.isStackable)
            {
                if (inventory.inventoryItemSlots[i].ID == item.ID)
                {
                    inventory.inventoryItemSlots[i].AddAmount(amount);
                    return;
                }
            }
        }
        SetEmptySlot(item,amount);
        
        //inventory.inventoryItemSlots.Add(new InventorySlot(_item,_item.ID, _amount));
    }
    public void SetEmptySlot(Item item, int amount) //used to return SetEmptySlot. Idk why.
    {
        for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
        {
            if (inventory.inventoryItemSlots[i].ID <= 0)
            {
                inventory.inventoryItemSlots[i].UpdateSlot(item, item.ID, amount);
                return;
            }
        }
        //still need to setup some form of indicator when inventory is full
        //return null;
    }
    public void RemoveNumItems(Item item, int amount)
    {
        for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
        {
            if (inventory.inventoryItemSlots[i].item == item)
            {
                if(inventory.inventoryItemSlots[i].amount > amount)
                {
                    inventory.inventoryItemSlots[i].RemoveAmount(amount);
                    inventory.inventoryItemSlots[i].UpdateSlot(item, item.ID, inventory.inventoryItemSlots[i].amount);
                }
                else
                inventory.inventoryItemSlots[i].UpdateSlot(null, 0, 0);
            }     
        }
    }
   
    public void SwapItemSlot(InventorySlot slot1, InventorySlot slot2)
    {
        InventorySlot temp = new InventorySlot(slot2.item, slot2.ID, slot2.amount);
        slot2.UpdateSlot(slot1.item, slot1.ID, slot1.amount);
        slot1.UpdateSlot(temp.item, temp.ID, temp.amount);
    }
    
    public void EquipItem(Item item,int whichHand)//, InventorySlot slot) //we use Item not ItemObject because we want it to be similar to other displayInventory functions and we only need it so we can find the item to search for in the itemDatabase.
    {
        ItemObject toEquipItemObject;
        
           
        for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
        {
            if (inventory.inventoryItemSlots[i].item == item)
            {
                //currentItem = item;
                //var selectedHandItem = SelectHandItem(item, item.GetHandID());
                playerItemController.StoreCurrentItem(whichHand);
                toEquipItemObject = itemDatabase.GetItemObj[inventory.inventoryItemSlots[i].ID];//item database is organized by ID while inventoryItemSlots is organized by whatever
                toEquipItemObject.SetHandID(whichHand);
                toEquipItemObject.AssignItemToPlayer(playerItemController);
                UpdateHandItem(item, whichHand);
                RemoveNumItems(item, 1);
            }
        }
    }

    public void EquipEmpty(int whichHand)
    {
        ItemObject emptyEquip = itemDatabase.GetItemObj[0];
        emptyEquip.SetHandID(whichHand);
        emptyEquip.AssignItemToPlayer(playerItemController);
        UpdateHandItem(new Item(emptyEquip), whichHand);
    }

    public void UpdateHandItem(Item item, int handID)
    {
        if (handID == 0)
        {
            inventory.leftHandItem = item;
            item.equipHandID = handID;
        }
        else if (handID == 1)
        {
            inventory.rightHandItem = item;
            item.equipHandID = handID;
        }   
    }
    public void RemoveItemStack(Item item)
    {
        //ItemObject trashItemObject = itemDatabase.GetItem[item.ID];
        for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
        {
           if(inventory.inventoryItemSlots[i].item == item)
           {
                // instantiate item at eye level and have it drop to ground
                //inventory.inventoryItemSlots[i].ID = itemDatabase.GetItem[0].ID;
                playerItemController.DropStack(itemDatabase.GetItemObj[item.ID], inventory.inventoryItemSlots[i].amount);
                inventory.inventoryItemSlots[i].UpdateSlot(null,0,0);
           }
        }
    }

    public void SaveInventory()
    {
        //Json method allows more customizing to the user when saving
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bF = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //Debug.Log("Saved to: "+ Application.persistentDataPath);
        //bF.Serialize(file, saveData);
        //file.Close();
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, inventory);
        stream.Close();
    }

    public void LoadInventory()
    {
        //if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        //{
        //    BinaryFormatter bF = new BinaryFormatter();
        //    FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
        //    JsonUtility.FromJsonOverwrite(bF.Deserialize(file).ToString(), this);
        //    file.Close();
        //}
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        inventory = (Inventory)formatter.Deserialize(stream);
        ////optional strategy if problems start:
        //for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
        //{
        //    inventory.inventoryItemSlots[i].UpdateSlot(newContainer.inventoryItemSlots[i].item, newContainer.inventoryItemSlots[i].ID, newContainer.inventoryItemSlots[i].amount);
        //}
        stream.Close();
        EquipItem(inventory.leftHandItem, 0);
        EquipItem(inventory.rightHandItem, 1);
        Debug.Log("Loaded from: " + Application.persistentDataPath);
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        inventory = new Inventory();
    }





    //public void EquipCurrentItem()//, InventorySlot slot) //we use Item not ItemObject because we want it to be similar to other displayInventory functions and we only need it so we can find the item to search for in the itemDatabase.
    //{
    //    //if(item == null) //null checker. Might use try-catch in future
    //    //{
    //    //    EquipEmpty();
    //    //    return;
    //    //}
    //    Item currItem = currentItem;
    //    for (int i = 0; i < inventory.inventoryItemSlots.Length; i++)
    //    { 
    //        if (inventory.inventoryItemSlots[i].item == currItem)
    //        {
    //            //currItem = item;
    //            toEquipItemObject = itemDatabase.GetItem[inventory.inventoryItemSlots[i].ID];//item database is organized by ID while inventoryItemSlots is organized by whatever
    //            toEquipItemObject.currHand = currentItem.GetHand();
    //            toEquipItemObject.AssignItemToPlayer(playerItemController);
    //        }
    //    }
    //}
}
