using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSpeed
{
    public int currentSpeed;

    public PlayerSpeed(int startingSpeed)
    {
        currentSpeed = startingSpeed;
    }
    public int GetSpeed()
    {
        return currentSpeed;
    }
    public void Slow(int slowAmount)
    {
        currentSpeed -= slowAmount;
    }

    public void Restore(int amount)
    {
        currentSpeed += amount;
    }
    //void Update()
    //{
    //    if (speed < 0)
    //    {
    //        speed = 0;
    //        passedOut = true;
            
    //    }
    //    if (speed > maxSpeed)
    //        speed= maxSpeed;
    //}
}
