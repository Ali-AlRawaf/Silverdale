using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public int questNum;
    public int itemNum;
    public string itemName;

    QuestManager theQM;

    void Start()
    {
        theQM = FindObjectOfType<QuestManager>();
    }    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !theQM.completed[questNum] && theQM.quests[questNum].gameObject.activeSelf)
        {
            theQM.itemCollected = itemName;
            theQM.itemNum = itemNum;
            gameObject.SetActive(false);
        }
    }
}