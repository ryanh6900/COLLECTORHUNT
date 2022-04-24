using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class myUserControl : MonoBehaviour
{
    public myPlayer myPlayer; //Reference to playercharacter
    //public Camera cam;
    public Transform Cam; //reference to the transform of the FPCamera 
    public Vector3 camForward; //current forward direction of camera
    public Vector3 Move;
    public bool Jump;
    void Start()
    {
        myPlayer = GetComponent<myPlayer>();
        //cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!Jump)
            Jump = Input.GetButtonDown("Jump");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        camForward = Vector3.Scale(Cam.forward, new Vector3(3, 0, 1)).normalized;
        Move = v * camForward + h * Cam.right;
        if (Input.GetKey(KeyCode.C))
        {
            myPlayer.isCrouching = true;
        }
           
        if (Input.GetKey(KeyCode.LeftShift))
        {
            myPlayer.isSprinting = true;
        }
            
       
        

            //Jump = CrossPlatformInputManager.GetButtonDown("Jump");
    }
    private void FixedUpdate()
    {
       
    }
}
