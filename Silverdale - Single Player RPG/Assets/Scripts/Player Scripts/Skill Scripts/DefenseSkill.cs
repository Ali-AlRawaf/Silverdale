using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSkill : MonoBehaviour
{
    public static int defLvl = 1;
    public static float defExp;
    public static bool defAtt = false;
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

        defExp = PlayerEngine.FindNextLevelExpRequirement(defLvl);
    }

    void Update()
    {
        if (defAtt && Vector3.Distance(transform.position, ePos.position) <= 0.5f && ePos != null)
        {
            aiPath.target = transform;
            if (countDown >= coolDown)
            {               
                LookAtEachother();
                PlayerAnimationController.anim.SetBool("Attack", true);
                DefAttackEnemy();
                countDown = 0;
            }
        }
        else
            PlayerAnimationController.anim.SetBool("Attack", false);

        if (countDown <= coolDown)
            countDown += Time.deltaTime;
    }

    //Gets called when an enemy is going to be attacked with a strength hit.
    public void DefAttackEnemy()
    {
        if (ePos == null)
            return;

        bool boss = false;
        float aff;

        if (ePos.tag != "Ezekiel")
        {
            eEng = ePos.GetComponent<EnemyEngine>();
            if (eEng.eWeak == 1)
                aff = 75;
            else if (eEng.ePower == 1)
                aff = 45;
            else
                aff = 65;
        }
        else
        {
            bEng = ePos.GetComponent<Ezekiel>();
            boss = true;
            if (bEng.weak == 1)
                aff = 75;
            else if (bEng.pow == 1)
                aff = 45;
            else
                aff = 65;
        }

        float hitChance = aff * (defLvl + PlayerEngine.pWeaponLvl * 2) / eDef;
        float hit = Random.Range(0, 100);
        if (hit < hitChance)
        {
            float maxHit = Mathf.Ceil(0.5f * defLvl + PlayerEngine.pArmourLvl);
            int defRoll = Mathf.RoundToInt(Random.Range(1, maxHit));
            DefenseGiveExp(defRoll);
            PlayerHealth.GiveHealthExp(defRoll);
            DamageController.CreateDamageText(defRoll.ToString(), ePos.transform);

            if (boss)
                bEng.health -= defRoll;
            else
            {
                if (defRoll > eEng.eHealth)
                    defRoll = Mathf.RoundToInt(eEng.eHealth);
                eEng.eHealth -= defRoll;
            }

            Debug.Log("DEF - We hit: " + (defRoll));
        }
        else
        {
            Debug.Log("DEF - P:0");
            DamageController.CreateDamageText("0", ePos.transform);
        }

        if (!boss)
            eEng.Aggro = true;
    }

    //Gives exp depending on dmg done
    void DefenseGiveExp(int exp)
    {
        if (PlayerEngine.MaxLevel(defLvl))
            return;

        defExp += exp * 4;
        Debug.Log("Defense Exp: " + defExp);

        if (defExp >= PlayerEngine.FindNextLevelExpRequirement(defLvl + 1))
            DefenseLevel();
    }
    
    //Levels Defense
    void DefenseLevel()
    {
        defLvl++;
        PlayerEngine.theDM.dLines = new string[1];
        PlayerEngine.theDM.dLines[0] = "You have leveled up defense. You are now at level: " + defLvl.ToString() + ".";
        PlayerEngine.theDM.currentLine = 0;
        PlayerEngine.theDM.ShowDialog();
    }

    public void LookAtEachother()
    {
        xDelta = ePos.position.x - transform.position.x;
        yDelta = ePos.position.y - transform.position.y;

        if (xDelta > 0 && yDelta > 0)
        {
            if (xDelta > yDelta)
                PlayerAnimationController.anim.SetInteger("Direction", 3);
            else
                PlayerAnimationController.anim.SetInteger("Direction", 1);

            Debug.Log("Quadrant 1");
        }
        else if (xDelta < 0 && yDelta > 0)
        {
            if (-1 * xDelta > yDelta)
                PlayerAnimationController.anim.SetInteger("Direction", 4);
            else
                PlayerAnimationController.anim.SetInteger("Direction", 1);

            Debug.Log("Quadrant 2");
        }
        else if (xDelta < 0 && yDelta < 0)
        {
            if (xDelta < yDelta)
                PlayerAnimationController.anim.SetInteger("Direction", 4);
            else
                PlayerAnimationController.anim.SetInteger("Direction", 2);

            Debug.Log("Quadrant 3");
        }
        else if (xDelta > 0 && yDelta < 0)
        {
            if (xDelta > yDelta * -1)
                PlayerAnimationController.anim.SetInteger("Direction", 3);
            else
                PlayerAnimationController.anim.SetInteger("Direction", 2);
            Debug.Log("Quadrant 4");
        }

        PlayerAnimationController.anim.SetInteger("Direction", 0);
    }
}