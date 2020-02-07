using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int pHealth = 10;
    public static int pHealthLevel = 10;
    public static float pHealthExp;

    public GameObject healSpellButton;
    public GameObject health;

    //public GameObject[] hearts = new GameObject[25];
    //public static GameObject[] heart = new GameObject[25];

    DialogManager theDM;
    float coolDown = 25f;
    float countDown;

    void Awake()
    {
        //heart = hearts;

        //for (int i = 0; i < pHealth; i++)
        //    heart[i].SetActive(true);

        pHealthExp = PlayerEngine.FindNextLevelExpRequirement(pHealthLevel);
        theDM = FindObjectOfType<DialogManager>();
        StartCoroutine(HealBackHealth());
        countDown = coolDown;
    }

	void Update ()
    {
        health.GetComponent<Text>().text = pHealth.ToString() + " / " + pHealthLevel.ToString();

        if (pHealth <= 0)
        {            
            AIPath aiPath = GetComponent<AIPath>();
            aiPath.canMove = false;

            StrengthSkill strSkill = GetComponent<StrengthSkill>();
            StrengthSkill.clickOnEnemy = false;
            StrengthSkill.strAtt = false;
            strSkill.ePos = null;
            
            DefenseSkill defSkill = GetComponent<DefenseSkill>();
            DefenseSkill.clickOnEnemy = false;
            DefenseSkill.defAtt = false;
            defSkill.ePos = null;

            MagicSkill.magTar = null;

            pHealth = Mathf.RoundToInt(0.5f * pHealthLevel);

            transform.position = new Vector3(-0.087f, -6.31f, 0f);

            aiPath.target = transform;

            aiPath.canMove = false;

            //for (int i = 0; i < pHealth; i++)
            //    heart[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

            theDM.dLines = new string[1];
            theDM.dLines[0] = "You have died and respawned back at the village. You have lost 100 coins.";
            theDM.currentLine = 0;
            theDM.ShowDialog();            

            StartCoroutine(WaitForDeath());
            PlayerEngine pEng = GetComponent<PlayerEngine>();
            pEng.RespawnPlayer();
        }

        if (countDown <= coolDown)
        {
            countDown += Time.deltaTime;
            if (healSpellButton != null)
                healSpellButton.GetComponentInChildren<Text>().text = (25 - countDown).ToString("0.00");
        }   
        else
            if (healSpellButton != null)
                healSpellButton.GetComponentInChildren<Text>().text = "Ready";

    }

    IEnumerator WaitForDeath()
    {
        PlayerAnimationController.anim.SetBool("Dead", true);
        yield return new WaitForSeconds(1.5f);
        PlayerAnimationController.anim.SetBool("Dead", false);
        AIPath aiPath = GetComponent<AIPath>();
        aiPath.canMove = true;
    }

    IEnumerator HealBackHealth()
    {        
        if (pHealth < pHealthLevel)
        {
            //print(pHealth);
            //hearts[pHealth - 1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            pHealth++;
        }
        yield return new WaitForSeconds(20f);
        StartCoroutine(HealBackHealth());
    }

    public void Heal()
    {
        if (countDown >= coolDown)
        {
            pHealth = pHealthLevel;

            //for (int i = 0; i < pHealth; i++)
            //    heart[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You are now at full health.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();            
            countDown = 0;
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You must wait for the spell to cool down!";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }
    }

    public void LoseHealth(int healthLost)
    {
        //int num = 0;
        //print(healthLost);

        //if (pHealth - healthLost >= 0)
        //    num = pHealth - healthLost;
        //else num = 0;

        //for (int i = pHealth - 1; i >= num; i--)
        //{
        //    print(i);
        //    heart[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 255);
        //}

        if (healthLost > pHealth)
            pHealth = 0;
        else
            pHealth -= healthLost;
    }

    //Gives exp depending on dmg done
    public static void GiveHealthExp(int exp)
    {
        pHealthExp += exp * 4;
        Debug.Log("Health Exp: " + pHealthExp);

        if (pHealthExp >= PlayerEngine.FindNextLevelExpRequirement(pHealthLevel + 1))
        {
            pHealthLevel++;
            pHealth = pHealthLevel;

            //heart[pHealth - 1].SetActive(true);

            //for (int i = 0; i < pHealth; i++)
            //    heart[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You have leveled up health! You are now at level " + pHealthLevel + ".";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }            
    }
}
