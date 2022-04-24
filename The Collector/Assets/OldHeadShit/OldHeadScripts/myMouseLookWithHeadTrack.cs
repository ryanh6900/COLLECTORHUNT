using UnityEngine;

//
// Player head tracking and mouse look script
// 
public class myMouseLookWithHeadTrack : MonoBehaviour
{
    // Ideas from: https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
    // and https://github.com/bytefeast/camera-control/blob/main/MouseLook.cs
    public Animator animator;
    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;

    [Range(0.1f, 100f)] [SerializeField] float sensitivity = 50f;

    [Tooltip("Limit vertical camera rotation to prevent the flipping when rotation goes above 90")]
    [Range(0f, 90f)] [SerializeField] float xRotationLimit = 90;
    [Range(0f, 90f)] [SerializeField] float yRotationLimit = 75;
    
    // Are string literials in function calls really allocated on the heap?
    // if so, using string constants for calling GetAxis would be more efficient
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";

    void Start()
    {
        yRotationLimit = 75;
    }

    void Update()
    {
       
       
        xRotation += Input.GetAxis(xAxis) * sensitivity * Time.deltaTime;
        float horizontalLook = xRotation;
        animator.SetFloat("HorizontalLook", Input.GetAxis(xAxis));
        yRotation += Input.GetAxis(yAxis) * sensitivity * Time.deltaTime;

        yRotation = Mathf.Clamp(yRotation, -yRotationLimit, yRotationLimit);
        
        //var xQuat = Quaternion.AngleAxis(xRotation, Vector3.up);
        //var yQuat = Quaternion.AngleAxis(yRotation, Vector3.left);
        var xQuat = Quaternion.Euler(0f, xRotation, 0f);
        var yQuat = Quaternion.Euler(-yRotation, 0f, 0f);
        //float rotationForAnim = 0f;
        //rotationForAnim += xRotation;
        //animator.SetFloat("HorizontalLook", rotationForAnim / xRotationLimit);
        // Rotate player based on X mouse movement
        playerBody.localRotation = xQuat;
        //Debug.Log("Rotation: " + xRotation);
        // Tilt up and down based on Y mouse movement
        transform.localRotation = yQuat;
        //Use values in imported animation names and match with if statements that change the playerBody.local Rotation;
    }
}
