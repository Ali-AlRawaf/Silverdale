using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{    
    public QuestManager theQM;
    public int questNum;
    [Header("Text Attributes")]
    public string[] startText;
    public string[] endText;
    [Header("Relying On Quest Attributes")]
    public bool reliesOnQuest;
    public QuestObject questRely;
    [Header("Kill Quest Attributes")]
    public string targetEnemy;
    public int targetKillCount;
    public int currentKillCount;
    public bool isKilling;
    [Header("Item Quest Attributes")]
    public bool isItemQuest;
    public int itemShopNum;
    public bool removeItemOnStart;
    public int itemNumForRemovalOnStart;
    [Header("Other")]
    public bool enablesACollider;
    public string nameOfCollider;
    public GameObject colliderToEnable;
    
    BuyItems shop;

    void Start()
    {
        shop = GetComponentInParent<BuyItems>();
    }

	void Update()
    {
        if (isItemQuest)
        {
            if (shop.itemsBought[itemShopNum])
            {
                theQM.itemCollected = null;
                EndQuest();
            }            
        }                          

        if (isKilling)
        {
            if (theQM.enemyKilled == targetEnemy)
            {
                theQM.enemyKilled = null;
                currentKillCount++;
            }

            if (currentKillCount >= targetKillCount)
                EndQuest();
        }


        if (enablesACollider)
            if (GameObject.Find("QuestStartTrigger(10)") != null)
            {
                colliderToEnable = GameObject.Find("EnterBasement");  
                colliderToEnable.GetComponent<BoxCollider2D>().isTrigger = true;
            }
    }

    public void StartQuest()
    {
        if (removeItemOnStart)
            InventoryManager.OnLoseItem(itemNumForRemovalOnStart, 1);
        
        gameObject.SetActive(true);
        theQM.ShowQuestText(startText);
    }

    public void EndQuest()
    {
        theQM.completed[questNum] = true;
        if (endText.Length > 0)
            theQM.ShowQuestText(endText);

        gameObject.SetActive(false);        
    }
}
