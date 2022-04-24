using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject pickUpObj;
    public GameObject bodyType;
    public GameObject parent;

    Vector3 objOriginalPos;
    Quaternion objOriginalRot;

    void Start()
    {
        if(pickUpObj != null)
        {
            objOriginalPos = pickUpObj.transform.position;
            objOriginalRot = pickUpObj.transform.rotation;
        }
    }

    
    public void OnPickUp()
    {
        Vector3 distance = pickUpObj.transform.position - bodyType.transform.position;
        float magnitude =  distance.magnitude;
        if (magnitude <= 0.2f)
        {
            pickUpObj.transform.SetParent(parent.transform);
            pickUpObj.transform.localPosition = Vector3.zero;
        }
    }

    public void OnDrop()
    {
        if(pickUpObj != null)
        {
            pickUpObj.transform.parent = null;
            pickUpObj.transform.position = objOriginalPos;
            pickUpObj.transform.rotation = objOriginalRot;
        }
    }
}
