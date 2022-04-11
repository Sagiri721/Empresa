using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem
{
    public int maxEnergy;
    private int currentEnergy;

    public EnergySystem(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
        currentEnergy = maxEnergy;
    }

    public int Energy { get { return currentEnergy; } }

    public bool Use(int usedEnergy) // Isto retorna bool para que, se a ação for inválida e consumir demasiada energia, o programa não execute as animações da abilidade e essas merdas.
    {

        if (usedEnergy <= currentEnergy)
        {
            currentEnergy -= usedEnergy;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Restore(int restoredEnergy)
    {

        currentEnergy += restoredEnergy;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }

    /* Additional function just in case we ever want to implement changing the max energy by a certain value
     * changeAmount will be positive if the max energy is increased and negative if it's decreased: That will be handled before the function is called
     * ChangeMaxEnergy is called every time an effect that changes the max energy is applied
    public void ChangeMaxEnergy(int changeAmount)
    {
        maxEnergy += changeAmount;
        if (maxEnergy <= 0)
        {
            maxEnergy = 1; // This check is just to prevent these effects from leaving the entity at an invalid energy value.
        }
    } */
}
