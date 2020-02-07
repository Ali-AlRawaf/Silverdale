using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 0-4 ores, 5-9 ingots, 10-14 quest items

public class InventoryManager : MonoBehaviour
{
    public static int[] amountOf = new int[24];
    public static int counter = 0;

    public GameObject[] amounts = new GameObject[24];
    public GameObject[] slots = new GameObject[24];
    public Sprite[] icons = new Sprite[15];

    public static Sprite[] itemIcons = new Sprite[15];
    public static GameObject[] amountDisplay = new GameObject[24];
    public static GameObject[] invSlots = new GameObject[24];   
    public static int[] currentInvPos = new int[24];
    public static bool[] slotOpen = new bool[24];

    public GameObject[] oreCount = new GameObject[5];
    public GameObject[] ingotCount = new GameObject[5];
    public GameObject yourCoins;
     
    void Start()
    {
        counter = itemIcons.Length;
        amountDisplay = amounts;
        invSlots = slots;
        itemIcons = icons;
    }

    private void Update()
    {
        for(int i = 0; i < 24; i++)
        {
            if (invSlots[i].activeSelf == true)
                slotOpen[i] = false;
            else
                slotOpen[i] = true;
        }

        for (int i = 0; i < 24; i++)
            if (amountOf[i] > 0)
            {
                invSlots[currentInvPos[i]].SetActive(true);
                invSlots[currentInvPos[i]].GetComponent<Image>().sprite = itemIcons[i];
                amountDisplay[currentInvPos[i]].GetComponent<Text>().text = amountOf[i].ToString();
            }

        yourCoins.GetComponent<Text>().text = PlayerEngine.pCash.ToString();

        for (int i = 0; i < 5; i++)
            oreCount[i].GetComponent<Text>().text = amountOf[i].ToString();

        for (int i = 0; i < 5; i++)
            ingotCount[i].GetComponent<Text>().text = amountOf[i + 5].ToString(); 
    }

    public void OreCheck(int ore)
    {
        if (amountOf[ore] >= 3)
        {
            OnLoseItem(ore, 3);
            OnGetItem(ore + 5); //Get Ingot for 3 ores
            CraftingSkill.CraftingGiveExp((ore + 1) * 25);

            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "Crafted a " + MiningSkill.oreType[ore] + " ingot.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "Not enough " + MiningSkill.oreType[ore] + ".";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }            
    }

    public void GetCraftReq(int lvlReq)
    {
        if (CraftingSkill.craftLvl > lvlReq)
        {
            if (lvlReq == 5)
                OreCheck(1);
            else if (lvlReq == 10)
                OreCheck(2);
            else if (lvlReq == 15)
                OreCheck(3);
            else if (lvlReq == 20)
                OreCheck(4);
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "Crafting level too low to craft this ingot.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();            
        }
    }

    public void IngotCheck(int ingot)
    {
        if (amountOf[ingot] > 0)
        {
            if (ingot == 5)
                PlayerEngine.pCash += 2;
            else if (ingot == 6)
                PlayerEngine.pCash += 20;
            else if (ingot == 7)
                PlayerEngine.pCash += 50;
            else if (ingot == 8)
                PlayerEngine.pCash += 100;
            else if (ingot == 9)
                PlayerEngine.pCash += 200;

            OnLoseItem(ingot, 1);
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You dont have any " + MiningSkill.oreType[ingot - 5] + " ingots.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }            
    }

    public void QuestItemCheck(int item)
    {
        if (amountOf[item] == 0)
        {
            if (item == 10 && PlayerEngine.pCash >= 30)
                PlayerEngine.pCash -= 30;
            else if (item == 11 && PlayerEngine.pCash >= 75)
                PlayerEngine.pCash -= 75;
            else if (item == 12 && PlayerEngine.pCash >= 100)
                PlayerEngine.pCash -= 100;
            else if (item == 13 && PlayerEngine.pCash >= 300)
                PlayerEngine.pCash -= 300;
            else if (item == 14 && PlayerEngine.pCash >= 500)
                PlayerEngine.pCash -= 500;
            else
            {
                PlayerEngine.theDM.dLines = new string[1];
                PlayerEngine.theDM.dLines[0] = "You don't have enough ducats.";
                PlayerEngine.theDM.currentLine = 0;
                PlayerEngine.theDM.ShowDialog();
                return;
            }

            OnGetItem(item);

            if (item == 13)
            {
                BuyItems buyIt = GetComponent<BuyItems>();
                buyIt.AllowPurchaseOfDiary();
            }
                
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "You already have this quest item.";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();            
        }         
    }

    public static void OnGetItem(int item)
    {     
        if (amountOf[item] > 0)
            StackDecreaseItem(item, true, 0);
        else
            AddRemoveItem(item, true);       
    }

    public static void OnLoseItem(int item, int amountLost)
    {
            StackDecreaseItem(item, false, amountLost);
    }

    static void AddRemoveItem(int item, bool add)
    {
        if (add)
        {
            for (int i = 0; i < counter; i++)
            {
                if (slotOpen[i])
                {
                    invSlots[i].SetActive(true);
                    invSlots[i].GetComponent<Image>().sprite = itemIcons[item];
                    amountOf[item] += 1;
                    amountDisplay[i].GetComponent<Text>().text = amountOf[item].ToString();
                    currentInvPos[item] = i;
                    break;
                }
            }
        }
        else
        {
            invSlots[currentInvPos[item]].SetActive(false);
            amountDisplay[currentInvPos[item]].GetComponent<Text>().text = null; 
        }
    }

    static void StackDecreaseItem(int item, bool stack, int decrement)
    {
        if (stack)
        {
            amountOf[item] += 1;
            amountDisplay[currentInvPos[item]].GetComponent<Text>().text = amountOf[item].ToString();
        }
        else
        {
            amountOf[item] -= decrement;

            if(amountOf[item] == 0)
            {
                AddRemoveItem(item, false);
                return;
            }

            amountDisplay[currentInvPos[item]].GetComponent<Text>().text = amountOf[item].ToString(); 
        }
    }
}