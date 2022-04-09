using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private int maxHealthPoints;

    HealthSystem hp;

    private void Awake()
    {
        hp = new HealthSystem(maxHealthPoints);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                hp.HurtPlayer(other.gameObject.GetComponent<Enemy>().Damage);

                break;
            case "Projectiles":
                hp.HurtPlayer(other.gameObject.GetComponent<ProjectileMovement>().Damage);
                break;
        }

    }
}
