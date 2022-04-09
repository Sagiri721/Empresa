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
    public double GetDistanceFromObject(GameObject obj)
    {
        Vector2 dist = obj.transform.position - transform.position;

        return dist.sqrMagnitude;
    }

    public void FireProjectile(int angle)
    {

        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
    }

    public void FireProjectileToPlayer()
    {
        Transform t = GameObject.FindWithTag("Player").GetComponent<Transform>();

        //get the direction of the other object from current object
        Vector3 dir = t.position - transform.position;
        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 90 * ((Mathf.Sign(dir.x) == -1) ? 1 : 0) + angle));
    }

    private void Start()
    {
        InvokeRepeating("FireProjectileToPlayer", 0f, 2f);
    }
}
