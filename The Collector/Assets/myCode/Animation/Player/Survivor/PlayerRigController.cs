using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Threading.Tasks;
public class PlayerRigController : MonoBehaviour
{
    public Animator playerAnimator;
    private AnimatorHelper animatorHelper;
    private PlayerItemController playerItemController;
    [SerializeField] private bool sprintCutscene;
    [SerializeField] private bool idleCutscene;
    [SerializeField] private bool walkCutscene;
    //private FirstPersonPlayer player;
    public AudioSource playerInteractionAudioSource;
    public Transform playerRightHandIKTarget;
    public Transform playerRightHandPickUpBone;
    public GroundItem currentInteractionItem;
    public GameObject currentInteractionObj;
    public Rig pickUpRig;
    public Rig headRig;
    public bool isPickingUp;
    //public bool usingItemLeft;
    //public bool usingItemRight;

    //public FirstPersonPlayer player; //fix this soon after you understand the rigging. The rig controller doesnt need to know about player, the player should be able to just pass info to this.
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponentInParent<FirstPersonPlayer>();
        isPickingUp = false;
        playerAnimator = GetComponent<Animator>();
        playerInteractionAudioSource = GetComponentInParent<AudioSource>();
        playerItemController = GetComponentInParent<PlayerItemController>();
        animatorHelper = GetComponent<AnimatorHelper>();
        if (sprintCutscene) SetForwardAnimationState(6, true);
        else if (idleCutscene) SetForwardAnimationState(0, false);
        else if (walkCutscene) SetForwardAnimationState(2, false);
    }

    // Update is called once per frame
    void Update()
    {
        
        //currentInteractionItem = player.currentInteractionItem;
        //currentInteractionObj = 

    }
    private void SetForwardAnimationState(float val, bool isSprinting)
    {
        playerAnimator.SetFloat("CurrentInput", val);
        playerAnimator.SetBool("isSprinting", isSprinting);
    }
    public void AnimatorOnEquippedItem(int whichHand, float itemEquippedIndex, bool isEquippingEmpty) //have an AnimatorOnBigItem function for things that requiere 2 hands to use.
    {
        if (whichHand == 0)
        {
            playerAnimator.SetFloat("ItemEquippedLH", itemEquippedIndex);
            animatorHelper.TurnOffAnimationLayer(playerAnimator,2);
            if (isEquippingEmpty) animatorHelper.TurnOffAnimationLayer(playerAnimator,3);
            else animatorHelper.TurnOnAnimationLayer(playerAnimator,3);
        }
        else if (whichHand == 1)
        {
            playerAnimator.SetFloat("ItemEquippedRH", itemEquippedIndex);
            animatorHelper.TurnOffAnimationLayer(playerAnimator,4);
            if(isEquippingEmpty) animatorHelper.TurnOffAnimationLayer(playerAnimator,5);
            else animatorHelper.TurnOnAnimationLayer(playerAnimator,5);
        }
    }
    public async void AnimatorOnItemUsage(int whichHand, bool isStartingToUse)
    {
        if (whichHand == 0)
        {
            if(isStartingToUse) animatorHelper.TurnOnAnimationLayer(playerAnimator,2);
            else
            {
                await Task.Delay(250);
                animatorHelper.TurnOffAnimationLayer(playerAnimator,2);
            }      
        }
        else if (whichHand == 1)
        {
            if(isStartingToUse) animatorHelper.TurnOnAnimationLayer(playerAnimator,4);
            else
            {
                await Task.Delay(250);
                animatorHelper.TurnOffAnimationLayer(playerAnimator,4);
            }
        }
    }

    private void OnAnimationCompleteItemUse()//animation event function. That's why there are no script references
    {

    }
    private void OnAnimationGrabbedItem() //animation event function. That's why there are no script references
    {
        //player.canMove = false;
        isPickingUp = true;
        currentInteractionObj.transform.SetParent(playerRightHandPickUpBone, true);
        currentInteractionObj.transform.localPosition = Vector3.zero;
    }

    private void OnAnimationStoredItem() //same as function above. In future find way to clean up the layer toggling and link it to the "OnAnimator" functions above.
    {
        playerAnimator.SetTrigger("DonePickingUp");
        //Item newItem = new Item(currentInteractionItem.itemObject);
        playerInteractionAudioSource.PlayOneShot(currentInteractionItem.pickUpSound);
        Destroy(currentInteractionObj);
        playerItemController.inventoryObj.AddItem(new Item(currentInteractionItem.itemObject), 1);
        if (playerItemController.currentRightHandObject) playerItemController.currentRightHandObject.SetActive(true);
        animatorHelper.TurnOffAnimationLayer(playerAnimator,2);
        animatorHelper.TurnOffAnimationLayer(playerAnimator,4);
        animatorHelper.TurnOnAnimationLayer(playerAnimator,5);
        isPickingUp = false;
        //player.canMove = true;
    }
    
}
