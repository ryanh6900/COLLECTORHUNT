using UnityEngine;

public class myPlayer : MonoBehaviour
{
    public CharacterController controller;
    public myMouseLook mouseLook;
    //private Camera camera;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    const float k_Half = 0.5f;

    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    public bool isCrouching;
    public bool isGrounded;
    public bool isSprinting;

    CapsuleCollider capsule;
    float CapsuleHeight;
    Vector3 CapsuleCenter;
    Vector3 velocity;
    Vector3 forwardVelocity;

    Animator animator;

     void Start()
    {
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        //camera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -12f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * x + transform.forward* z;
        forwardVelocity = moveDir * speed;
        if (isSprinting)
            controller.Move(forwardVelocity * Time.deltaTime);
        else
            controller.Move(forwardVelocity * k_Half * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (Input.GetButtonDown("Crouch") && isGrounded)
        {
            isCrouching = !isCrouching;
        }



        animator.SetFloat("Forward", z, 0.1f, Time.deltaTime);
        animator.SetBool("Crouch", isCrouching);
        animator.SetBool("OnGround", isGrounded);


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    //void UpdateAnimator(Vector3 move)
    //{
    //    animator.SetFloat("Forward",transform.forward*z)
    //}
    //void UpdateAnimator(Vector3 move)
    //{
    //    // update the animator parameters
    //    m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
    //    //m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
    //    m_Animator.SetBool("Crouch", m_Crouching);
    //    m_Animator.SetBool("OnGround", m_IsGrounded);
    //    if (!m_IsGrounded)
    //    {
    //        m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
    //    }

    //    // calculate which leg is behind, so as to leave that leg trailing in the jump animation
    //    // (This code is reliant on the specific run cycle offset in our animations,
    //    // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
    //    float runCycle =
    //        Mathf.Repeat(
    //            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
    //    float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
    //    if (m_IsGrounded)
    //    {
    //        m_Animator.SetFloat("JumpLeg", jumpLeg);
    //    }

    //    // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
    //    // which affects the movement speed because of the root motion.
    //    if (m_IsGrounded && move.magnitude > 0)
    //    {
    //        m_Animator.speed = m_AnimSpeedMultiplier;
    //    }
    //    else
    //    {
    //        // don't use that while airborne
    //        m_Animator.speed = 1;
    //    }
    //}
    void changeCapsuleAndCameraForCrouching(bool crouch)
    {
        if(isGrounded && crouch)
        {
            if (isCrouching)
                return;
            capsule.height = capsule.height / 2f;
            capsule.center = capsule.center / 2f;
            //camera.transform.position = capsule.center;
            isCrouching = true;
        }
        else
        {
            //substituted rigidbody.position for transform.position because I am experimenting without rigid body.
            Ray crouchRay = new Ray(transform.position + Vector3.up * capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = CapsuleHeight - capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                isCrouching = true;
                return;
            }
            capsule.height = CapsuleHeight;
            capsule.center = CapsuleCenter;
            //camera.transform.position = CapsuleCenter;
            isCrouching = false;
        }
    }

    void preventStandwithLowCeiling()
    {
        if (!isCrouching)
        {
            //substituted rigidbody.position for transform.position because I am experimenting without rigid body.
            Ray crouchRay = new Ray(transform.position + Vector3.up * capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = CapsuleHeight - capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                isCrouching = true;
        }
    }

   
}
