using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningSkill : MonoBehaviour
{    
    public static int mineLvl = 1;
    public static float mineExp;
    public static bool clickedToMine = false;
    public static bool startMining = false;
    public static Transform oreTransform;

    public static string[] oreType = { "Bronze", "Silver", "Gold", "Sapphire", "Ruby" };

    public GameObject pickaxe;

    int lvlReq;
    int oreTypeIndex;
    float coolDown = 2;
    float countDown;
    
    Vector3 orePos;

    ClickToMine oreScript;

    void Start()
    {
        mineExp = PlayerEngine.FindNextLevelExpRequirement(mineLvl);
        countDown = coolDown;
    }

    void Update()
    {
        if (startMining && countDown >= coolDown && Vector3.Distance(transform.position, orePos) <= 1f)
            MineOre();

        if (countDown <= coolDown)
            countDown += Time.deltaTime;          
    }


    public void GetOre(Transform ore, int _lvlReq)
    {        
        lvlReq = _lvlReq;
        orePos = ore.position;
        oreScript = ore.GetComponent<ClickToMine>();

        if (lvlReq > mineLvl)
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "Low level. You need level " + lvlReq + " to mine this ore.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();            
            return;
        }

        for (int i = 0; i < oreType.Length; i++)
            if (ore.tag == oreType[i])
            {
                oreTypeIndex = i;
                break;
            }

        oreTransform = ore;
        startMining = true;
    }

    void MineOre()
    {
        pickaxe.GetComponent<SpriteRenderer>().enabled = true;
        float mineChance = 15 * mineLvl / lvlReq;
        float chance = Random.Range(0, 100);
        if (chance < mineChance)
        {
            pickaxe.GetComponent<SpriteRenderer>().enabled = false;
            startMining = false;
            InventoryManager.OnGetItem(oreTypeIndex);

            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You have mined some " + oreType[oreTypeIndex] + ".";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();

            Debug.Log("You have mined some " + oreType[oreTypeIndex]);
            MiningGiveExp(oreScript.exp);
            StartCoroutine(oreScript.RespawnOre());
        }
        else
        {
            pickaxe.GetComponent<SpriteRenderer>().enabled = true;
            pickaxe.transform.position = new Vector2(oreTransform.position.x - 0.44f, oreTransform.position.y + 0.21f);
            Debug.Log("You swung your pickaxe but failed to get any ore.");
        }

       countDown = 0;
    }

    void MiningGiveExp(float exp)
    {
        if (PlayerEngine.MaxLevel(mineLvl))
            return;

        mineExp += exp;
        Debug.Log("Mining Exp: " + mineExp);
        if (mineExp >= PlayerEngine.FindNextLevelExpRequirement(mineLvl + 1))
            MiningLevel();
    }

    //Levels magic
    void MiningLevel()
    {        
        mineLvl++;
        PlayerEngine.theDM.dLines = new string[1];
        PlayerEngine.theDM.dLines[0] = "You have leveled up mining! You are now at level " + mineLvl + ".";
        PlayerEngine.theDM.currentLine = 0;
        PlayerEngine.theDM.ShowDialog();
    }
}
