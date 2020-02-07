using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestObject[] quests;
    public DialogManager theDM;
    public bool[] completed;
    public int itemNum;
    public string itemCollected;
    public string enemyKilled;

    void Start()
    {
        completed = new bool[quests.Length];
    }

    public void ShowQuestText(string[] questText)
    {
        theDM.dLines = new string[questText.Length];

        for (int i = 0; i < theDM.dLines.Length; i++)
            theDM.dLines[i] = questText[i];
        
        theDM.currentLine = 0;
        theDM.ShowDialog();
    }
}