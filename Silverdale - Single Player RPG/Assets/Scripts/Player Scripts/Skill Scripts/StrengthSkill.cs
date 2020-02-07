using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthSkill : MonoBehaviour
{
    public static int strLvl = 1;
    public static float strExp;
    public static bool strAtt = false;
    public static bool clickOnEnemy = false;
    public static int eDef;

    [HideInInspector]
    public Transform ePos;
    [HideInInspector]
    public float countDown;
    [HideInInspector]
    public float coolDown; //Changes on the weapon
    [HideInInspector]
    public float xDelta, yDelta;

    PlayerHealth Health;
    AIPath aiPath;
    EnemyEngine eEng;
    Ezekiel bEng;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        coolDown = 2f;
        countDown = coolDown;

        strExp = PlayerEngine.FindNextLevelExpRequirement(strLvl);
    }

    void Update()
    {
        if (strAtt && Vector3.Distance(transform.position, ePos.position) <= 0.5f && ePos != null)
        {
            aiPath.target = transform;
            if (countDown >= coolDown)
            {
                LookAtEachother();
                PlayerAnimationController.anim.SetBool("strAttack", true);             
                StrAttackEnemy();
                countDown = 0;
            }
        }
        else
            PlayerAnimationController.anim.SetBool("strAttack", false);

        if (countDown <= coolDown)
            countDown += Time.deltaTime;
    }

    //Gets called when an enemy is going to be attacked with a strength hit.
    public void StrAttackEnemy()
    {
        if (ePos == null)
            return;

        bool boss = false;
        float aff;

        if (ePos.tag != "Ezekiel")
        {
            eEng = ePos.GetComponent<EnemyEngine>();
            if (eEng.eWeak == 0)
                aff = 75;
            else if (eEng.ePower == 0)
                aff = 45;
            else
                aff = 65;
        }            
        else
        {
            bEng = ePos.GetComponent<Ezekiel>();
            boss = true;
            if (bEng.weak == 0)
                aff = 75;
            else if (bEng.pow == 0)
                aff = 45;
            else
                aff = 65;
        }
                      
        float hitChance = aff * (strLvl + PlayerEngine.pWeaponLvl * 2) / eDef;
        float hit = Random.Range(0, 100);
        if (hit < hitChance)
        {
            float maxHit = Mathf.Ceil(0.5f * strLvl + PlayerEngine.pWeaponLvl);
            int strRoll = Mathf.RoundToInt(Random.Range(1, maxHit));           
            StrengthGiveExp(strRoll);
            PlayerHealth.GiveHealthExp(strRoll);
            DamageController.CreateDamageText(strRoll.ToString(), ePos.transform);

            if (boss)
                bEng.health -= strRoll;
            else
            {
                if (strRoll > eEng.eHealth)
                    strRoll = Mathf.RoundToInt(eEng.eHealth);
                eEng.eHealth -= strRoll;
            }                

            Debug.Log("STR - We hit: " + (strRoll));
        }
        else
        {
            Debug.Log("STR - P:0");
            DamageController.CreateDamageText("0", ePos.transform);
        }

        if (!boss)
            eEng.Aggro = true;
    }

    //Gives exp depending on dmg done
    void StrengthGiveExp(int exp)
    {
        if (PlayerEngine.MaxLevel(strLvl))
            return;

        strExp += exp * 4;
        Debug.Log("Strength Exp: " + strExp);

        if (strExp >= PlayerEngine.FindNextLevelExpRequirement(strLvl + 1))
            StrengthLevel();
    }

    //Levels strength
    void StrengthLevel()
    {
        strLvl++;
        PlayerEngine.theDM.dLines = new string[1];
        PlayerEngine.theDM.dLines[0] = "You have leveled up strength! You are now at level " + strLvl + ".";
        PlayerEngine.theDM.currentLine = 0;
        PlayerEngine.theDM.ShowDialog();
    }

    public void LookAtEachother()
    {
        int dir = 0, eDir = 0;
        xDelta = ePos.position.x - transform.position.x;
        yDelta = ePos.position.y - transform.position.y;

        if (xDelta > 0 && yDelta > 0)
        {
            if (xDelta > yDelta)
                dir = 3;
            else
                dir = 1;
        }
        else if (xDelta < 0 && yDelta > 0)
        {
            if (-1 * xDelta > yDelta)
                dir = 4;
            else
                dir = 1;
        }
        else if (xDelta < 0 && yDelta < 0)
        {
            if (xDelta < yDelta)
                dir = 4;
            else
                dir = 2;
        }
        else if (xDelta > 0 && yDelta < 0)
        {
            if (xDelta > yDelta * -1)
                dir = 3;
            else
                dir = 2;
        }

        if (dir == 1) eDir = 2;
        else if (dir == 2) eDir = 1;
        else if (dir == 3) eDir = 4;
        else if (dir == 4) eDir = 3;

        PlayerAnimationController.anim.SetInteger("Direction", dir);
        ePos.GetChild(0).GetComponent<Animator>().SetInteger("Direction", eDir);

        PlayerAnimationController.anim.SetInteger("Direction", 0);
        ePos.GetChild(0).GetComponent<Animator>().SetInteger("Direction", 0);        
    }
}