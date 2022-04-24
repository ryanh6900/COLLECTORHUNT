using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class InventoryDisplay : MonoBehaviour
{
    public MouseItem mouseItem;
    public InventoryObject inventorySystemObj;
    public GameObject inventorySlotPrefab;
    public Sprite backpackImage;
    //public Image uiBackground;
    public float xStart;
    public float yStart;
    public float xSpaceBetweenItems;
    public int numColumns;
    public float ySpaceBetweenItems;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Awake()
    {
        //add line that enables image first so there isnt empty image 
        CreateInventoryBackgroundSprite();
        CreateSlots();
    }
    void Update()
    {
        UpdateSlots();
    }
    void CreateInventoryBackgroundSprite()
    {
        Image image = GetComponent<Image>();
        image.enabled = true;
        image.sprite = backpackImage;
    }
    void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for(int i=0; i< inventorySystemObj.inventory.inventoryItemSlots.Length; i++)
        {
            var obj = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            itemsDisplayed.Add(obj, inventorySystemObj.inventory.inventoryItemSlots[i]);
        }
        //for (int i = 0; i < inventorySystemObj.inventory.inventoryItemSlots.Length; i++)
        //{
        //    InventorySlot slot = inventorySystemObj.inventory.inventoryItemSlots[i];

        //    var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        //    obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventorySystemObj.itemDatabase.GetItem[slot.item.ID].iconImage;
        //    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        //    obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //    itemsDisplayed.Add(slot, obj);
        //}
    }
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventorySystemObj.itemDatabase.GetItemObj[slot.Value.ID].iconImage;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(164, 52, 52, 255);
                slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount <= 0 || !slot.Value.item.isStackable? "" : slot.Value.amount.ToString("n0");
            }
          
            //else
            //{
            //    //itemsDisplayed.Remove(slot.Key);
            //    var sprite = Resources.Load<Sprite>("Sprites/BackgroundNavyGridSprite");
            //    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = sprite;
            //    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = inventorySystemObj.itemDatabase.GetItem[slot.Value.item.ID].prefab.GetComponent<Image>().color;
            //    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(164, 52, 52, 255);
            //    slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            //}

        }
        //for (int i = 0; i < inventorySystemObj.inventory.inventoryItemSlots.Length; i++)
        //{
        //    InventorySlot slot = inventorySystemObj.inventory.inventoryItemSlots[i];

        //    if (itemsDisplayed.ContainsKey(slot))
        //        itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //    else
        //    {
        //        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        //        //obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventorySystemObj.itemDatabase.GetItem[slot.item.ID].prefab.GetComponent<Image>().sprite;
        //        obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventorySystemObj.itemDatabase.GetItem[slot.item.ID].iconImage;
        //        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        //        if(slot.amount>0)obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //        itemsDisplayed.Add(slot, obj);
        //    }
        //}
    }
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            mouseItem.hoverItem = itemsDisplayed[obj];
    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        if(itemsDisplayed[obj].ID > 0)
        {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            //if statement was here checking if ID is greater than 0.
            mouseObject.transform.SetParent(transform.parent);
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventorySystemObj.itemDatabase.GetItemObj[itemsDisplayed[obj].ID].iconImage;
            img.raycastTarget = false;
            mouseItem.obj = mouseObject;
            mouseItem.item = itemsDisplayed[obj];
        }
    }
    public void OnDragEnd(GameObject obj)
    {   
        if(itemsDisplayed[obj].ID > 0)
        {
            if (mouseItem.hoverObj)
            {
                inventorySystemObj.SwapItemSlot(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
            }
            else
            {
                inventorySystemObj.RemoveItemStack(itemsDisplayed[obj].item);
            }
            mouseItem.item = null;
            Destroy(mouseItem.obj);
        }  
    }
    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    //public void OnClick(GameObject obj)
    //{
    //    if (itemsDisplayed.ContainsKey(obj))
    //        mouseItem.hoverItem = itemsDisplayed[obj];
    //}

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numColumns)), yStart + (-ySpaceBetweenItems * (i / numColumns)), 0f);

    }
}
