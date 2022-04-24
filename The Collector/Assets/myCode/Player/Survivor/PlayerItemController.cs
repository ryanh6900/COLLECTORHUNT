using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public InventoryObject inventoryObj;
    public PlayerRigController playerRigController;
    //private Animator playerAnimator;
    private AudioSource playerAudioSource;
    [SerializeField] private Transform inventoryUIParent;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform rightHandFlashlightTransform;
    [SerializeField] private Transform leftHandFlashlightTransform;
    [SerializeField] private Transform dropTransform;
    public GameObject playerObject;
    public GameObject currentLeftHandObject;
    public GameObject currentRightHandObject;
    public PlayerHealth playerHealth;
    public PlayerStamina playerStamina;
    public PlayerSpeed playerSpeed;
    //[SerializeField] private EmptyObject empty;
    //[SerializeField] private GameObject emptyGameObject;

    void Start()
    {
        inventoryObj.ConnectToItemController(this);
        //playerAnimator = GetComponentInChildren<Animator>();
        playerRigController = GetComponentInChildren<PlayerRigController>();
        playerAudioSource = GetComponent<AudioSource>();
    }
    public void GetSurvivorPlayerVitals(FirstPersonPlayer player) //not sure how to connect player vitals to this or rig controller
    {
        playerHealth = player.healthOfPlayer;
        playerStamina = player.staminaOfPlayer;
        playerSpeed = player.speedOfPlayer;
    }
    public void EquipEmpty(EmptyObject emptyObject)
    {
        //StoreItem(nullObject.handID);
        switch (emptyObject.handID)
        {
            case 0:
                ClearCurrentHandObj(currentLeftHandObject);
                currentLeftHandObject = CreateEmptyInstanceInHand(emptyObject,leftHandTransform);
                playerRigController.AnimatorOnEquippedItem(0, 0, true);
                break;
            case 1:
                ClearCurrentHandObj(currentRightHandObject);
                currentRightHandObject = CreateEmptyInstanceInHand(emptyObject,rightHandTransform);
                playerRigController.AnimatorOnEquippedItem(1, 0, true);
                break;
            default:
                break;
        }
        //ClearCurrentHandObj(currentLeftHandObject);
    }
    public void EquipBattery(BatteryObject battery)
    {
        switch (battery.handID)
        {
            case 0:
                ClearCurrentHandObj(currentLeftHandObject);
                currentLeftHandObject = CreateBatteryInstanceInHand(battery, leftHandTransform);
                break;
            case 1:
                ClearCurrentHandObj(currentRightHandObject);
                currentRightHandObject = CreateBatteryInstanceInHand(battery, rightHandTransform);
                break;
            default:
                break;
        }
    }

    public void EquipMedkit(MedkitObject medkit)
    {
        //ClearCurrentHandObj(currentLeftHandObject);
        //currentLeftHandObject = CreateItemInstanceInHand(medkit, leftHandTransform);
        //StoreItem(medkit.handID);
        switch (medkit.handID)
        {
            case 0:
                ClearCurrentHandObj(currentLeftHandObject);
                currentLeftHandObject = CreateMedkitInstanceInHand(medkit, leftHandTransform);
                break;
            case 1:
                ClearCurrentHandObj(currentRightHandObject);
                currentRightHandObject = CreateMedkitInstanceInHand(medkit, rightHandTransform);
                break;
            default:
                break;
        }
    }
    
    public void EquipFlashlight(FlashlightObject flashlight)
    {
        //playerAnimator.SetTrigger("EquipItem");
        //playerAnimator.SetFloat("ItemHold", 0.5f);
        //StoreItem(flashlight.handID);
        switch (flashlight.handID)
        {
            case 0:
                ClearCurrentHandObj(currentLeftHandObject);
                playerRigController.AnimatorOnEquippedItem(0, 1,false);
                Debug.Log("Success");
                currentLeftHandObject = CreateFlashlightInstanceInHand(flashlight, leftHandFlashlightTransform);
                break;
            case 1:
                ClearCurrentHandObj(currentRightHandObject);
                //playerAnimator.SetFloat("ItemEquippedRH", 1.0f);
                playerRigController.AnimatorOnEquippedItem(1, 1,false);
                Debug.Log("Success");
                currentRightHandObject = CreateFlashlightInstanceInHand(flashlight, rightHandFlashlightTransform);
                break;
            default:
                break;
        }
    }

    //public void EquipItem(ItemObject itemObj) //not being used. Possible implementation if I decide to use both hands of player instead of just left.
    //{
    //    switch (itemObj.currentHand)
    //    {
    //        case Hand.Left:
    //            ClearCurrentHandObj(currentLeftHandObject);
    //            break;
    //        case Hand.Right:
    //            ClearCurrentHandObj(currentRightHandObject);
    //            break;
    //        default:
    //            break;
    //    }
    //}
   
    public void UseBattery(BatteryObject batteryObject,FlashlightObject flashlightObject)
    {
        flashlightObject.ChangeBattery(batteryObject.restorePower);
        //inventoryObj.EquipEmpty(batteryObject.handID); //implement method where you mark item as used then drop it from hand.
        //ClearCurrentHandObj(currentLeftHandObject);
        //playerObject.GetComponent<Flashlight>().ChangeBattery(batteryObject.restorePower);
    }
    public void UseMedkit(MedkitObject medkitObject,PlayerHealth playerHealth)
    {
        medkitObject.RestoreHealth(playerHealth);
        medkitObject.itemDepleted = true;
        Debug.Log("playerHealed");
        //inventoryObj.EquipEmpty(medkitObject.handID);
    }
    public void TurnOnFlashlight(FlashlightObject flashlightObject)
    {
        //playerRigController.AnimatorOnItemUsage(flashlightObject.handID,flashlightObject.timeToUse,true);\
        playerAudioSource.PlayOneShot(flashlightObject.turnOnSound);
        flashlightObject.ToggleFlashlight(true);
        Debug.Log("linterna encendida!");
    }
    public void TurnOffFlashlight(FlashlightObject flashlightObject)
    {
       //playerRigController.AnimatorOnItemUsage(flashlightObject.handID,flashlightObject.timeToUse,false);
       playerAudioSource.PlayOneShot(flashlightObject.turnOffSound);
       flashlightObject.ToggleFlashlight(false);
       Debug.Log("linterna apagada!");
    }
    public float ItemDrain(ItemObject itemObj, float timeUsed)
    {
        if(itemObj.itemHealth<= 0)
        {
            itemObj.itemDepleted = true;
            itemObj.itemHealth = 0;
            return 0;
        }
        return itemObj.itemDrain * timeUsed;
    }
    public void SwapHands(ItemObject left, ItemObject right)
    {
        ClearCurrentHandObj(currentLeftHandObject);
        ClearCurrentHandObj(currentRightHandObject);
        currentLeftHandObject = CreateItemInstanceInHand(right, leftHandTransform);
        currentRightHandObject = CreateItemInstanceInHand(left, rightHandTransform);
        HandleComponentsInHand(currentLeftHandObject);
        HandleComponentsInHand(currentRightHandObject);
        inventoryObj.inventory.leftHandItem = new Item(right);
        inventoryObj.inventory.rightHandItem = new Item(left);
    }
    public void StoreCurrentItem(int whichHand)
    {
        Item itemToStore = (whichHand == 0) ? inventoryObj.inventory.leftHandItem : inventoryObj.inventory.rightHandItem;
        if(itemToStore.ID !=0) inventoryObj.AddItem(itemToStore, 1);
        inventoryObj.EquipEmpty(whichHand);
        //playerRigController.AnimatorOnStoredItem(whichHand);
    }
    public void DropItemFromHand(GameObject obj)
    {
        if (obj)
        {
            HandleComponentsOnDrop(obj);
            //inventoryObj.UpdateHandItem(null,)
        } 
    }

    public void DropStack(ItemObject itemObj, int amount)
    {
        //Debug.Log(itemObj.GetPrefab());
        for (int i = 0; i < amount; i++)
        {
            var itemObjectInstance = Instantiate(itemObj.GetPrefab(), dropTransform);
            itemObjectInstance.transform.localPosition = itemObj.GetLocalPosition();
            itemObjectInstance.transform.localRotation = itemObj.GetLocalRotatition();
            itemObjectInstance.transform.localScale = itemObj.GetLocalScale();
            HandleComponentsOnDrop(itemObjectInstance);

        }
    }
    
    private GameObject CreateItemInstanceInHand(ItemObject itemObj, Transform anchor)
    {
        var batteryInstance = Instantiate(itemObj.GetPrefab(), anchor);
        HandleComponentsInHand(batteryInstance);
        batteryInstance.transform.localPosition = itemObj.GetLocalPosition();
        batteryInstance.transform.localRotation = itemObj.GetLocalRotatition();
        batteryInstance.transform.localScale = itemObj.GetLocalScale();
        return batteryInstance;
    }
    private GameObject CreateEmptyInstanceInHand(EmptyObject emptyObj, Transform anchor)
    {
        var emptyInstance = Instantiate(emptyObj.GetPrefab(), anchor);
        emptyInstance.transform.localPosition = emptyObj.GetLocalPosition();
        emptyInstance.transform.localRotation = emptyObj.GetLocalRotatition();
        emptyInstance.transform.localScale = emptyObj.GetLocalScale();
        return emptyInstance;
    }
    private GameObject CreateBatteryInstanceInHand(BatteryObject battery, Transform anchor)
    {
        var batteryInstance = Instantiate(battery.GetPrefab(), anchor);
        HandleComponentsInHand(batteryInstance);
        batteryInstance.transform.localPosition = battery.GetLocalPosition();
        batteryInstance.transform.localRotation = battery.GetLocalRotatition();
        batteryInstance.transform.localScale = battery.GetLocalScale();
        return batteryInstance;
    }
    private GameObject CreateMedkitInstanceInHand(MedkitObject medkit, Transform anchor)
    {
        var medkitInstance = Instantiate(medkit.GetPrefab(), anchor);
        HandleComponentsInHand(medkitInstance);
        medkitInstance.transform.localPosition = medkit.GetLocalPosition();
        medkitInstance.transform.localRotation = medkit.GetLocalRotatition();
        medkitInstance.transform.localScale = medkit.GetLocalScale();  
        return medkitInstance;
    }
    private GameObject CreateFlashlightInstanceInHand(FlashlightObject flashlight, Transform anchor)
    {
        var flashlightInstance = Instantiate(flashlight.GetPrefab(), anchor);
        HandleComponentsInHand(flashlightInstance);
        flashlightInstance.transform.localPosition = flashlight.GetLocalPosition();
        flashlightInstance.transform.localRotation = flashlight.GetLocalRotatition();
        flashlightInstance.transform.localScale = flashlight.GetLocalScale();
        //flashlight.GetFlashlightLight().SetActive(false);
        flashlight.flashlightLight = flashlightInstance.GetComponentInChildren<Light>();
        return flashlightInstance;
    }
    private void ClearCurrentHandObj(GameObject obj)
    {
        if (obj) Destroy(obj);
    }
    public Transform GetUIParent()
    {
        return inventoryUIParent;
    }
    public void HandleComponentsOnDrop(GameObject obj)
    {
        if (!obj.TryGetComponent<Rigidbody>(out var rigidbody)) obj.AddComponent<Rigidbody>();
        obj.GetComponent<SphereCollider>().center = Vector3.zero;
        //obj.GetComponent<SphereCollider>().radius = 5;
        obj.GetComponent<GroundItem>().inHand = false;
        Behaviour itemGlow = (Behaviour)obj.GetComponent("Halo");
        itemGlow.enabled = true;
        obj.transform.parent = null;
    }

    public void HandleComponentsInHand(GameObject obj)
    {
        Destroy(obj.GetComponent<Rigidbody>());
        obj.GetComponent<SphereCollider>().center = new Vector3(-5, -5, -5);
        //obj.GetComponent<SphereCollider>().radius = 0;
        obj.GetComponent<GroundItem>().inHand = true;
        Behaviour itemGlow = (Behaviour)obj.GetComponent("Halo");
        itemGlow.enabled = false;
    }
}
