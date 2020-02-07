using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public GameObject equipped, swordButton, saberButton, yourCash;

    public static bool saberBought = false, swordBought = false;

    void Update()
    {
        yourCash.GetComponent<Text>().text = PlayerEngine.pCash.ToString();
    }

    public void purchaseWeapon(string weapon)
    {
        if (weapon == "saber" && PlayerEngine.pCash >= 150 && StrengthSkill.strLvl >= 5)
        {
            saberBought = true;
            equipped.SetActive(true);
            saberButton.SetActive(false);
        }
        else if (weapon == "sword" && PlayerEngine.pCash >= 400 && StrengthSkill.strLvl >= 15)
        {
            swordBought = true;
            equipped.SetActive(true);
            swordButton.SetActive(false);
        }
    }

    public void ChangeEquipStatus(string weapon)
    {
        if (weapon == "spear" && !saberBought && !swordBought)
        {
            equipped.SetActive(true);
            swordButton.SetActive(false);
            saberButton.SetActive(false);
        }
        else if (weapon == "saber" && saberBought && !swordBought)
        {
            equipped.SetActive(true);
            swordButton.SetActive(false);
            saberButton.SetActive(false);
        }
        else if (weapon == "sword" && swordBought)
        {
            equipped.SetActive(true);
            swordButton.SetActive(false);
            saberButton.SetActive(false);
        }
        else
        {
            equipped.SetActive(false);

            if (weapon == "spear")
            {
                swordButton.SetActive(false);
                saberButton.SetActive(false);
            }
            else if (weapon == "saber")
            {
                if (swordBought)
                {
                    saberButton.SetActive(false);
                    swordButton.SetActive(false);
                }
                else if (!saberBought)
                {
                    saberButton.SetActive(true);
                    swordButton.SetActive(false);
                }
            }
            else if (weapon == "sword")
            {
                saberButton.SetActive(false);
                swordButton.SetActive(true);
            }
        }
    }
}
