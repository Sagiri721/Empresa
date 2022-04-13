using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int damage = 0;
    public int Damage { get { return damage; } set { damage = value; } }

    //The animator component
    Animator animator;

    //Reference to the movement script
    Movement movement;

    //The projectile if fires
    public GameObject projectile;

    //Can it fire projectiles?
    public bool canShoot;

    //The energy it consumes shooting
    public int energyConsume = 0;

    WeaponManager weaponManager;

    public float recoilSpd = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (!canShoot)
            projectile = null;

        animator = GetComponent<Animator>();
        movement = GameObject.FindWithTag("Player").GetComponent<Movement>();
        weaponManager = GameObject.FindWithTag("Player").GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.J) && canShoot && movement.IsGrounded &&
        weaponManager.Recoil >= weaponManager.maxRecoil && WeaponManager.GetCurrentWeapon().GetComponent<Weapon>() == this)
        {
            FireProjectile();
        }
    }

    public void FireProjectile()
    {
        //play the shoot animation
        animator.SetBool("isShooting", true);
        movement.spd /= 4;

        //Make the is shooting false
        Invoke("GunFire", 0.7f);
    }

    public void GunFire()
    {
        if (weaponManager.Recoil < weaponManager.maxRecoil)
        {
            movement.spd = movement.normalSpd;
            return;
        }

        animator.SetBool("isShooting", false);

        //return to normal speed
        movement.spd = movement.normalSpd;

        //Apply knockback after the gun fire
        //Calculate the sin of the angle
        float sin = Mathf.Sqrt(1 - Mathf.Pow(transform.rotation.z, 2));

        int flipX = GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        //Big calculations B)
        movement.Knockback(35, new Vector3(sin * flipX, transform.rotation.z * flipX, 0).normalized);

        //Create the actual projectile
        ProjectileMovement pm = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileMovement>(); ;
        pm.dir = flipX == 1 ? transform.right : -transform.right;

        weaponManager.Recoil = 0;

        //Remove energy
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>().energy.Use(energyConsume);
        GameObject.FindWithTag("HUD").GetComponent<HudController>().UpdateHudValues();
    }
}
