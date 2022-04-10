using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int damage = 0;
    public int Damage { get { return damage; } set { damage = value; } }

    //The projectile if fires
    public GameObject projectile;

    //Can it fire projectiles?
    public bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        if (!canShoot)
            projectile = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.J) && canShoot)
        {
            FireProjectile(transform.eulerAngles.z);
        }
    }

    public void FireProjectile(float angle)
    {
        //Para arranjar
        //Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
    }
}
