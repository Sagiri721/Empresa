using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
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

    //Matriz placeholder de 2 linhas e colunas ilimitadas que serve como inventário de item. (A parte de 5 colunas é inteiramente arbitrária)
    //No futuro quando já tivermos concordado em todos os items que vão existir, isto vai ser substituído por um .json, mas funciona por agora.
    private int[,] itemInventory = new int[2, 5]; 

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
        //Counter to keep track of the column containing the selected item
        int arrayCounter = 0;
        if (Input.anyKey) //Para eliminar estes checks todos em frames onde nada está a ser pressionado
        {
            //Hurt your self
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                hp.Hurt(5);
                hud.UpdateHudValues();
                return;
            }

            //Item use when pressing Z
            if (Input.GetKeyDown(KeyCode.Z) && itemInventory[1, arrayCounter] > 0)
            {
                switch (itemInventory[0, arrayCounter])
                {
                    case 1: //código da função do item 1
                        break;
                    case 2: //código da função do item 2
                        break;
                    //etc.
                }
                itemInventory[1, arrayCounter]--;
                return;
            }

            //Scroll the item selection wheel up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (arrayCounter == itemInventory.GetLength(0) - 1)
                {
                    arrayCounter = 0;
                }
                else
                {
                    arrayCounter++;
                }
                return;
            }

            //Scroll the item selection wheel down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (arrayCounter == 0)
                {
                    arrayCounter = itemInventory.GetLength(0) - 1;
                }
                else
                {
                    arrayCounter--;
                }
                return;
            }
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
}
