using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Battery Object", menuName = "InventorySystem/Items/Battery")]
public class BatteryObject : ItemObject
{
    public int restorePower;
    public void Awake()
    {
        type =ItemType.battery;
    }
    public override void AssignItemToPlayer(PlayerItemController equipController)
    {
        equipController.EquipBattery(this);
    }
    public override void UseItem(PlayerItemController itemUseController,FirstPersonPlayer player)
    {
        if(!itemDepleted)itemUseController.UseBattery(this,player.playerFlashlight);
    }
    public override void StopUsingItem(PlayerItemController itemUseController)
    {
        throw new System.NotImplementedException();
    }
    public override void DepleteItem(PlayerItemController itemUseController, float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
