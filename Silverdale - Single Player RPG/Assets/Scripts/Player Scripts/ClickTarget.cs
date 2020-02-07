using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTarget : MonoBehaviour
{
    public bool guiClick;
    public GameObject player;
    StrengthSkill strSkill;
    DefenseSkill defSkill;
    AIPath aiPath;

    static bool exists;

    void Start ()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        transform.position = player.transform.position;
        aiPath = player.GetComponent<AIPath>();
        strSkill = player.GetComponent<StrengthSkill>();
        defSkill = player.GetComponent<DefenseSkill>();
	}
	
    public void GUIClick(bool onGUI)
    {
        guiClick = onGUI;

        if (CombatOption.isOffense)
        {
            if (strSkill.ePos != null || defSkill.ePos != null)
            {
                StrengthSkill.strAtt = true;
                StrengthSkill.eDef = DefenseSkill.eDef;
                DefenseSkill.defAtt = false;
                if (strSkill.ePos != null)
                    return;
                else
                {
                    strSkill.ePos = defSkill.ePos;
                    defSkill.ePos = null;
                }                
                aiPath.target = strSkill.ePos;    
            }            
        }
        else
        {
            if (strSkill.ePos != null || defSkill.ePos != null)
            {
                DefenseSkill.defAtt = true;
                DefenseSkill.eDef = StrengthSkill.eDef;
                StrengthSkill.strAtt = false;
                if (defSkill.ePos != null)
                    return;
                else
                {
                    defSkill.ePos = strSkill.ePos;
                    strSkill.ePos = null;
                }               
                aiPath.target = defSkill.ePos;
            }
        }
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (guiClick || MiningSkill.clickedToMine)
            {
                MiningSkill.clickedToMine = false;                
                return;
            }

            MiningSkill.startMining = false;           

            if (player.GetComponent<MiningSkill>().pickaxe != null)
            {
                SpriteRenderer mine = player.GetComponent<MiningSkill>().pickaxe.GetComponent<SpriteRenderer>();
                mine.enabled = false;
            }                

            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0;

            if (StrengthSkill.clickOnEnemy || DefenseSkill.clickOnEnemy) //If player clicked on enemy
            {
                if (StrengthSkill.clickOnEnemy)
                    aiPath.target = strSkill.ePos;
                else
                    aiPath.target = defSkill.ePos;

                //Sets click on enemy to false
                StrengthSkill.clickOnEnemy = false;
                DefenseSkill.clickOnEnemy = false;
            }
            else
            {
                aiPath.speed = 1f;
                transform.position = p;
                //Sets attacking to false
                StrengthSkill.strAtt = false;
                DefenseSkill.defAtt = false;                
                aiPath.target = transform;
            }            
        }
	}
}
