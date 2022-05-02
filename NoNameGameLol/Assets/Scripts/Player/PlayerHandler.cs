using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHandler : MonoBehaviour
{
    //Posição onde o player respawn quando morre
    Vector3 respawnPos = Vector3.zero;

    //Pontos máximos de hp que se pode ter
    [SerializeField]
    private int maxHealthPoints;

    //Referencia à classe de operações com hp
    public static HealthSystem hp;

    //Pontos máximos de energia que se pode ter
    [SerializeField]
    private int maxEnergyPoints;

    //Referencia à classe de operações com energia
    public static EnergySystem energy;

    //Do we have items in the first place?
    private bool itemExists = false;

    //Counter to keep track of the column containing the selected item
    static int arrayCounter = 0;

    public static int ArrayCounter { get { return arrayCounter; } }

    //Global damage that spikes give to players
    public int spikeDamage = 10;

    //Matriz placeholder de 2 linhas e colunas ilimitadas que serve como inventário de item.
    private static int[,] itemInventory = new int[2, 0];

    public int DeathCounter = 0;

    private AudioSource audioSource;
    public AudioClip audioHurt, audioHeal, audioItemUse, audioDeath, audioChangeItem, audioItemGet;

    private void Awake()
    {
        hp = new HealthSystem(maxHealthPoints);
        energy = new EnergySystem(maxEnergyPoints);
    }

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        respawnPos = transform.position;

        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hp.Hp > 1)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    PlayerHandler.hp.Hurt(other.gameObject.GetComponent<Enemy>().Damage);
                    audioSource.PlayOneShot(audioHurt);

                    Debug.Log("Ouch?");

                    break;
                case "Projectiles":
                    PlayerHandler.hp.Hurt(other.gameObject.GetComponent<ProjectileMovement>().Damage);
                    audioSource.PlayOneShot(audioHurt);

                    break;
                case "Spike":
                    GetComponent<Movement>().Knockback(100, other.gameObject.transform.rotation.z == 1 ? Vector2.up : Vector2.down);
                    PlayerHandler.hp.Hurt(spikeDamage);
                    audioSource.PlayOneShot(audioHurt);

                    break;
            }
        }


    }

    public void Respawn()
    {
        GetComponent<Movement>().enabled = true;
        transform.position = respawnPos;
        GetComponent<Animator>().SetBool("isDead", false);
        WeaponManager.GetCurrentWeapon().GetComponent<Animator>().SetBool("isDead", GetComponent<Animator>().GetBool("isDead"));
        hp.Hp = hp.maxHealth;

        audioSource.PlayOneShot(audioHeal);
        WeaponManager.GetCurrentWeapon().SetActive(true);
        DeathCounter++;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F5))
        {
            transform.position = respawnPos;
            hp.Hp = hp.maxHealth;
            audioSource.PlayOneShot(audioHeal);
        }

        if (hp.Hp <= 0)
        {
            //Animação de morrer
            GetComponent<Movement>().enabled = false;
            Invoke("Respawn", 1.3f);

            WeaponManager.GetCurrentWeapon().SetActive(false);

            audioSource.PlayOneShot(audioDeath);

            GetComponent<Animator>().SetBool("isDead", true);
            WeaponManager.GetCurrentWeapon().GetComponent<Animator>().SetBool("isDead", GetComponent<Animator>().GetBool("isDead"));
            hp.Hp = 1;
        }

        if (Input.anyKey) //Para eliminar estes checks todos em frames onde nada está a ser pressionado
        {

            if (itemExists) //Para eliminar estes checks todos os frames em que não temos items
            {
                //Item use when pressing L
                if (Input.GetKeyDown(KeyBinding.itemUseKey))
                {
                    switch (itemInventory[0, arrayCounter])
                    {
                        case 1: //código da função do item 1 (pilha)

                            //Restora energia se nenhuma for desperdiçada
                            if (!(energy.Energy + 300 > energy.maxEnergy))
                                energy.Restore(300);
                            else
                                return;
                            break;

                        case 2: //código da função do item 2 (processador)
                            Movement m = GetComponent<Movement>();
                            m.spd = 2 * m.spd;
                            m.Invoke("SpeedDecrease", 5);
                            break;
                    }
                    audioSource.PlayOneShot(audioItemUse);
                    RemoveFromInventory(arrayCounter);
                    return;
                }

                //Scroll the item selection wheel up
                if (Input.GetKeyDown(KeyBinding.itemScrollUpKey))
                {
                    if (arrayCounter == itemInventory.GetLength(1) - 1)
                        arrayCounter = 0;
                    else
                    {
                        arrayCounter++;
                    }
                    audioSource.PlayOneShot(audioChangeItem);

                    return;
                }

                //Scroll the item selection wheel down
                if (Input.GetKeyDown(KeyBinding.itemScrollDownKey))
                {
                    if (arrayCounter == 0)
                        arrayCounter = itemInventory.GetLength(1) - 1;
                    else
                    {
                        arrayCounter--;
                    }
                    audioSource.PlayOneShot(audioChangeItem);
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

        audioSource.PlayOneShot(audioItemGet);
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

    public static int GetCurrentItemId()
    {

        try
        {
            return itemInventory[0, arrayCounter];
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    #region 
    //Ignore this
    public static int GetMaxHealth()
    { return hp.maxHealth; }
    public static int GetMaxEnergy()
    { return energy.maxEnergy; }

    public static int GetCurrentHealth()
    { return hp.Hp; }
    public static int GetCurrentEnergy()
    { return energy.Energy; }
    #endregion
}
