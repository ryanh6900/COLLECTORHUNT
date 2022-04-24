using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerStamina
{ 
    public int currentStamina;
    public bool hasRegenerated;

    public PlayerStamina(int startingStamina)
    {
        currentStamina = startingStamina;
    }
    public int GetStamina()
    {
        return currentStamina;
    }
}
