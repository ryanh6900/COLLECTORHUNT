using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerHealth playerHealthData;
    public PlayerStamina playerStaminaData;
    public PlayerSpeed playerSpeedData;
    public int numCollected;
    public float[] pos;

    public PlayerData(FirstPersonPlayer player)
    {

        //health = player.health;
        //speed = player.speed;
        //stamina = player.stamina;
        playerHealthData = player.healthOfPlayer;
        playerSpeedData = player.speedOfPlayer;
        playerStaminaData = player.staminaOfPlayer;
        //numCollected = player.numCollected;
        pos = new float[3];
        pos[0] = player.transform.position.z;
        pos[1] = player.transform.position.y;
        pos[2] = player.transform.position.z;
    }
}
