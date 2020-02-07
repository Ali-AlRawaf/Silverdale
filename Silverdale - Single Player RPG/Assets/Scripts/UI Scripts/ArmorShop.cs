using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorShop : MonoBehaviour
{
    public GameObject equipped, leather, iron, gold;

    public static bool leatherBought, ironBought, goldBought;

    public void purchaseArmor(string armor)
    {
        if(armor == "leather" && PlayerEngine.pCash >= 300 && DefenseSkill.defLvl >= 5)
        {
            leatherBought = true;
            equipped.SetActive(true);
            leather.SetActive(false);
        }
        else if (armor == "iron" && PlayerEngine.pCash >= 600 && DefenseSkill.defLvl >= 10)
        {
            ironBought = true;
            equipped.SetActive(true);
            iron.SetActive(false);
        }
        else if (armor == "gold" && PlayerEngine.pCash >= 1000 && DefenseSkill.defLvl >= 20)
        {
            goldBought = true;
            equipped.SetActive(true);
            gold.SetActive(false);
        }
    }

    public void changeEquipStatus(string armor)
    {
        if ((armor == "leather" && leatherBought && !ironBought && !goldBought) || (armor == "iron" && ironBought && !goldBought) || (armor == "gold" && goldBought))
        {
            equipped.SetActive(true);
            leather.SetActive(false);
            iron.SetActive(false);
            gold.SetActive(false);
        }
        else
        {
            equipped.SetActive(false);

            if (armor == "leather")
            {
                if (goldBought || ironBought)
                {
                    leather.SetActive(false);
                    iron.SetActive(false);
                    gold.SetActive(false);
                }
                else if(!leatherBought)
                {
                    leather.SetActive(true);
                    iron.SetActive(false);
                    gold.SetActive(false);
                }
            }
            else if (armor == "iron")
            {
                if (goldBought)
                {
                    leather.SetActive(false);
                    iron.SetActive(false);
                    gold.SetActive(false);
                }
                else if (!ironBought)
                {
                    leather.SetActive(false);
                    iron.SetActive(true);
                    gold.SetActive(false);
                }
            }
            else if (armor == "gold")
            {
                leather.SetActive(false);
                iron.SetActive(false);
                gold.SetActive(true);
            }
        }
    }
}
