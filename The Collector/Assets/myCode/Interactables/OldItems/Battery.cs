using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityStandardAssets.Characters.FirstPerson;
public class Battery : MonoBehaviour
{

    public bool batteryInRange;
    //bool buttonActivated;
    public bool powerAdded;
    public AudioClip batterySound;
    public AudioSource source;
    private static int batteryPower = 50;
    public GUISkin guiSkin;
    public bool hasPlayed;

    private void Start()
    {
        batteryInRange = false;
        powerAdded = false;
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
            batteryInRange = true;
    }

    private void OnTriggerExit(Collider c)
    {
        batteryInRange = false;
    }

    private void OnGUI()
    {
        if (batteryInRange)
        {
            GUI.skin = guiSkin;
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 120, 50), "Pick up battery");
        }
        if (powerAdded)
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 120, 50), "50% battery life added");
    }

    void Update()
    {
        if (batteryInRange)
        {
            if (Input.GetKeyDown("e"))
            {
                if (!hasPlayed)
                {
                    //loadBattery();
                    hasPlayed = true;
                    powerAdded = true;
                }
            }
        }
    }

}
