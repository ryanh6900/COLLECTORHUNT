using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Medkit Object", menuName = "InventorySystem/Items/Medkit")]
public class MedkitObject : ItemObject
{
    public int healAmount;
    public void Awake()
    {
        type = ItemType.medkit;
        timeToUse = 3.0f;
        itemHealth = 60f;
        itemDrain = 20;
    }

    public override void AssignItemToPlayer(PlayerItemController equipmentController)
    {
        equipmentController.EquipMedkit(this); 
    }

    public override void UseItem(PlayerItemController itemUseController, FirstPersonPlayer player)
    {
        if(!itemDepleted) itemUseController.UseMedkit(this,player.healthOfPlayer);
    }
    public override void DepleteItem(PlayerItemController itemUseController, float deltaTime)
    {
        if (itemHealth > 0) itemHealth -= itemUseController.ItemDrain(this, deltaTime);
        else itemDepleted = true;
    }
    public void RestoreHealth(PlayerHealth playerHealth)
    {
        playerHealth.Heal(healAmount);
    }
    public override void StopUsingItem(PlayerItemController itemUseController)
    {
        //itemUseController.playerRigController.
        return;
    }
}
