using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TriggerDoorController : MonoBehaviour
{
     private Animator doorAnim;
    //[SerializeField] private bool openTrigger = false;
    //[SerializeField] private bool closeTrigger = false;
   public bool doorOpened = false;
   public bool doorInRange = false;
   public bool doorLocked = false;

   void Start()
    {
       doorAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorInRange = true; 

        }
    }
    private void OnTriggerExit(Collider other)
    {
        doorInRange = false;
    }

     void OnGUI()
    {
        //if(doorInRange && !doorLocked && !doorOpened)
        //    GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 120, 50), "Open Door");
        //if(!doorLocked && doorOpened && doorInRange)
        //    GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 120, 50), "Close Door");
        if(doorLocked && !doorOpened)
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 120, 50), "You need a key to open this door");
    }
    void Update()
    {

        if (doorInRange && !doorLocked)
        {
            if (Input.GetKeyDown("e"))
            {
                if (!doorOpened)
                {
                    doorAnim.SetBool("isOpen", true);
                    doorOpened = true;
                   
                }

                else
                {
                    doorAnim.SetBool("isOpen", false);
                    doorOpened = false;
                }

            }
        }
    }
}
