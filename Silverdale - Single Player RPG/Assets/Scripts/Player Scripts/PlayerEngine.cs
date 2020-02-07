using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
[System.Serializable]
public class PlayerEngine : MonoBehaviour
{
    public static int pCash = 0;
    public static float pSpeed = 1; 
    public static int pArmourLvl = 1;
    public static int pWeaponLvl = 1;
    public static DialogManager theDM;

    public string startingPointName;

    AIPath aiPath;
    
    int levelReq;

    static bool exists;

    void Awake()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        theDM = FindObjectOfType<DialogManager>();
        aiPath = GetComponent<AIPath>();
        aiPath.speed = pSpeed;
    }

    void Update()
    {
        aiPath.speed = pSpeed;
    }

    public void SendLevelReq(int lvlReq)
    {
        levelReq = lvlReq;
    }

    public void PurchaseWeapon(int cost)
    {
        if (cost > pCash)
        {
            theDM.dLines = new string[1];
            theDM.dLines[0] = "You do not have enough ducats.";
            theDM.currentLine = 0;
            theDM.ShowDialog();
            return;
        }

        if (levelReq > StrengthSkill.strLvl)
        {
            theDM.dLines = new string[1];
            theDM.dLines[0] = "Strength Lv. " + levelReq + " required.";
            theDM.currentLine = 0;
            theDM.ShowDialog();
            return;
        }

        pCash -= cost;
        if (levelReq == 10)
            pWeaponLvl = 2;
        else
            pWeaponLvl = 3;

        ReskinPlayer.Reskin(pArmourLvl, pWeaponLvl);
    }

    public void PurchaseArmor(int cost)
    {
        if (cost > pCash)
        {
            theDM.dLines = new string[1];
            theDM.dLines[0] = "You do not have enough ducats.";
            theDM.currentLine = 0;
            theDM.ShowDialog();
            return;
        }

        if (levelReq > DefenseSkill.defLvl)
        {
            theDM.dLines = new string[1];
            theDM.dLines[0] = "Defense Lv. " + levelReq + " required.";
            theDM.currentLine = 0;
            theDM.ShowDialog();
            return;
        }

        pCash -= cost;
        if (levelReq == 5)
            pArmourLvl = 2;
        else if (levelReq == 10)
            pArmourLvl = 3;
        else
            pArmourLvl = 4;

        ReskinPlayer.Reskin(pArmourLvl, pWeaponLvl);
    }

    public static bool MaxLevel(int level)
    {
        if (level == 25)
            return true;
        return false;
    }

    public static float FindNextLevelExpRequirement(int level)
    {
        float num = 0;
        for (float i = 0; i < level - 1; i++)
        {
            num += Mathf.Floor(i + 300 * Mathf.Pow(2, i / 7));
        }
        num = Mathf.Floor(num / 4);
        return num;
    }

    public void RespawnPlayer()
    {
        ClickTarget click = FindObjectOfType<ClickTarget>();
        aiPath.target = transform;
        SceneManager.LoadScene("Village");        
        //uiControl.whichScene("Village");        
        //uiControl.StartFade(false);        

        if (pCash >= 100)
            pCash -= 100;
        else
            pCash = 0;        
    }
}