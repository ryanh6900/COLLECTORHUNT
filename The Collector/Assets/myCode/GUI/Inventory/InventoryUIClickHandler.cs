using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class InventoryUIClickHandler : MonoBehaviour, IPointerClickHandler
{
    private InventoryDisplay inventoryUI;
    //public UnityEvent equip;
    //public UnityEvent onMiddle;

    private void Start()
    {
        inventoryUI = GetComponentInParent<InventoryDisplay>(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(inventoryUI.mouseItem.hoverItem.item != null)
                inventoryUI.inventorySystemObj.EquipItem(inventoryUI.mouseItem.hoverItem.item,0);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //inventoryUI.mouseItem.hoverItem.item.SetHandID(1);
            //Debug.Log(inventoryUI.mouseItem.hoverItem.item.GetHandID());
            if (inventoryUI.mouseItem.hoverItem.item != null)
                inventoryUI.inventorySystemObj.EquipItem(inventoryUI.mouseItem.hoverItem.item,1);
            
        }
        //else if (eventData.button == PointerEventData.InputButton.Middle)
        //{
        //    onMiddle.Invoke();
        //}
    }
}
