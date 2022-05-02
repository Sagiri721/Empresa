using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public string[] weaponNames;

    //The name of all the itemss
    public string[] itemsNames = { "Pilha", "Processador", "Power supply" };
    //The image of all the items
    public Sprite[] itemsImages;

    //The difente faces of the robot
    public Sprite[] faces;

    private RectTransform hpSize, mpSize, recoil, hpEffect;

    //The robot's face on top, and the item indicator
    private Image image, itemImage;
    private int currentImage = 0;

    private float displayHP = -100;
    private bool effect = false;

    GameObject recoilIndicator;
    Text weaponIndicator, slotIndicator;

    //private Text Deaths;

    void Start()
    {
        //Get values for variables

        image = GetComponent<Image>();
        itemImage = GameObject.Find("ItemImage").GetComponent<Image>();
        slotIndicator = GameObject.Find("ItemImage").GetComponentInChildren<Text>();

        weaponIndicator = GetComponentInChildren<Text>();
        recoilIndicator = GameObject.Find("RecoilInd");
        recoil = recoilIndicator.GetComponent<RectTransform>();

        image.sprite = faces[currentImage];

        var hp = GameObject.Find("HpBar");
        var mp = GameObject.Find("MpBar");

        hpSize = hp.GetComponent<RectTransform>();
        mpSize = mp.GetComponent<RectTransform>();
        hpEffect = GameObject.Find("HpBarEffect").GetComponent<RectTransform>();

        hp.GetComponent<Image>().color = Color.green;
        mp.GetComponent<Image>().color = Color.blue;

        GameObject.Find("HpBarEffect").GetComponent<Image>().color = new Color(Color.green.r, Color.green.g - 0.5f, Color.green.b, Color.green.a);

        //Deaths = GameObject.Find("DeathCounter").GetComponent<Text>();
    }
    private void LateUpdate()
    {
        recoil.offsetMax = new Vector2(WeaponManager.Recoil, recoil.offsetMax.y);

        weaponIndicator.text = "Current weapon: " + weaponNames[(WeaponManager.CurrentWeapon)];

        if (WeaponManager.GetCurrentWeapon().tag == "Rotatable")
            recoilIndicator.SetActive(true);
        else
        {
            recoilIndicator.SetActive(false);
        }

        //-------------------------------------------------
        //HP and MP bar
        //Calculate the hp and mp percentage
        float hpPercent = (Mathf.Abs(PlayerHandler.GetMaxHealth() - PlayerHandler.GetCurrentHealth()) / (float)PlayerHandler.GetMaxHealth());
        float mpPercent = (Mathf.Abs(PlayerHandler.GetMaxEnergy() - PlayerHandler.GetCurrentEnergy()) / (float)PlayerHandler.GetMaxEnergy());

        float hpBarVal = 8 + 298 * hpPercent;
        float mpBarVal = 73 + 217 * mpPercent;

        if (displayHP == -100 || displayHP > hpBarVal)
            displayHP = hpBarVal;

        UpdateEffectValue(hpBarVal);

        hpSize.offsetMax = new Vector2(-hpBarVal, hpSize.offsetMax.y);
        mpSize.offsetMax = new Vector2(-mpBarVal, mpSize.offsetMax.y);

        hpEffect.offsetMax = new Vector2(-displayHP, hpEffect.offsetMax.y);

        //Draw the adequate face
        int minDiv = PlayerHandler.GetMaxHealth() / 3;
        int cur = PlayerHandler.GetCurrentHealth();

        if (cur >= minDiv * 2)
            SetImage(0);
        else if (cur >= minDiv)
            SetImage(1);
        else
            SetImage(2);

        if (cur <= 0)
            SetImage(3);

        //-------------------------------------------------
        //Weapon indicator
        slotIndicator.text = "Slot " + (PlayerHandler.ArrayCounter + 1);
        itemImage.sprite = itemsImages[PlayerHandler.GetCurrentItemId()];

        //Deaths.text = "Deaths: " + PlayerHandler.DeathCounter;
    }

    private void SetImage(int num)
    {
        image.sprite = faces[num];
    }

    private void UpdateEffectValue(float hpValue)
    {

        if (effect)
        {
            if (hpValue <= displayHP)
            {
                displayHP = hpValue;
                effect = false;
            }
            else
            {
                displayHP++;
            }
        }
        else
        {
            if (hpValue > displayHP)
            {
                Invoke("Effect", 0.5f);
            }
        }


    }

    private void Effect()
    {
        effect = true;
    }
}
