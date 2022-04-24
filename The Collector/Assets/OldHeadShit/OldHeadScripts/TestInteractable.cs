using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public override void OnInteract()
    {
        print("INTERACTED WITH " + gameObject.name);
    }
    public override void OnHandlePickupItem()
    {
        //inventorySystem.Add(referenceItem);
        Destroy(gameObject);
    }
    public override void OnFocus()
    {
       print("LOOKING AT " + gameObject.name);
       //referenceItem = gameObject.
    }
    
    public override void OnLoseFocus()
    {
        //throw new System.NotImplementedException();
        print("STOPPED LOOKING AT " + gameObject.name);
    }
}
