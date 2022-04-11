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

    //Referencia à classe de HUD para se poder fazer update à HUD
    HudController hud;

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

    private void Start()
    {

        hud = GameObject.Find("RobotFace").GetComponent<HudController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                hp.Hurt(other.gameObject.GetComponent<Enemy>().Damage);
                hud.UpdateHudValues();

                break;
            case "Projectiles":
                hp.Hurt(other.gameObject.GetComponent<ProjectileMovement>().Damage);
                hud.UpdateHudValues();

                break;
        }

    }

    private void Update()
    {

        //Hurt your self
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            hp.Hurt(5);
            hud.UpdateHudValues();
        }
    }

    public int GetMaxHealth()
    { return hp.maxHealth; }
    public int GetMaxEnergy()
    { return energy.maxEnergy; }

    public int GetCurrentHealth()
    { return hp.Hp; }
    public int GetCurrentEnergy()
    { return energy.Energy; }

    // todo: criar evento de on item/ability use retirar/restorar energia
}
