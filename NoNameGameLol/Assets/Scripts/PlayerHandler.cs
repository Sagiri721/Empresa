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

    //Do we have items in the first place?
    private bool itemExists = false;

    //Counter to keep track of the column containing the selected item
    int arrayCounter = 0;

    public int ArrayCounter { get { return arrayCounter; } }

    //Matriz placeholder de 2 linhas e colunas ilimitadas que serve como inventário de item.
    private int[,] itemInventory = new int[2, 0];

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

                break;
            case "Projectiles":
                hp.Hurt(other.gameObject.GetComponent<ProjectileMovement>().Damage);

                break;
        }

    }

    private void Update()
    {
        if (Input.anyKey && itemExists) //Para eliminar estes checks todos em frames onde nada está a ser pressionado
        {
            //Hurt your self
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                hp.Hurt(5);
                return;
            }

            if (Input.anyKey && itemExists) //Para eliminar estes checks todos em frames onde nada está a ser pressionado e todos os frames em que não temos items
            {
                //Item use when pressing L
                if (Input.GetKeyDown(KeyCode.L))
                {
                    switch (itemInventory[0, arrayCounter])
                    {
                        case 1: //código da função do item 1

                            energy.Restore(300);
                            break;
                        case 2: //código da função do item 2
                            break;
                    }

                    RemoveFromInventory(arrayCounter);
                    return;
                }

                //Scroll the item selection wheel up
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (arrayCounter == itemInventory.GetLength(1) - 1)
                        arrayCounter = 0;
                    else
                        arrayCounter++;
                    return;
                }

                //Scroll the item selection wheel down
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (arrayCounter == 0)
                        arrayCounter = itemInventory.GetLength(1) - 1;
                    else
                        arrayCounter--;
                    return;
                }
            }
        }
    }

    //Inventory functions
    public void AddToInventory(Items items, int quantity)
    {
        itemExists = true;

        //Check if already on inventory
        for (int i = 0; i < itemInventory.GetLength(1); i++)
        {
            if (itemInventory[0, i] == items.id)
            {
                itemInventory[1, i] += quantity;
                return;
            }
        }

        //If not resize the array to include the new id and the assigned quantity
        itemInventory = ResizeArray(itemInventory, itemInventory.GetLength(1) + 1);
        itemInventory[0, itemInventory.GetLength(1) - 1] = items.id;
        itemInventory[1, itemInventory.GetLength(1) - 1] = items.quantity;
    }

    public void RemoveFromInventory(int n)
    {
        itemInventory[1, n]--;
        if (itemInventory[1, n] == 0)
        {
            if (arrayCounter == itemInventory.GetLength(1) - 1)
            {
                itemInventory = ResizeArray(itemInventory, itemInventory.GetLength(1) - 1);
                arrayCounter--;
            }
            else
            {
                for (int i = arrayCounter; i < itemInventory.GetLength(1) - 1; i++)
                {
                    itemInventory[0, i] = itemInventory[0, i + 1];
                    itemInventory[1, i] = itemInventory[1, i + 1];
                    itemInventory = ResizeArray(itemInventory, itemInventory.GetLength(1) - 1);
                }
            }
            if (itemInventory.GetLength(1) == 0)
            {
                itemExists = false;
                arrayCounter = 0;
            }
        }
    }

    //Resize the inventory array. A new row number isn't necessary since there will always be 2 rows.
    private int[,] ResizeArray(int[,] original, int newColumnNumber)
    {
        int[,] newArray = new int[2, newColumnNumber];
        if (newColumnNumber < original.GetLength(1))
        {
            for (int i = 0; i < 2; i++)
            {
                for (int i1 = 0; i1 < newColumnNumber; i1++)
                {
                    newArray[i, i1] = original[i, i1];
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                for (int i1 = 0; i1 < original.GetLength(1); i1++)
                {
                    newArray[i, i1] = original[i, i1];
                }
            }
        }
        return newArray;
    }

    public int GetCurrentItemId()
    {

        try
        {
            return itemInventory[0, arrayCounter];
        }
        catch (System.Exception ignored)
        {
            return 0;
        }
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