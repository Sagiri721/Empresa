using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public string[] weaponNames;

    //The difente faces of the robot
    public Sprite[] faces;

    private RectTransform hpSize, mpSize, recoil;

    //The robot's face on top
    private Image image;
    private int currentImage = 0;

    GameObject recoilIndicator;
    Text weaponIndicator;

    //Reference to the players hp, energy, and all of that
    private PlayerHandler meters;
    //Reference to the players weapon inventory and such
    private WeaponManager weaponManager;

    void Start()
    {
        //Get values for variables
        var player = GameObject.FindGameObjectWithTag("Player");

        meters = player.GetComponent<PlayerHandler>();
        weaponManager = player.GetComponent<WeaponManager>();

        image = GetComponent<Image>();
        weaponIndicator = GetComponentInChildren<Text>();
        recoilIndicator = GameObject.Find("RecoilInd");
        recoil = recoilIndicator.GetComponent<RectTransform>();

        image.sprite = faces[currentImage];

        var hp = GameObject.Find("HpBar");
        var mp = GameObject.Find("MpBar");

        hpSize = hp.GetComponent<RectTransform>();
        mpSize = mp.GetComponent<RectTransform>();

        hp.GetComponent<Image>().color = Color.green;
        mp.GetComponent<Image>().color = Color.blue;

        UpdateHudValues();
        UpdateWeaponUI();
    }

    private void Update()
    {
        recoil.offsetMax = new Vector2(weaponManager.Recoil, recoil.offsetMax.y);
    }

    public void UpdateWeaponUI()
    {
        weaponIndicator.text = "Current weapon: " + weaponNames[weaponManager.CurrentWeapon];
        if (WeaponManager.GetCurrentWeapon().tag == "Rotatable")
            recoilIndicator.SetActive(true);
        else
        {
            recoilIndicator.SetActive(false);
        }
    }

    public void UpdateHudValues()
    {

        //Calculate the hp and mp percentage
        float hpPercent = (Mathf.Abs(meters.GetMaxHealth() - meters.GetCurrentHealth()) / (float)meters.GetMaxHealth());
        float mpPercent = (Mathf.Abs(meters.GetMaxEnergy() - meters.GetCurrentEnergy()) / (float)meters.GetMaxEnergy());

        float hpBarVal = 8 + 298 * hpPercent;
        float mpBarVal = 73 + 217 * mpPercent;

        hpSize.offsetMax = new Vector2(-hpBarVal, hpSize.offsetMax.y);
        mpSize.offsetMax = new Vector2(-mpBarVal, mpSize.offsetMax.y);

        //Draw the adequate face
        int minDiv = meters.GetMaxHealth() / 3;
        int cur = meters.GetCurrentHealth();

        if (cur >= minDiv * 2)
            SetImage(0);
        else if (cur >= minDiv)
            SetImage(1);
        else
            SetImage(2);

        if (cur <= 0)
            SetImage(3);
    }

    private void SetImage(int num)
    {
        image.sprite = faces[num];
    }
}
