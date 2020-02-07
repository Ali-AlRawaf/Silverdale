using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public int questNumber;
    public bool startQuest;
    public bool endQuest;

    QuestManager theQM;    

	void Start()
    {
        theQM = FindObjectOfType<QuestManager>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !theQM.completed[questNumber])
        {
            if (startQuest && !theQM.quests[questNumber].gameObject.activeSelf)
            {
                if (questNumber == 10)
                    theQM.quests[questNumber].colliderToEnable = GameObject.Find("EnterBasement");

                if (theQM.quests[questNumber].reliesOnQuest)
                {
                    if (theQM.completed[theQM.quests[questNumber].questRely.questNum])
                    {
                        theQM.quests[questNumber].gameObject.SetActive(true);
                        theQM.quests[questNumber].StartQuest();
                        return;
                    }
                    else
                        return;
                }
                    
                theQM.quests[questNumber].gameObject.SetActive(true);
                theQM.quests[questNumber].StartQuest();
            }
            
            if (endQuest && theQM.quests[questNumber].gameObject.activeSelf)
            {
                theQM.completed[questNumber] = true;
                theQM.quests[questNumber].gameObject.SetActive(false);
                theQM.quests[questNumber].EndQuest();
            }
        }
    }
}
