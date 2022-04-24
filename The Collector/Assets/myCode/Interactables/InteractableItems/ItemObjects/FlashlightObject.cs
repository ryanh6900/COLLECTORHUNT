using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Flashlight Object", menuName = "InventorySystem/Items/Flashlight")]
public class FlashlightObject : ItemObject
{
    public GameObject flashlightLightObj;
    public Light flashlightLight;
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;
    void Awake()
    {
        type = ItemType.flashlight;
        isEquipmentObject = true;
        flashlightLight.enabled = false;
        timeToUse = 0.25f;
        itemHealth = 100f;
        itemDrain = 0.1f;
        itemDepleted = false;
        //flashlightLight = this.GetPrefab().GetComponentInChildren<Light>();
    }
    public override void AssignItemToPlayer(PlayerItemController equipmentController)
    {
        equipmentController.EquipFlashlight(this);
    }
    public override void UseItem(PlayerItemController itemUseController,FirstPersonPlayer player)
    {
        if(!itemDepleted)itemUseController.TurnOnFlashlight(this);
    }
    public override void StopUsingItem(PlayerItemController itemUseController)
    {
        itemUseController.TurnOffFlashlight(this); 
    }
    public override void DepleteItem(PlayerItemController itemUseController, float deltaTime)
    {
        itemHealth -= itemUseController.ItemDrain(this, deltaTime);
    }

    public void ToggleFlashlight(bool turnOn)
    {
        //if (turnOn) flashlightLightObj.SetActive(true);
        //else flashlightLightObj.SetActive(false);
        if (turnOn) flashlightLight.enabled = true;
        else flashlightLight.enabled = false;
    }
    public void ChangeBattery(float addedPower)
    {
        itemHealth += addedPower;
        itemDepleted = false;
    }
    public GameObject GetFlashlightLight()
    {
        return flashlightLightObj;
    }

}
