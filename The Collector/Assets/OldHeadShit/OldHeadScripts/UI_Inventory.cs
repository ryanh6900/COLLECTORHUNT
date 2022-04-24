//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//public class UI_Inventory : MonoBehaviour
//{
//    public Inventory inventory;
//    public Transform itemSlotContainer;
//    public Transform itemSlotTemplate;

//    public void Awake()
//    {
//        itemSlotContainer = transform.Find("itemSlotContainer");
//        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate"); 
//    }

//    public void SetInventory(Inventory inventory)
//    {
//        this.inventory = inventory;
//            RefreshInventoryItems();
//    }
    
//    public void RefreshInventoryItems()
//    {
//        int x = 0;
//        int y = 0;
//        float itemSlotCellSize = 100;
//         foreach(Item item in inventory.GetItemList())
//        {
//            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate,itemSlotContainer).GetComponent<RectTransform>();
//            itemSlotRectTransform.gameObject.SetActive(true);
//            itemSlotRectTransform.anchoredPosition = new Vector3(x * itemSlotCellSize, (y * itemSlotCellSize),0);
//            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
//            image.sprite = item.GetSprite();
//            x++;
//            if (x > 2)
//            {
//                x = 0;
//                y++;
//            }
            
//        }
//    }
//}
