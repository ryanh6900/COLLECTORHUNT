using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myMouseLook : MonoBehaviour
{
    public float mouseSens = 100.0f;
    public Transform playerBody;
    public Transform lookAt;
    private float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X")*mouseSens*Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -100f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        //We are literally rotating the player body. Thats why we can just hold w and use mouse to turn.
        playerBody.Rotate(Vector3.up * MouseX);
        xRotation -= MouseY;
    }
}
