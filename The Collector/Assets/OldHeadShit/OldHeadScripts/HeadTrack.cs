using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrack : MonoBehaviour
{
    public GameObject head;
    public Vector3 offset;

    //public Vector3 rotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = head.transform.position + offset; 
        transform.rotation = head.transform.rotation;
       //transform.Rotate(rotation);
    }
}
