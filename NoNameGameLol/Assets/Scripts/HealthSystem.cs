using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public int maxHealth;
    private int currentHealth;

    public HealthSystem(int maxHp)
    {
        maxHealth = maxHp;
        currentHealth = maxHealth;
    }

    public int Hp { get { return currentHealth; } }

    // Hurt is called every time a collision with an object that deals damage occurs.
    public void Hurt(int damageReceived)
    {
        currentHealth -= damageReceived;

        if (currentHealth <= 0)
        {
            //gameObject.SetActive(false);
            //Por razões fixes é melhor não fazer isto porque referencias = null be like :)

            //Dies :(
        }
    }

    // HealPlayer is called every time an item with healing properties is used
    public void Heal(int damageHealed)
    {
        currentHealth += damageHealed;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    /* Additional function just in case we ever want to implement changing the max health by a certain value
     * changeAmount will be positive if the max health is increased and negative if it's decreased: That will be handled before the function is called
     * ChangeMaxHealth is called every time an effect that changes the max health is applied
    public void ChangeMaxHealth(int changeAmount)
    {
        maxHealth += changeAmount;
        if (maxHealth <= 0)
        {
            maxHealth = 1; // This check is just to prevent these effects from killing the player or leaving them at an invalid health.
        }
    } */
}
