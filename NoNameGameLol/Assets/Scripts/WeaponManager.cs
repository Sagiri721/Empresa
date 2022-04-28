using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    //-----------------------------------
    //Declarations
    //The inventory in where the weapons we can swap between are stored
    [HideInInspector]
    public static GameObject[] weaponInventory = new GameObject[3];

    //The weapon we are currently holdings
    private static int currentWeapon = 0;

    //True if we can swap weapon right now, false blocks it
    private bool canSwap = true;

    public bool CanSwap { get { return canSwap; } set { canSwap = value; } }

    public int CurrentWeapon { get { return currentWeapon; } }

    //The speed with wich you can rotate the weapon around
    public int rotSpeed = 1;

    public int maxRecoil = 135;
    private float recoil = 135;

    public float Recoil { get { return recoil; } set { recoil = value; } }

    //-----------------------------------
    //Methods
    public static GameObject GetCurrentWeapon()
    {
        return weaponInventory[currentWeapon];
    }

    //Adds a weapon to the inventory
    public static void AddWeapon(GameObject weapon)
    {
        int pos = GetLastPos();

        //If it's not weapon or the inventory is full, can't add
        if (pos == -1 || !weapon.tag.Equals("Weapon"))
            return;
        else
        {
            weaponInventory[pos] = weapon;
        }
    }



    public static bool IsCurrentWeaponRotatable()
    {
        return weaponInventory[currentWeapon].tag == "Rotatable";
    }

    //Gets the first unoccupied position in vector
    private static int GetLastPos()
    {
        for (int i = 0; i < weaponInventory.Length; i++)
            if (weaponInventory[i] == null)
                return i;

        //Full inventory :pog:
        return -1;
    }

    //Returns true if there is a weapon with a certain name in the inventory
    public static bool HasWeapon(string name)
    {
        foreach (GameObject o in weaponInventory)
            if (o.name.Equals(name))
                return true;

        return false;
    }

    #region
    // Start is called before the first frame update
    private void Awake()
    {
        weaponInventory[0] = GameObject.Find("arms");
        weaponInventory[1] = GameObject.Find("keyboardgun");
    }

    // Update is called once per frame
    void Update()
    {

        if (recoil < maxRecoil)
            recoil += GetCurrentWeapon().GetComponent<Weapon>().recoilSpd * Time.deltaTime;

        if (!(weaponInventory[currentWeapon].tag == "Rotatable") || !weaponInventory[currentWeapon].GetComponent<Animator>().GetBool("isShooting"))
        {

            //Sistema de mudança de arma através de numeros
            //Para automatizar
            //If key 0/1/2/3/... is pressed change to the adequate weapon
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponInventory[currentWeapon].GetComponent<SpriteRenderer>().enabled = false;
                weaponInventory[0].GetComponent<SpriteRenderer>().enabled = true;

                if (currentWeapon != 0)
                    GetComponent<Movement>().ChangeWeapon(weaponInventory[0]);

                currentWeapon = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weaponInventory[currentWeapon].GetComponent<SpriteRenderer>().enabled = false;
                weaponInventory[1].GetComponent<SpriteRenderer>().enabled = true;

                if (currentWeapon != 1)
                    GetComponent<Movement>().ChangeWeapon(weaponInventory[1]);

                currentWeapon = 1;
            }

            int inv = GetComponent<SpriteRenderer>().flipX ? -1 : 1;

            //Sistema de rotação da arma
            //Rotate the weapon given the input
            if (GetCurrentWeapon().tag.Equals("Rotatable"))
            {

                float angle = weaponInventory[currentWeapon].transform.rotation.z;
                if (inv == 1)
                {
                    if (Input.GetKey(KeyCode.I) && angle < 0.5f) //Rotate upwards
                    {
                        weaponInventory[currentWeapon].transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * rotSpeed), Space.Self);
                    }
                    else if (Input.GetKey(KeyCode.K) && angle > -0.5f) //Rotate downwards
                    {
                        weaponInventory[currentWeapon].transform.Rotate(new Vector3(0, 0, -1 * Time.deltaTime * rotSpeed), Space.Self);
                    }
                }
                else
                {

                    if (Input.GetKey(KeyCode.I) && angle > -0.5f) //Rotate upwards
                    {
                        weaponInventory[currentWeapon].transform.Rotate(new Vector3(0, 0, -1 * Time.deltaTime * rotSpeed), Space.Self);
                    }
                    else if (Input.GetKey(KeyCode.K) && angle < 0.5f) //Rotate downwards
                    {
                        weaponInventory[currentWeapon].transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * rotSpeed), Space.Self);
                    }
                }
            }
        }

        #endregion
    }
}
