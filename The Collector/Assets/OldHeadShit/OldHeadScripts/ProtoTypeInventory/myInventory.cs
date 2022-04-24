//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//[CreateAssetMenu(menuName = "myInventory")]
////public class myInventory : ScriptableObject
////{
////    [SerializeField] private List<InventoryItemWrapper> items = new List<InventoryItemWrapper>();
////    [SerializeField] private myInventoryUI inventoryUIPrefab;
////    private myInventoryUI _inventoryUI;
////    private myInventoryUI inventoryUI
////    {
////        get
////        {
////            if(!_inventoryUI)
////                _inventoryUI = Instantiate(inventoryUIPrefab,playerEquipment.GetUIParent());
////            return _inventoryUI;
////        }

////    }
////    private Dictionary<myInventoryItem,int> itemToCountMap = new Dictionary<myInventoryItem,int>();
////    private PlayerEquipmentController playerEquipment;
////    public void InitInventory(PlayerEquipmentController playerEquipment)
////    {
////        this.playerEquipment = playerEquipment;
////        for(int i=0; i< items.Count; i++)
////        {
////            itemToCountMap.Add(items[i].GetItem(),items[i].GetItemCount());
////        }
////    }
////    public void OpenInventoryUI()
////    {
////        inventoryUI.gameObject.SetActive(true);
////        inventoryUI.InitInventoryUI(this);
////    }
////    public void AssignItem(myInventoryItem item)
////    {
        
////    }
////    public Dictionary<myInventoryItem,int> GetAllItemsMap()
////    {
////        return itemToCountMap;
////    }
////    public void AddItem(myInventoryItem item,int count)
////    {
////        int currentItemCount;
////        if (itemToCountMap.TryGetValue(item, out currentItemCount)) itemToCountMap[item] = currentItemCount + count;
////        else itemToCountMap.Add(item, count);

////        inventoryUI.CreateOrUpdateSlot(this,item, count);
////    }

////    public void RemoveItem(myInventoryItem item, int count)
////    {
////        int currentItemCount;
////        if(itemToCountMap.TryGetValue((item), out currentItemCount))
////        {
////            itemToCountMap[item]= currentItemCount - count;
////            if (currentItemCount - count <= 0) inventoryUI.DestroySlot(item);
////            else inventoryUI.UpdateSlot(item, currentItemCount - count);
////        }
////    }
////}
