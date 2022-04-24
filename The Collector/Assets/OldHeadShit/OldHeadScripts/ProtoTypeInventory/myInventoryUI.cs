using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class myInventoryUI : MonoBehaviour
//{
//    [SerializeField] private Transform slotsParent;
//    [SerializeField] private myInventorySlot slotPrefab;
    

//    public void InitInventoryUI(myInventory inventory)
//    {
//        var itemsMap = inventory.GetAllItemsMap();
//        foreach (var kvp in itemsMap)
//        {
//            CreateOrUpdateSlot(inventory,kvp.Key, kvp.Value);
//        }
//    }
//    private Dictionary<myInventoryItem, myInventorySlot> itemToSlotMap = new Dictionary<myInventoryItem, myInventorySlot>();
//    public void CreateOrUpdateSlot(myInventory inventory, myInventoryItem item, int itemCount)
//    {
//        if (!itemToSlotMap.ContainsKey(item))
//        {
//            var slot = CreateSlot(inventory,item, itemCount);
//            itemToSlotMap.Add(item, slot);
//        }
//        else
//        {
//            UpdateSlot(item, itemCount);
//        }
//    }
//    public void UpdateSlot(myInventoryItem item, int itemCount)
//    {
//        itemToSlotMap[item].UpdateSlotCount(itemCount);
//    }

//    public myInventorySlot CreateSlot(myInventory inventory, myInventoryItem item, int itemCount)
//    {
//        var slot = Instantiate(slotPrefab, slotsParent);
//        slot.InitSlotVisualisation(item.GetSprite(), item.GetName(), itemCount);
//        slot.AssignSlotButtonCallback(() => inventory.AssignItem(item));
//        return slot;
//    }
//    public void DestroySlot(myInventoryItem item)
//    {
//        Destroy(itemToSlotMap[item].gameObject);
//        itemToSlotMap.Remove(item);
//    }
//}
