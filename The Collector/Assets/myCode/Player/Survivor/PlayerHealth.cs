using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerHealth 
{
    //private int maxHealth = 100;
    public int currentHealth;

    public PlayerHealth(int startingHealth)
    {
        currentHealth = startingHealth;
    }

    public int GetHealth()
    {
        return currentHealth; 
    }

    public void Damage(int damageAmount)
    {
       currentHealth -= damageAmount;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount; 
    }
    //void Update()
    //{
    //    if (health < 0)
    //    {
    //        health = 0;
    //        isDead = true;
    //    }
    //    if (health > maxHealth)
    //        health = maxHealth;
    //}
}
