using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MagicSkill : MonoBehaviour
{
    [HideInInspector] public EnemyEngine eEng;
    public static int magLvl = 1;
    public static float magExp;
    public static int magAttType = 0;
    public static int eDef;
    public static Transform magTar, magChild;

    public GameObject waterButton;
    public GameObject earthButton;
    public GameObject fireButton;
    public GameObject healButton;

    Ezekiel bEng;
    float countDown;
    float coolDown = 3f;

    void Start()
    {
        magExp = PlayerEngine.FindNextLevelExpRequirement(magLvl);
        countDown = coolDown;

        if (magLvl >= 5)
        {
            waterButton.GetComponent<Button>().enabled = true;
            Color c = new Color(255, 255, 255, 255);
            waterButton.GetComponent<Image>().color = c;
            waterButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
            waterButton.GetComponentInChildren<Text>().text = "";
        }

        if (magLvl >= 15)
        {
            earthButton.GetComponent<Button>().enabled = true;
            Color c = new Color(255, 255, 255, 255);
            earthButton.GetComponent<Image>().color = c;
            earthButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
            earthButton.GetComponentInChildren<Text>().text = "";
        }

        if (magLvl >= 20)
        {
            fireButton.GetComponent<Button>().enabled = true;
            Color c = new Color(255, 255, 255, 255);
            fireButton.GetComponent<Image>().color = c;
            fireButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
            fireButton.GetComponentInChildren<Text>().text = "";
        }

        if (magLvl >= 25)
        {
            healButton.GetComponent<Button>().enabled = true;
            Color c = new Color(255, 255, 255, 255);
            healButton.GetComponent<Image>().color = c;
            healButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
            healButton.GetComponentInChildren<Text>().text = "";
        }
    }

    public void MagicSpellSelected(int magSel)
    {
        switch (magSel)
        {
            case 0:
                PlayerEngine.theDM.dLines = new string[1];
                PlayerEngine.theDM.dLines[0] = "Wind spell is now selected.";
                PlayerEngine.theDM.currentLine = 0;
                PlayerEngine.theDM.ShowDialog();
                break;
            case 1:
                PlayerEngine.theDM.dLines = new string[1];
                PlayerEngine.theDM.dLines[0] = "Water spell is now selected.";
                PlayerEngine.theDM.currentLine = 0;
                PlayerEngine.theDM.ShowDialog();
                break;
            case 2:
                PlayerEngine.theDM.dLines = new string[1];
                PlayerEngine.theDM.dLines[0] = "Earth spell is now selected.";
                PlayerEngine.theDM.currentLine = 0;
                PlayerEngine.theDM.ShowDialog();
                break;
            case 3:
                PlayerEngine.theDM.dLines = new string[1];
                PlayerEngine.theDM.dLines[0] = "Fire spell is now selected.";
                PlayerEngine.theDM.currentLine = 0;
                PlayerEngine.theDM.ShowDialog();
                break;
        }
    }

    void Update()
    {       
        if (countDown < coolDown)
            countDown += Time.deltaTime;
    }

    public void SetMagicAttackLevel(int lvl)
    {
        magAttType = lvl;
    }

    public void Attack()
    {
        if (countDown >= coolDown && magTar != null)
        {
            float aff;
            bool boss = false;

            if (magTar.tag != "Ezekiel")
            {
                eEng = magTar.GetComponent<EnemyEngine>();
                if (eEng.eWeak == 0)
                    aff = 75;
                else if (eEng.ePower == 0)
                    aff = 45;
                else
                    aff = 65;
            }
            else
            {
                bEng = magTar.GetComponent<Ezekiel>();
                boss = true;
                if (bEng.weak == 0)
                    aff = 75;
                else if (bEng.pow == 0)
                    aff = 45;
                else
                    aff = 65;
            }

            float hitChance = aff * (magLvl + magAttType) / eDef;
            float hit = Random.Range(0, 100);
            if (hit < hitChance)
            {
                float maxHit = Mathf.Ceil(0.5f * (magLvl + magAttType));
                int magRoll = Mathf.RoundToInt(Random.Range(1, maxHit));
                MagicGiveExp(magRoll);
                PlayerHealth.GiveHealthExp(magRoll);
                magChild = magTar.GetChild(magAttType + 1);

                if (boss)
                {
                    if (bEng.health - magRoll > 0)
                        StartCoroutine(MagicAnimTimer(magChild));
                    else
                    {
                        magChild.parent = null;
                        StartCoroutine(MagicAnimTimer(magChild));
                    }
                    bEng.health -= magRoll;
                }
                else
                {
                    if (magRoll > eEng.eHealth)
                        magRoll = Mathf.RoundToInt(eEng.eHealth);

                    if (eEng.eHealth - magRoll > 0)
                        StartCoroutine(MagicAnimTimer(magChild));
                    else
                    {
                        magChild.parent = null;
                        StartCoroutine(MagicAnimTimer(magChild));
                    }
                    eEng.eHealth -= magRoll;
                }

                DamageController.CreateDamageText(magRoll.ToString(), magTar.transform);
                Debug.Log("MAG - We hit: " + (magRoll));                              
            }
            else
            {
                Debug.Log("MAG - P:0");
                DamageController.CreateDamageText("0", magTar.transform);
            }
            if (!boss)
                eEng.Aggro = true;
            countDown = 0;
        }
    }

    IEnumerator MagicAnimTimer(Transform magChild)
    {
            magChild.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            magChild.gameObject.SetActive(false);
    }

    //Gives exp depending on spell cast
    void MagicGiveExp(int exp)
    {
        if (PlayerEngine.MaxLevel(magLvl))
            return;

        magExp += exp * 4;
        Debug.Log("Magic Exp: " + magExp);
        if (magExp >= PlayerEngine.FindNextLevelExpRequirement(magLvl + 1))
           MagicLevel();
    }

    //Levels magic
    void MagicLevel()
    {        
        magLvl++;

        PlayerEngine.theDM.dLines = new string[1];
        PlayerEngine.theDM.dLines[0] = "You have leveled up magic. You are now at level: " + magLvl.ToString() + ".";
        PlayerEngine.theDM.currentLine = 0;
        PlayerEngine.theDM.ShowDialog();

        switch (magLvl)
        {
            case 5:
                waterButton.GetComponent<Button>().enabled = true;
                Color c = new Color(255, 255, 255, 255);
                waterButton.GetComponent<Image>().color = c;
                waterButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
                waterButton.GetComponentInChildren<Text>().text = "";
                break;
            case 15:
                earthButton.GetComponent<Button>().enabled = true;
                Color o = new Color(255, 255, 255, 255);
                earthButton.GetComponent<Image>().color = o;
                earthButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
                earthButton.GetComponentInChildren<Text>().text = "";
                break;
            case 20:
                fireButton.GetComponent<Button>().enabled = true;
                Color u = new Color(255, 255, 255, 255);
                fireButton.GetComponent<Image>().color = u;
                fireButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
                fireButton.GetComponentInChildren<Text>().text = "";
                break;
            case 25:
                healButton.GetComponent<Button>().enabled = true;
                Color r = new Color(255, 255, 255, 255);
                healButton.GetComponent<Image>().color = r;
                healButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
                healButton.GetComponentInChildren<Text>().text = "";
                break;
        }
    }
}