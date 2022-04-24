using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    public bool kitInRange;
    public bool healthAdded;
    private static int kitHealth = 50;
    public AudioClip kitSound;
    public AudioSource source;
    public bool hasPlayed;
    FirstPersonPlayer player;

    void Start()
    {
        kitInRange = false;
        healthAdded = false;
        player = GetComponent<FirstPersonPlayer>();
    }

   
    void Update()
    {
        if (kitInRange)
        {
            if (Input.GetKeyDown("e"))
            {
                if (!hasPlayed)
                {
                    useKit();
                    hasPlayed = true;
                    healthAdded = true;
                }
            }
        }
    }

    public void useKit()
    {
        AudioSource.PlayClipAtPoint(kitSound,transform.position);
        player.healthOfPlayer.Heal(kitHealth);
    }
}
