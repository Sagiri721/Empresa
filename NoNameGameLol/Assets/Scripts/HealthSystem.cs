using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    private int playerMaxHealth;
    private int playerCurrentHealth;

    public HealthSystem(int maxHp)
    {
        playerMaxHealth = maxHp;
        playerCurrentHealth = playerMaxHealth;
    }

    public int Hp { get { return playerCurrentHealth; } }

    // HurtPlayer is called every time a collision with an object with the "enemy" tag occurs.
    public void HurtPlayer(int damageReceived)
    {
        playerCurrentHealth -= damageReceived;

        if (playerCurrentHealth <= 0)
        {
            //gameObject.SetActive(false);
            //Por razões fixes é melhor não fazer isto porque referencias = null be like :)

            //Dies :(
        }
    }

    // HealPlayer is called every time an item with healing properties is used
    public void HealPlayer(int damageHealed)
    {
        playerCurrentHealth += damageHealed;

        if (playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
    }

    /* Additional function just in case we ever want to implement changing the max health by a certain value
     * changeAmount will be positive if the max health is increased and negative if it's decreased: That will be handled before the function is called
     * ChangeMaxHealth is called every time an effect that changes the max health is applied
    public void ChangeMaxHealth(int changeAmount)
    {
        playerMaxHealth += changeAmount;
        if (playerMaxHealth <= 0)
        {
            playerMaxHealth = 1; // This check is just to prevent these effects from killing the player or leaving them at an invalid health.
        }
    } */
}
