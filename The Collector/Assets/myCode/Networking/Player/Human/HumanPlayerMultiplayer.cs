//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Animations.Rigging;
//using UnityEngine.Networking;
//using System.Threading.Tasks;

//public class HumanPlayerMultiplayer : MonoBehaviour
//{
//    public bool CanMove {get; private set; } = true;
//    private bool isPickingUp => Input.GetKey(pickUpKey); //&& itemInRange; //&& currentInteractable;
//    private bool isUsingInventory => Input.GetKey(inventoryKey);
//    private bool isStoring => Input.GetKey(storeItemKey);
//    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
//    private bool canUseItem => !isUsingInventory && !isUsingHotkeys && !isStoring && !isPickingUp;
//    private bool usingFreeLook => Input.GetKey(freeLookKey);
//    private bool usingLeftHandItem => Input.GetKey(LeftHandKey);
//    private bool usingRightHandItem => Input.GetKey(RightHandKey);
//    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
//    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !isDuringCrouchAnimation && characterController.isGrounded && !isSprinting; //&& !isBusy; //!isPi;

//    [Header("Functional Options")]
//    [SerializeField] private bool canSprint = true;
//    [SerializeField] private bool canJump = true;
//    [SerializeField] private bool canCrouch = true;
//    [SerializeField] private bool canUseHeadbob = true;
//    [SerializeField] private bool slidesOnSlopes = true;
//    [SerializeField] private bool canZoom = true;
//    [SerializeField] private bool canInteract = true;
//    [SerializeField] private bool useFootsteps = true;
//    public bool canMove = true;
//    public bool isGrounded;

//    [Header("Control Buttons")] //end product will use Buttons instead so the user can customize their own controls.
//    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; 
//    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
//    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
//    [SerializeField] private KeyCode zoomKey = KeyCode.Z;
//    [SerializeField] private KeyCode freeLookKey = KeyCode.C;
//    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
//    [SerializeField] private KeyCode dropKey = KeyCode.Q;
//    [SerializeField] private KeyCode interactKey = KeyCode.F;
//    [SerializeField] private KeyCode stealthKey = KeyCode.LeftAlt;
//    [SerializeField] private KeyCode LeftHandKey = KeyCode.Mouse0;
//    [SerializeField] private KeyCode RightHandKey = KeyCode.Mouse1;
//    [SerializeField] private KeyCode switchHandKey = KeyCode.Mouse2;
//    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;
//    [SerializeField] private KeyCode storeItemKey = KeyCode.X;
//    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    
//    [SerializeField] 
//    private KeyCode[] inventoryHotbarKeys = new KeyCode[]
//    {
//        KeyCode.Alpha1,
//        KeyCode.Alpha2,
//        KeyCode.Alpha3,
//        KeyCode.Alpha4,
//        KeyCode.Alpha5,
//        KeyCode.Alpha6,
//        KeyCode.Alpha7,
//        KeyCode.Alpha8,
//        KeyCode.Alpha9,
//    };
    
//   [Header("Movement Parameters")]
//    [SerializeField] private float walkSpeed = 3.0f;
//    [SerializeField] private float sprintSpeed = 6.0f;
//    [SerializeField] private float crouchSpeed = 1.0f;
//    [SerializeField] private float slideSpeed = 4.0f;
//    [SerializeField] private bool isStrafing;
//    [SerializeField] private Vector3 moveDirection;
//    [SerializeField] private Vector2 currentInput;
//    private float localCharacterForwardInput;

//    [Header("Movement Sound Parameters")]
//    [SerializeField] private float baseStepSpeed = 0.5f;
//    [SerializeField] private float crouchStepMultiplier = 1.5f;
//    [SerializeField] private float sprintStepMultiplier = 0.6f;
//    [SerializeField] private List<AudioClip> footstepSounds = default;
//    [SerializeField] private AudioClip jumpSound = default;
//    [SerializeField] private AudioClip landSound = default;
//    [SerializeField] private AudioClip crouchSound = default;
//    public AudioSource playerAudioSource = default;
//    private float footstepTimer = 0;
//    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
//    private FootstepChanger changer;

//    [Header("Look Parameters")]
//    [SerializeField] private bool testingCam;
//    [SerializeField] private Camera playerCamera;
//    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
//    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
//    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
//    [SerializeField,Range(1,100)] private float sideLookLimit = 80.0f;
//    [SerializeField] private Vector3 standingCameraLocalPos = new Vector3(0, 0.8f, 0.3f);
//    [SerializeField] private Vector3 crouchingCameraLocalPos = new Vector3(0.2f, 0, 0.6f);
//    [SerializeField] private Vector3 crouchingCameraRotation = new Vector3(45f, 0, 0);
//    [SerializeField] private Vector3 sprintCameraLocalPos = new Vector3(0, 0.75f, 0.6f);

//    [Header("Jumping Parameters")]
//    [SerializeField] private float jumpStrength = 8.0f;
//    [SerializeField] private float gravity = -20.0f; //This combo can be used to make space gravity
//    [SerializeField] private bool isJumping = false;
//    [SerializeField] private bool isFalling = false;
//    //[SerializeField] private bool isFalling => !characterController.isGrounded;

//    [Header("Crouch Parameters")]
//    [SerializeField] private float crouchingHeight = 0.85f;//= 0.5f;
//    [SerializeField] private float standingHeight = 2.0f;// = 2f;
//    [SerializeField] private float timeToCrouch = 0.4f;
//    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, -0.25f, 0); //controller centerpoint when crouching
//    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0); //controller centerpoint when standing
//    [SerializeField] private float standingRadius = 0.5f;
//    [SerializeField] private float crouchingRadius = 0.6f;
//    public bool isBusy;
//    public bool isCrouching;
//    private bool isDuringCrouchAnimation;
//    private Coroutine crouchRoutine;
//    private Coroutine sprintRoutine;
    
//    [Header("Headbob Parameters")]
//    [SerializeField] private float walkBobSpeed = 14.0f;
//    [SerializeField] private float walkBobAmount = 0.05f;
//    [SerializeField] private float sprintBobSpeed = 18.0f;
//    [SerializeField] private float sprintBobAmount = 0.1f;
//    [SerializeField] private float crouchBobSpeed = 8f;
//    [SerializeField] private float crouchBobAmount = 0.025f;
//    private float defaultYPos = 0;
//    private float timer;

//    [Header("Zoom Parameters")]
//    [SerializeField] private float timeToZoom = .3f;
//    [SerializeField] private float zoomFOV = 30f;
//    private float defaultFOV;
//    private Coroutine zoomRoutine;


//    //Sliding Parameters
//    private Vector3 hitPointNormal;

//    [Header("Player Data")]
//    public int health;
//    public int speed;
//    public int stamina;
//    private Rig rig;
//    public PlayerHealth healthOfPlayer;
//    public PlayerSpeed speedOfPlayer; 
//    public PlayerStamina staminaOfPlayer;
//    private Animator playerAnimator; //needs to be removed all info for animator will come from playerRigController in future.
//    private PlayerItemController playerItemController;
//    private PlayerRigController playerRigController;
//    [SerializeField] Transform rightHandIKTarget;
//    [SerializeField] Transform rightHandBone;
//    private Item leftHandItem;
//    private Item rightHandItem;
//    public Coroutine itemUseRightRoutine = null;
//    public Coroutine itemUseLeftRoutine = null;
//    public FlashlightObject playerFlashlight;

//    [Header("GUI")]
//    [SerializeField] private GameObject inventoryPanel;
//    //private GameObject pauseUI;
//    //private PauseGameMenu gameMenuScript;
//    [SerializeField] private GameObject pauseMenuObject;
//    [SerializeField] private MenuController menuController;

//    //private Inventory inventory;
//    //[SerializeField] private UI_Inventory uiInventory;
//    //public InventorySystem inventorySystem;
//    //public GameObject inventoryUI;
//    private bool isSliding
//    {
//        get
//        {
//            Debug.DrawRay(transform.position, Vector3.down, Color.red);
//            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 1f))
//            {
//                hitPointNormal = slopeHit.normal;
//                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }

//    private bool isUsingHotkeys 
//    {
//       get
//        {
//            for (int i = 0; i < inventoryHotbarKeys.Length; i++)
//                if (Input.GetKey(inventoryHotbarKeys[i]))
//                    return true;
//            return false;
//        }
//    }

//    [Header("Interaction")]
//    [SerializeField] private Vector3 interactionRayPoint = default;
//    [SerializeField] private float interactionDistance = default;
//    [SerializeField] private LayerMask interactionLayerMask = default;

//    //private Item currentItem;
//    private bool itemInRange;
//    private CharacterController characterController;
//    private CapsuleCollider capsuleCollider;
    

//    private float rotationX = 0;
//    private float rotationY = 0;
//    private void OnLevelWasLoaded()
//    {
       
//    }
//    void Awake()
//    {
//        //cache components
//        healthOfPlayer = new PlayerHealth(100);
//        speedOfPlayer = new PlayerSpeed(100);
//        staminaOfPlayer = new PlayerStamina(100);
//        playerCamera = GetComponentInChildren<Camera>();
//        characterController = GetComponent<CharacterController>();
//        capsuleCollider = GetComponent<CapsuleCollider>();
//        playerAnimator = GetComponentInChildren<Animator>();
//        rig = GetComponentInChildren<Rig>();
//        playerRigController = GetComponentInChildren<PlayerRigController>();
//        playerItemController = GetComponent<PlayerItemController>();
//        //inventoryPanel = GameObject.Find("InventoryPanel");
//        //pauseUI = GameObject.Find("PauseMenuCanvas");
//        //gameMenuScript = pauseUI.GetComponent<PauseGameMenu>();
//        changer = GetComponent<FootstepChanger>();
//        playerAudioSource = GetComponent<AudioSource>();
//        playerCamera.transform.localPosition = standingCameraLocalPos;
//        defaultYPos = playerCamera.transform.localPosition.y;
//        defaultFOV = playerCamera.fieldOfView;
//        inventoryPanel.transform.localScale = playerItemController.inventoryObj.hiddenUIScale;
//        menuController = pauseMenuObject.GetComponentInChildren<MenuController>();
//    }

//    void Update()
//    {
//        HandleHealth();
//        HandleStamina();
//        HandleSpeed();
//        HandlePauseMenu();
//        HandleInventory();
//        HandleCursor();
//        if (CanMove)
//        {
//            HandleMovementInput();
//            HandleItems();
//            if (!testingCam && !isUsingInventory)
//                HandleMouseInput();
//            if (canZoom)
//                HandleZoom();
//            if (canUseHeadbob)
//                HandleHeadbob();
//            if (canJump)
//                HandleJump();
//            if (canCrouch)
//                HandleCrouch();
//            if (canSprint)
//                HandleSprint();
//            if (canInteract)
//                HandleInteractionInput();
//            //HandleInteractionCheck();
//            if (canUseItem)
//                HandleItemUsage();
//            if (useFootsteps)
//                HandleFootsteps();
//            ApplyPhysics();
//        }
//    }
//    public void ConnectToMenu()
//    {
//        //find a way to give the menu the player object so it can load properly.
//    }
//    public void HandlePauseMenu()
//    {
//        if (Input.GetKeyDown(pauseKey))
//        {
//            bool gameIsPaused = menuController.gameIsPaused;
//            if (!gameIsPaused)
//            {
//                pauseMenuObject.SetActive(true);
//                menuController.TogglePause();
//            }
//            else
//            {
//                pauseMenuObject.SetActive(false);
//                menuController.TogglePause();
//            }
//        }
//    }
//    private void HandleHealth()
//    {
//        health = healthOfPlayer.GetHealth();
//    }
//    private void HandleSpeed()
//    {
//        speed = speedOfPlayer.GetSpeed();
//    }
//    private void HandleStamina()
//    {
//        stamina = staminaOfPlayer.GetStamina();
//    }
//    private void HandleMovementInput()
//    {
//        canMove = !playerRigController.isPickingUp;
//        if (canMove)
//        {
//            currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"), (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical")); //scales -1,0,or 1 from the Input manager and multiplies by movespeed of playear
//            localCharacterForwardInput = currentInput.y;
//            UpdateFwdBwdAnimations();
//            UpdateStrafingAnimations();
//            //UpdatePickUpAnimation();
//            UpdateSprintAnimation();
//            float moveDirectionY = moveDirection.y;
//            moveDirection = ((transform.TransformDirection(Vector3.right) * currentInput.x)
//             + (transform.TransformDirection(Vector3.forward) * currentInput.y)); //we are using x and z components of moveDirection for our player ground movement.
//            moveDirection.y = moveDirectionY;
//            characterController.Move(moveDirection * Time.deltaTime);
//        } 
//    }
//    private void HandleMouseInput()
//    {
//        if (!menuController.gameIsPaused)
//        {
//            //if (usingFreeLook)
//            //{
//            //    playerCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            //    rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
//            //    rotationY += Input.GetAxis("Mouse X") * lookSpeedX;
//            //    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
//            //}

//            //use Input get axis as soon as w
//            playerAnimator.SetFloat("MouseInputForTurn", Input.GetAxis("Mouse X") * lookSpeedX);
//            //Debug.Log(Input.GetAxis("Mouse X")* lookSpeedX);
//            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
//            rotationY += Input.GetAxis("Mouse X") * lookSpeedX; //what we link to animator to turn feet

//            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, upperLookLimit); //these limits will be replaced with reference to the rig constraints.
//                                                                                 //rotationY = Mathf.Clamp(rotationY, -sideLookLimit, sideLookLimit); //can use this for a checker to when to use feet or I can move feet with every look
//            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
//            //playerCamera.transform.localRotation = Quaternion.Euler(0,rotationY, 0); 
//            transform.rotation = Quaternion.Euler(0, rotationY, 0);

//            //if (localCharacterForwardInput > 0)
//            //{
//            //    transform.rotation = Quaternion.Euler(0, rotationY, 0);
//            //    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
//            //}
//            //else
//            //{
//            //    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
//            //}

//            //playerCamera.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
//        }
//    }
//    private void HandleCursor()
//    {
//        if (menuController.gameIsPaused || isUsingInventory)
//        {
//            Cursor.lockState = CursorLockMode.None;
//            Cursor.visible = true;
//        }
//        else
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;
//        }
//    }
//    private void HandleInventory()
//    {
//        HandleInventoryHotKeys();
//        HandleInventoryUI();
//    }
//    private void HandleItems()
//    {
//        HandleItemStoring();
//        HandleItemHandSwapping();
//        HandleItemDropInput();
//    }
//    private void HandleInventoryUI()
//    {
//        if (Input.GetKeyDown(inventoryKey))
//        {
//            playerAudioSource.PlayOneShot(playerItemController.inventoryObj.displaySound,0.4f);
//            inventoryPanel.transform.localScale = playerItemController.inventoryObj.visibleUIScale;
//        }
//        if (Input.GetKeyUp(inventoryKey))
//        {
//            playerAudioSource.PlayOneShot(playerItemController.inventoryObj.hideSound, 0.4f);
//            inventoryPanel.transform.localScale = playerItemController.inventoryObj.hiddenUIScale;
//        }
//    }
//    private void HandleInventoryHotKeys()
//    {
//        HandleHotKeyStoring();
//        HandleHotKeyEquipping();
//    }
//    private void HandleHotKeyEquipping()
//    {
//        for (int i = 0; i < inventoryHotbarKeys.Length; i++)
//        {
//            if (Input.GetKey(inventoryHotbarKeys[i]))
//            {
//                if (playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item != null)
//                {
//                    if (Input.GetKeyDown(LeftHandKey)) playerItemController.inventoryObj.EquipItem(playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item, 0);
//                    else if (Input.GetKeyDown(RightHandKey)) playerItemController.inventoryObj.EquipItem(playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item, 1);
//                }
//            }
//        }
//    }
//    private void HandleHotKeyStoring()
//    {
//        //for (int i = 0; i < inventoryHotbarKeys.Length; i++)
//        //{
//        //    if (Input.GetKey(LeftHandKey))
//        //    {
//        //        if (Input.GetKeyDown(storeEquippedItemKey))
//        //        {
//        //            if (playerItemController.inventoryObj.currentItem != null)
//        //            {
//        //                if (playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item == playerItemController.inventoryObj.currentItem && !playerItemController.inventoryObj.inventory.inventoryItemSlots[i].isFull)
//        //                {
//        //                    playerItemController.inventoryObj.inventory.inventoryItemSlots[i].AddAmount(1);
//        //                    playerItemController.inventoryObj.EquipEmpty(0);
//        //                }
//        //                //else if(playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item != playerItemController.inventoryObj.currentItem && !playerItemController.inventoryObj.inventory.inventoryItemSlots[i].isFull)
//        //                else
//        //                    return;
//        //            }
//        //        }
                
//        //    }
//        //    else if (Input.GetKey(RightHandKey))
//        //    {
//        //        if (Input.GetKeyDown(storeEquippedItemKey))
//        //        {
//        //            if (playerItemController.inventoryObj.currentItem1 != null)
//        //            {
//        //                if (playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item == playerItemController.inventoryObj.currentItem1 && !playerItemController.inventoryObj.inventory.inventoryItemSlots[i].isFull)
//        //                {
//        //                    playerItemController.inventoryObj.inventory.inventoryItemSlots[i].AddAmount(1);
//        //                    playerItemController.inventoryObj.EquipEmpty(1);
//        //                }
//        //                //else if(playerItemController.inventoryObj.inventory.inventoryItemSlots[i].item != playerItemController.inventoryObj.currentItem && !playerItemController.inventoryObj.inventory.inventoryItemSlots[i].isFull)
//        //                else
//        //                    return;
//        //            }
//        //        }
//        //    }
//        //}
//    }
//    private void HandleItemStoring() //reverse logic to hold item button then press specific key to do player action.
//    {
//        if (Input.GetKey(storeItemKey))
//        {
//            if (Input.GetKeyDown(LeftHandKey))
//            {
//                //playerItemController.inventoryObj.AddItem(playerItemController.inventoryObj.currentItem, 1);
//                playerItemController.StoreCurrentItem(0);
//                Debug.Log("Lefthand item stored");
//            }
//            else if (Input.GetKeyDown(RightHandKey))
//            {
//                //playerItemController.inventoryObj.AddItem(playerItemController.inventoryObj.currentItem, 1);
//                playerItemController.StoreCurrentItem(1);
//                Debug.Log("Righthand item stored");
//            }
//        }
//    }
//    private void HandleItemHandSwapping()
//    {
//        if (Input.GetKeyDown(switchHandKey))
//        {
//            playerItemController.SwapHands(playerItemController.inventoryObj.inventory.leftHandItem.itemObj, playerItemController.inventoryObj.inventory.rightHandItem.itemObj);
//            Debug.Log("Hand items swapped");
//        }   
//    }
//    private void HandleItemUsage()
//    {
//        UpdateAnimatorForItemUse();
//        if (canUseItem)
//        {
//            if (itemUseRightRoutine == null) HandleItemUsageLeft();
//            if (itemUseLeftRoutine == null) HandleItemUsageRight();
//            HandleItemDepletion();
//        }
//    }
//    //private void HandleEquipmentItemUsage()
//    //{
//    //    if(canUseItem)
//    //    {
//    //        //if(usingItem){
//    //        //    if (Input.GetKeyDown(LeftHandKey) && playerItemController.inventoryObj.currentItem.itemObj != null)
//    //        //    {
//    //        //        usingItem =true;
//    //        //        if (Input.GetKeyUp(LeftHandKey))
//    //        //            playerItemController.inventoryObj.currentItem.itemObj.UseItem(playerItemController);
//    //        //    }

//    //        //    else if (Input.GetKeyDown(RightHandKey) && playerItemController.inventoryObj.currentItem1.itemObj != null)
//    //        //        playerItemController.inventoryObj.currentItem1.itemObj.UseItem(playerItemController);
//    //        //}
//    //        //if (Input.GetKey(interactKey))
//    //        if (Input.GetKeyDown(LeftHandKey) && playerItemController.inventoryObj.inventory.leftHandItem.itemObj.isEquipmentObject)
//    //        {
//    //            playerItemController.inventoryObj.inventory.leftHandItem.itemObj.UseItem(playerItemController);
//    //            Debug.Log("Started Using Item in left hand.");
//    //            //playerRigController.TurnOnAnimationLayer(2);
//    //            //playerRigController.TurnOffAnimationLayer(4);
//    //        }
//    //        else if (Input.GetKeyDown(RightHandKey) && playerItemController.inventoryObj.inventory.rightHandItem.itemObj.isEquipmentObject)
//    //        {
//    //            playerItemController.inventoryObj.inventory.rightHandItem.itemObj.UseItem(playerItemController);
//    //            Debug.Log("Started Using Item in right hand.");
//    //            //playerRigController.TurnOnAnimationLayer(4);
//    //            //playerRigController.TurnOffAnimationLayer(2);
//    //        }
//    //        if(Input.GetKeyUp(LeftHandKey)) playerItemController.inventoryObj.inventory.leftHandItem.itemObj.StopUsingItem(playerItemController);
//    //        else if(Input.GetKeyUp(RightHandKey)) playerItemController.inventoryObj.inventory.rightHandItem.itemObj.StopUsingItem(playerItemController);
//    //        UpdateAnimatorForItemUse();
//    //    }  
//    //}
//    private void HandleInteractionInput()
//    {
//        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayerMask))
//        {
//            //var item = hit.collider.GetComponent<GroundItem>();
//            playerRigController.currentInteractionItem = hit.collider.GetComponent<GroundItem>();
//           if (playerRigController.currentInteractionItem && !playerRigController.currentInteractionItem.inHand)
//           {
//                playerRigController.currentInteractionObj = hit.collider.gameObject;
                
//                if (Input.GetKeyDown(pickUpKey)) //isPickingUp
//                {

//                    Debug.Log(hit.collider.gameObject);
//                    if(playerItemController.currentRightHandObject) playerItemController.currentRightHandObject.SetActive(false);
//                    playerAnimator.SetLayerWeight(5, 0);
//                    playerAnimator.SetLayerWeight(2, 1);
//                    playerAnimator.SetLayerWeight(4, 1);  
//                    Destroy(playerRigController.currentInteractionObj.GetComponent<Rigidbody>());
//                    Destroy(playerRigController.currentInteractionObj.GetComponent<SphereCollider>());
                    
//                    //rightHandIKTarget.transform.position = currentInteractionObj.transform.position;
//                    //playerRigController.currentInteractionObj = currentInteractionObj;
//                    playerAnimator.SetTrigger("GrabItem");
//                    //playerAnimator.ResetTrigger("GrabItem");

//                }
//            }
//        }
//    }
//    private void HandleItemUsageLeft()
//    {
//        ItemObject currentLeftItemObj = playerItemController.inventoryObj.inventory.leftHandItem.itemObj;
//        if (Input.GetKeyDown(LeftHandKey) && currentLeftItemObj.ID !=0)
//        {
//            if (itemUseLeftRoutine != null)
//            {
                
//                Debug.Log("Stopped coroutine left");
//                StopCoroutine(itemUseLeftRoutine);
//                itemUseLeftRoutine = null;
//            }
//            playerRigController.AnimatorOnItemUsage(currentLeftItemObj.handID, true);
//            itemUseLeftRoutine = StartCoroutine(ItemUseRoutine(currentLeftItemObj));
//        }
//        if (Input.GetKeyUp(LeftHandKey))
//        {
//            if (itemUseLeftRoutine != null)
//            {
//                Debug.Log("Stopped coroutine left");
//                StopCoroutine(itemUseLeftRoutine);
//                itemUseLeftRoutine = null;
//            }
//            currentLeftItemObj.StopUsingItem(playerItemController);
//            playerRigController.AnimatorOnItemUsage(currentLeftItemObj.handID, false);
//        }
//    }
   
//    private void HandleItemUsageRight()
//    {
//        ItemObject currentRightItemObj = playerItemController.inventoryObj.inventory.rightHandItem.itemObj;
//        if (Input.GetKeyDown(RightHandKey) && currentRightItemObj.ID != 0)
//        {
//            if (itemUseRightRoutine != null)
//            {
//                Debug.Log("Stopped coroutine right");
//                StopCoroutine(itemUseRightRoutine);
//                itemUseRightRoutine = null;
//            }
//            playerRigController.AnimatorOnItemUsage(currentRightItemObj.handID, true);
//            itemUseRightRoutine = StartCoroutine(ItemUseRoutine(currentRightItemObj));
//        }
//        if (Input.GetKeyUp(RightHandKey))
//        {
//            if (itemUseRightRoutine != null)
//            {
//                Debug.Log("Stopped coroutine right");
//                StopCoroutine(itemUseRightRoutine);
//                itemUseRightRoutine = null;
//            }
//            //currentRightItemObj.DepleteItem(playerItemController, itemUsageTime);
//            //Debug.Log(itemUsageTime);
//            playerRigController.AnimatorOnItemUsage(currentRightItemObj.handID, false);
//            currentRightItemObj.StopUsingItem(playerItemController);
//            //itemUsageTime = 0;
//        }
//        //UpdateAnimatorForItemUse();
//    }
//    //private IEnumerator RestorableItemUseRoutine(ItemObject itemObject)
//    //{
//    //    float timeElapsed = 0;
//    //    while(timeElapsed < itemObject.timeToUse)
//    //    {
//    //        timeElapsed += Time.deltaTime;
//    //        yield return null;
//    //    }
//    //    playerItemController.GetSurvivorPlayerVitals(this);
//    //    itemObject.UseItem(playerItemController);

//    //    //ItemUseRoutine = null;
//    //}
//    private IEnumerator ItemUseRoutine(ItemObject itemObject)
//    {
        
//        float timeElapsed = 0;
//        while (timeElapsed < itemObject.timeToUse)
//        {
//            timeElapsed += Time.deltaTime;
//            yield return null;
//        }
//        //playerItemController.GetSurvivorPlayerVitals(this);
//        itemObject.UseItem(playerItemController,this);
        
//        //Set function to update layerweight of injured layer in rig controller.
//        //ItemUseRoutine = null;
//    }
//    private void HandleItemDepletion()
//    {
//        if (usingLeftHandItem) playerItemController.inventoryObj.inventory.leftHandItem.itemObj.DepleteItem(playerItemController, Time.deltaTime);
//        else if (usingRightHandItem) playerItemController.inventoryObj.inventory.rightHandItem.itemObj.DepleteItem(playerItemController, Time.deltaTime);
//    }
//    private void HandleItemDropInput()
//    {
//        if (Input.GetKey(dropKey))
//        {
//            if (Input.GetKeyDown(LeftHandKey))
//            {
//                playerItemController.DropItemFromHand(playerItemController.currentLeftHandObject);
//                playerItemController.inventoryObj.inventory.leftHandItem = new Item(playerItemController.inventoryObj.itemDatabase.GetItemObj[0]);
//            }
//            else if (Input.GetKeyDown(RightHandKey))
//            {
//                playerItemController.DropItemFromHand(playerItemController.currentRightHandObject);
//                playerItemController.inventoryObj.inventory.rightHandItem = new Item(playerItemController.inventoryObj.itemDatabase.GetItemObj[0]);
//            }
//        } 
//    }
//    private void HandleSprint() //use this format for item use coroutine
//    {
//            if (Input.GetKeyDown(sprintKey))
//            {
//            isCrouching = false;
//            UpdateCrouchAnimation();
//                if (sprintRoutine != null)
//                {
//                    StopCoroutine(sprintRoutine);
//                    sprintRoutine = null;
//                }
//                sprintRoutine = StartCoroutine(HandleSprintCamera(true));
//            }
//            if (Input.GetKeyUp(sprintKey))
//            {
//                if (sprintRoutine != null)
//                {
//                    StopCoroutine(sprintRoutine);
//                    sprintRoutine = null;
//                }
//                sprintRoutine = StartCoroutine(HandleSprintCamera(false));
//            }
//    }
//    private IEnumerator HandleSprintCamera(bool toSprint)
//    {
//        //if (Physics.Raycast(playerCamera.transform.position, Vector3.forward, 3f))
//        //    yield break;
//        float timeElapsed = 0;
//        float sprintTransitionTime = 0.4f;
//        Vector3 targetCameraPos = toSprint ? sprintCameraLocalPos : standingCameraLocalPos;
//        Vector3 currentCameraPos = playerCamera.transform.localPosition;

//        while (timeElapsed < sprintTransitionTime)
//        {
//            playerCamera.transform.localPosition = Vector3.Lerp(currentCameraPos, targetCameraPos, timeElapsed / sprintTransitionTime);
//            timeElapsed += Time.deltaTime;
//            yield return null;
//        }
//        playerCamera.transform.localPosition = targetCameraPos;
//        sprintRoutine = null;
//    }
//    private void HandleCrouch()//we implement this to have smooth response to the user spamming the crouch button
//    {
//        if (shouldCrouch) StartCoroutine(HandleCrouchOrStandCamera());

//        //if (Input.GetKeyDown(crouchKey))
//        //{
//        //    if (sprintRoutine != null)
//        //    {
//        //        StopCoroutine(sprintRoutine);
//        //        sprintRoutine = null;
//        //    }
//        //    crouchRoutine = StartCoroutine(CrouchOrStand());
//        //}
//        //if (Input.GetKeyUp(zoomKey))
//        //{
//        //    if (zoomRoutine != null)
//        //    {
//        //        StopCoroutine(zoomRoutine);
//        //        zoomRoutine = null;
//        //    }
//        //    zoomRoutine = StartCoroutine(ToggleZoom(false));
//        //}
//    }
//    private IEnumerator HandleCrouchOrStandCamera()
//    {
//        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
//            yield break;

//        isDuringCrouchAnimation = true;
//        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
//        float currentHeight = characterController.height;
//        float targetRadius = isCrouching ? standingRadius : crouchingRadius;
//        float currentRadius = characterController.radius;
//        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
//        Vector3 currentCenter = characterController.center;
//        Vector3 targetCamCenter = isCrouching ? standingCameraLocalPos : crouchingCameraLocalPos;
//        Vector3 currentCamCenter = playerCamera.transform.localPosition;
//        isCrouching = !isCrouching;
//        UpdateCrouchAnimation();
//        PlayCrouchSound();
//        float timeElapsed = 0;
//        while (timeElapsed < timeToCrouch)
//        {
//            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
//            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
//            characterController.radius = Mathf.Lerp(currentRadius, targetRadius, timeElapsed / timeToCrouch);
//            capsuleCollider.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
//            capsuleCollider.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
//            capsuleCollider.radius = Mathf.Lerp(currentRadius, targetRadius, timeElapsed / timeToCrouch);
//            playerCamera.transform.localPosition = Vector3.Lerp(currentCamCenter, targetCamCenter, timeElapsed / timeToCrouch);
//            timeElapsed += Time.deltaTime;
//            yield return null;
//        }

//        characterController.height = targetHeight;
//        characterController.center = targetCenter;
//        characterController.radius = targetRadius;
//        capsuleCollider.height = targetHeight;
//        capsuleCollider.center = targetCenter;
//        capsuleCollider.radius = targetRadius;
//        playerCamera.transform.localPosition = targetCamCenter;
//        isDuringCrouchAnimation = false;
//        crouchRoutine = null;
//    }
//    private void HandleZoom() //we implement this to have smooth response to the user spamming the zoom button
//    {
//        if (isPickingUp || isUsingInventory)
//        {
//            StartCoroutine(HandleZoomCamera(false));
//            return;
//        }
//            if (Input.GetKeyDown(zoomKey))
//            {
//                if (zoomRoutine != null)
//                {
//                    StopCoroutine(zoomRoutine);
//                    zoomRoutine = null;
//                }
//                zoomRoutine = StartCoroutine(HandleZoomCamera(true));
//            }
//            if (Input.GetKeyUp(zoomKey))
//            {
//                if (zoomRoutine != null)
//                {
//                    StopCoroutine(zoomRoutine);
//                    zoomRoutine = null;
//                }
//                zoomRoutine = StartCoroutine(HandleZoomCamera(false));
//            }
//    }
//    private IEnumerator HandleZoomCamera(bool zoomingIn)
//    {
//        float targetFOV = zoomingIn ? zoomFOV : defaultFOV; //if true we want to achieve zoomed-in FOV if not we want to achieve normal FOV
//        float startingFOV = playerCamera.fieldOfView;
//        float timeElapsed = 0;
//        while (timeElapsed < timeToZoom)
//        {
//            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
//            timeElapsed += Time.deltaTime;
//            yield return null;
//        }
//        playerCamera.fieldOfView = targetFOV;
//        zoomRoutine = null;
//    }
//    private void ApplyPhysics()
//    {
//        isGrounded = characterController.isGrounded;
//        UpdateFallAnimation();
//        if (!characterController.isGrounded)
//        {
//            isFalling = true;
//            moveDirection.y += gravity * Time.deltaTime;
//        }
            
//        if (slidesOnSlopes && isSliding)
//            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z * slideSpeed); 
//    }
//    private void HandleJump()
//    {
//        if (isJumping || isFalling)
//        {
//            if (characterController.isGrounded)
//            {
//                isJumping = false;
//                isFalling = false;
//                UpdateJumpAnimation();
//                PlayLandingSound(); 
//            }
//        }
//        if (shouldJump)
//        {
//            isJumping = true;
//            UpdateJumpAnimation();
//            PlayJumpSound();
//            moveDirection.y = jumpStrength;
//        }
//    }
    
//    //private void HandleInteractionCheck()
//    //{
//    //    if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint),out RaycastHit hit, interactionDistance))
//    //    {
//    //        if(hit.collider.gameObject.layer == 9 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
//    //        {
//    //            hit.collider.TryGetComponent(out currentInteractable);

//    //            if (currentInteractable != null)
//    //                currentInteractable.OnFocus();
//    //        }
//    //    }
//    //    else if (currentInteractable)
//    //    {
//    //        currentInteractable.OnLoseFocus();
//    //        currentInteractable = null;
//    //    }
//    //}

//    //private void HandleInteractInput()
//    //{
//    //    if (isPickingUp && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayerMask))
//    //    {
//    //        //playerAnimator.SetBool("isInteracting", isInteracting);
//    //        currentInteractable.OnInteract();
//    //        currentInteractable.OnHandlePickupItem();

//    //    }
//    //    //if (Input.GetKeyDown(dropKey))
//    //    //{
//    //    //    currentInteractable.Drop();
//    //    //}
//    //}

//    //private void HandleInventory()
//    //{
//    //    if (Input.GetKeyDown(interactKey){

//    //    }

//    //}
//    private void HandleFootsteps()
//    {
//        if (!characterController.isGrounded)
//            return;
//        if (currentInput == Vector2.zero)
//            return;
//        footstepTimer -= Time.deltaTime;

//        if (footstepTimer <= 0)
//        {
//            PlayFootStepAudio();
//            //if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
//            //{
//            //    switch (hit.collider.tag)
//            //    {

//            //    }
//            //}
//            footstepTimer = GetCurrentOffset;
//        }
//    }
//    private void HandleHeadbob()
//    {
//        if (!characterController.isGrounded)
//            return;
//        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
//        {
//            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
//            playerCamera.transform.localPosition = new Vector3(
//                playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
//                playerCamera.transform.localPosition.z);
//        }
//    }
//    private void PlayJumpSound()
//    {
//        CheckSurface();
//        playerAudioSource.PlayOneShot(jumpSound);
//    }
//    private void PlayLandingSound()
//    {
//        CheckSurface();
//        playerAudioSource.PlayOneShot(landSound);
//        //m_NextStep = m_StepCycle + .5f;
//    }
//    private void PlayCrouchSound()
//    {
//        playerAudioSource.PlayOneShot(crouchSound);
//    }
//    private void PlayFootStepAudio()
//    {
//        // pick & play a random footstep sound from the list of the current surface
//        // excluding sound at index 0
//        // move picked sound to index 0 so it's not picked next time
//        CheckSurface();
//        int n = Random.Range(1, footstepSounds.Count);
//        playerAudioSource.PlayOneShot(footstepSounds[n]);
//        footstepSounds[n] = footstepSounds[0];
//        footstepSounds[0] = playerAudioSource.clip;
//    }
//    private void CheckSurface()
//    {
//        changer.CheckTerrainLayer();
//        changer.CheckSurfaceTag();
//    }
//    public void ChangeFootsteps(MovementSoundContainer container)
//    {
//        footstepSounds.Clear();
//        for (int i = 0; i < container.footstepSounds.Count; i++)
//        {
//            footstepSounds.Add(container.footstepSounds[i]);
//        }
//        jumpSound = container.jumpSound;
//        landSound = container.landSound;
//        crouchSound = container.crouchSound;
//    }
//    public void SavePlayer()
//    {
//        SaveSystem.SavePlayer(this);
//    }
//    public void LoadPlayer()
//    {
//        PlayerData data = SaveSystem.LoadPlayer(this);
//        healthOfPlayer = data.playerHealthData;
//        speedOfPlayer = data.playerSpeedData;
//        staminaOfPlayer = data.playerStaminaData;
//        Vector3 position;
//        position.x = data.pos[0];
//        position.y = data.pos[1];
//        position.z = data.pos[2];
//        transform.position = position;
//    }
//    public void UpdateAnimations()
//    {

//    }
//    private void UpdateFwdBwdAnimations()
//    {
//        playerAnimator.SetBool("isMoving", Mathf.Abs(moveDirection.z) > 0);
//        playerAnimator.SetBool("isMovingBwd", currentInput.y < 0);
//        playerAnimator.SetFloat("CurrentInput", isStrafing ? currentInput.x : currentInput.y); //Mathf.Abs(moveDirection.z) ?  have long if cases that change it different components of localfvelocity depending on state
//    }
//    private void UpdateCrouchAnimation()
//    {
//        playerAnimator.SetBool("isCrouching", isCrouching);
//    }
//    private void UpdateStrafingAnimations()
//    {
//        isStrafing = Mathf.Abs(currentInput.x) > 0;
//        playerAnimator.SetBool("isStrafing", isStrafing);
//    }
//    private void UpdateSprintAnimation()
//    {
//        playerAnimator.SetBool("isSprinting", isSprinting);
//    }
//    private void UpdateFallAnimation()
//    {
//        playerAnimator.SetBool("isFalling", isFalling);
//    }
//    private void UpdateJumpAnimation()
//    {
//        playerAnimator.SetBool("isJumping", isJumping);
//    }
//    private void UpdatePickUpAnimation()
//    {
//        playerAnimator.SetBool("isInteracting", isPickingUp);
//        //yield return new WaitForSeconds(1);
//    }
//    private void UpdateAnimatorForItemUse()
//    {
//        playerAnimator.SetBool("UsingItemLeft", usingLeftHandItem);
//        playerAnimator.SetBool("UsingItemRight", usingRightHandItem);
//        //playerRigController.usingItemLeft = usingLeftHandItem;
//        //playerRigController.usingItemRight = usingRightHandItem;
//    }
//    private void OnApplicationQuit()
//    {
//        playerItemController.inventoryObj.inventory.inventoryItemSlots = new InventorySlot[9];
//        playerItemController.inventoryObj.inventory.leftHandItem = new Item(playerItemController.inventoryObj.itemDatabase.GetItemObj[0]);
//        playerItemController.inventoryObj.inventory.rightHandItem = new Item(playerItemController.inventoryObj.itemDatabase.GetItemObj[0]);
//    }

//    //private void OnCollisionEnter(Collision collision)
//    //{
//    //    //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
//    //    //if(changer.checker.CV)
//    //    Debug.Log("Player Landed!");
//    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && isJumping)
//    //    {
//    //        PlayLandingSound();
//    //        isJumping = false;
//    //    }
//    //}
//}
