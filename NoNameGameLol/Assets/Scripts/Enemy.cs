using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //That damage that it inflicts to the player and such
    [SerializeField]
    private int damage, maxHealthPoints;
    //Reference to the hp system
    HealthSystem hp;
    //If it let's out projectiles, reference too them
    public GameObject projectile;

    public int Damage { get { return damage; } set { damage = value; } }

    private void Awake()
    {
        hp = new HealthSystem(maxHealthPoints);
    }

    //Returns the distance from a certain object
    public float GetDistanceFromObject(GameObject obj)
    {
        Vector2 dist = obj.transform.position - transform.position;

        return dist.sqrMagnitude;
    }

    public void FireProjectile(int angle)
    {

        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void Start()
    {
        //InvokeRepeating("FireProjectileToPlayer", 0f, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == 8)
            hp.Hurt(other.gameObject.GetComponent<ProjectileMovement>().Damage);
    }
}
