using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerWitch : MonoBehaviour
{
    private bool isCrawling = false;
    private bool isCrouching = false;
    public bool canMove = true;

    private bool shouldCrouch => Input.GetKeyDown(crouchKey);

    [Header("Control Buttons")] //end product will use Buttons instead so the user can customize their own controls.
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode crawlKey = KeyCode.Z;
    [SerializeField] private KeyCode freeLookKey = KeyCode.C;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode stealthKey = KeyCode.LeftAlt;
    [SerializeField] private KeyCode LeftAttackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode RightAttackKey = KeyCode.Mouse1;
 
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float crawlSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 3.0f;
    [SerializeField] private bool isStrafing;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector2 currentInput;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private bool isGrounded;
    private float localCharacterForwardInput;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpStrength = 8.0f;
    [SerializeField] private float gravity = -20.0f; //This combo can be used to make space gravity
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;

    [Header("Look Parameters")]
    [SerializeField] private bool testingCam;
    [SerializeField] private bool isPaused;
    [SerializeField] private Camera playerCamera;
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    private float rotationX = 0;
    private float rotationY = 0;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float sideLookLimit = 80.0f;
    [SerializeField] private Vector3 standingCameraLocalPos = new Vector3(0, 0.8f, 0.3f);
    [SerializeField] private Vector3 crouchingCameraLocalPos = new Vector3(0.2f, 0, 0.6f);
    [SerializeField] private Vector3 crouchingCameraRotation = new Vector3(45f, 0, 0);
    [SerializeField] private Vector3 crawlingCameraLocalPos = new Vector3(0, 0.75f, 0.6f);

    WitchPlayerAnimationController animationController;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayerMask = default;

    [Header("GUI")]
    [SerializeField] private GameObject inGameMenu;
    private MenuController menuController;
    [Header("Enemy Parameters")]
    FirstPersonPlayer humanTarget;
    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponentInChildren<WitchPlayerAnimationController>();
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        inGameMenu = GameObject.Find("InGameMenuNew");
        menuController = inGameMenu.GetComponentInChildren<MenuController>();
        isPaused = false;
        inGameMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseMenu();
        HandleCursor();
        HandleMovementInput();
        if(!isPaused)HandleMouseInput();
        HandleItemInteraction();
        HandleAutoAttacks();
        HandleCrouching();
        HandleCrawling();
        ApplyPhysics();
    }

    private void HandleMovementInput()
    {
        if (canMove)
        {
            currentInput = new Vector2((isCrouching ? crouchSpeed : isCrawling ? crawlSpeed : walkSpeed) * Input.GetAxis("Horizontal"), (isCrouching ? crouchSpeed : isCrawling ? crawlSpeed : walkSpeed) * Input.GetAxis("Vertical"));
            animationController.UpdateAnimationsOnMovement(currentInput);//scales -1,0,or 1 from the Input manager and multiplies by movespeed of playear
            localCharacterForwardInput = currentInput.y;
            float moveDirectionY = moveDirection.y;
            moveDirection = ((transform.TransformDirection(Vector3.right) * currentInput.x)
             + (transform.TransformDirection(Vector3.forward) * currentInput.y)); //we are using x and z components of moveDirection for our player ground movement.
            moveDirection.y = moveDirectionY;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void HandleMouseInput()
    {
        //playerAnimator.SetFloat("MouseInputForTurn", Input.GetAxis("Mouse X") * lookSpeedX);
        //Debug.Log(Input.GetAxis("Mouse X")* lookSpeedX);
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationY += Input.GetAxis("Mouse X") * lookSpeedX; //what we link to animator to turn feet

        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, upperLookLimit); //these limits will be replaced with reference to the rig constraints.
        //rotationY = Mathf.Clamp(rotationY, -sideLookLimit, sideLookLimit); //can use this for a checker to when to use feet or I can move feet with every look
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        //playerCamera.transform.localRotation = Quaternion.Euler(0,rotationY, 0); 
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
    private void HandleCrouching()
    {

        if (shouldCrouch)
        {
            isCrouching = !isCrouching;
            isCrawling = false;
        }
        animationController.UpdateAnimationsOnCrouch(isCrouching);
        //if (Input.GetKeyUp(crouchKey))
        //{
        //    animationController.UpdateAnimationsOnCrouch(false);
        //}

    }

    private void HandleCrawling()
    {
        if (Input.GetKeyDown(crawlKey))
        {
            isCrawling = !isCrawling;
            isCrouching = false;
        }
        animationController.UpdateAnimationsOnCrawl(isCrawling);
    }
    private void ApplyPhysics()
    {
        isGrounded = characterController.isGrounded;
        //UpdateFallAnimation();
        if (!characterController.isGrounded)
        {
            isFalling = true;
            moveDirection.y += gravity * Time.deltaTime;
        }
    }

    private void HandleAutoAttacks()
    {
        if (Input.GetKeyDown(LeftAttackKey))
        {
            animationController.AnimatorOnStartAttack(0);
        }
        else if (Input.GetKeyDown(RightAttackKey))
        {
            animationController.AnimatorOnStartAttack(1);
        }
    }

    private void HandleItemInteraction()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayerMask))
        {
            //var item = hit.collider.GetComponent<GroundItem>();
            var item = hit.collider.GetComponent<GroundItem>();
            animationController.interactionItem = item;
            if (item && !item.inHand)
            {
                animationController.interactionObj = hit.collider.gameObject;
                Debug.Log(hit.collider.gameObject);
                HandleItemInteractInput();
            }
        }
    }
    private void HandleItemInteractInput()
    {
        if (Input.GetKeyDown(interactKey)) //isPickingUp
        {
            //Destroy(playerRigController.currentInteractionObj.GetComponent<Rigidbody>());
            //Destroy(playerRigController.currentInteractionObj.GetComponent<SphereCollider>());

            //rightHandIKTarget.transform.position = currentInteractionObj.transform.position;
            //playerRigController.currentInteractionObj = currentInteractionObj;
            animationController.AnimatorOnStartMovingItem();
            //playerAnimator.ResetTrigger("GrabItem");

        }
        if (Input.GetKeyUp(interactKey))
        {
            animationController.AnimatorOnDoneMoving();
        }
    }

    public void HandlePauseMenu()
    {
        isPaused = menuController.gameIsPaused;
        if (Input.GetKeyDown(pauseKey))
        {
            if (!isPaused)
            {
                inGameMenu.SetActive(true);
                menuController.TogglePause();
            }
            else
            {
                inGameMenu.SetActive(false);
                menuController.TogglePause();
            }
        }
    }
    private void HandleCursor()
    {
        if (menuController.gameIsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
