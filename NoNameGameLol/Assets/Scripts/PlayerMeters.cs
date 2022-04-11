using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeters : MonoBehaviour
{
    //Pontos máximos de hp que se pode ter
    [SerializeField]
    private int maxHealthPoints;

    //Referencia à classe de operações com hp
    HealthSystem hp;

    //Pontos máximos de energia que se pode ter
    [SerializeField]
    private int maxEnergyPoints;

    //Referencia à classe de operações com energia
    EnergySystem energy;

    private void Awake()
    {
        hp = new HealthSystem(maxHealthPoints);
        energy = new EnergySystem(maxEnergyPoints);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                hp.Hurt(other.gameObject.GetComponent<Enemy>().Damage);

                break;
            case "Projectiles":
                hp.Hurt(other.gameObject.GetComponent<ProjectileMovement>().Damage);
                break;
        }

    }

    // todo: criar evento de on item/ability use retirar/restorar energia
}
