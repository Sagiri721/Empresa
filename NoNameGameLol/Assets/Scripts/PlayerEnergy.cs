using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField]
    private int maxEnergyPoints;

    EnergySystem energy;

    private void Awake()
    {
        energy = new EnergySystem(maxEnergyPoints);
    }

    // todo: criar evento de on item/ability use retirar/restorar energia
}
