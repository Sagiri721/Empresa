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
    public EnergySystem energy;

    //Counter to keep track of the column containing the selected item
    int arrayCounter = 0;

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

        if (Input.anyKey) //Para eliminar estes checks todos em frames onde nada está a ser pressionado
        {
            //Hurt your self
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                hp.Hurt(5);
                hud.UpdateHudValues();
                return;
            }

            //Item use when pressing L
            if (Input.GetKeyDown(KeyCode.L) && itemInventory[1, arrayCounter] > 0)
            {
                switch (itemInventory[0, arrayCounter])
                {
                    case 1: //código da função do item 1

                        energy.Restore(300);
                        hud.UpdateHudValues();
                        break;
                    case 2: //código da função do item 2
                        break;
                }

                itemInventory[1, arrayCounter]--;
                return;
            }

            //Scroll the item selection wheel up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (arrayCounter == itemInventory.GetLength(1) - 1)
                    arrayCounter = 0;
                else
                    arrayCounter++;

                hud.ChangeItemImage(itemInventory[0, arrayCounter], arrayCounter);
                return;
            }

            //Scroll the item selection wheel down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (arrayCounter == 0)
                    arrayCounter = itemInventory.GetLength(1) - 1;
                else
                    arrayCounter--;

                hud.ChangeItemImage(itemInventory[0, arrayCounter], arrayCounter);
                return;
            }
        }
    }

    //Inventory functions
    public void AddToInventory(Items items, int quantity)
    {

        //Check if already on inventory
        for (int i = 0; i < itemInventory.GetLength(1); i++)
        {
            if (itemInventory[0, i] == items.id)
            {
                itemInventory[1, i] += quantity;
                return;
            }
        }

        //If not just add to last position if it exists
        int pos = GetLastPos();

        if (pos != -1)
        {
            itemInventory[0, pos] = items.id;
            itemInventory[1, pos] = quantity;
        }

        hud.ChangeItemImage(itemInventory[0, arrayCounter], arrayCounter);
    }

    private int GetLastPos()
    {
        for (int i = 0; i < itemInventory.GetLength(1); i++)
        {
            if (itemInventory[1, i] == 0)
                return i;
        }

        //Inventory is full
        return -1;
    }


    #region 
    //Ignore this
    public int GetMaxHealth()
    { return hp.maxHealth; }
    public int GetMaxEnergy()
    { return energy.maxEnergy; }

    public int GetCurrentHealth()
    { return hp.Hp; }
    public int GetCurrentEnergy()
    { return energy.Energy; }
    #endregion
}
