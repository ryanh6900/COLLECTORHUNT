using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Empty Object", menuName = "InventorySystem/Items/Empty")]
public class EmptyObject : ItemObject //this class might be completely removed in the future for peformance.
{
    public void Awake()
    {
        type = ItemType.Empty;
        this.isEquipmentObject = true;
    }
    public override void AssignItemToPlayer(PlayerItemController equipmentController)
    {
        equipmentController.EquipEmpty(this);
    }
    public override void UseItem(PlayerItemController itemUseController,FirstPersonPlayer player)
    {
        return;
    }
    public override void StopUsingItem(PlayerItemController itemUseController)
    {
        return;
    }
    public override void DepleteItem(PlayerItemController itemUseController, float deltaTime)
    {
        return;
    }
}
    

