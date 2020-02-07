using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePanel : MonoBehaviour
{
    public GameObject[] buttons = new GameObject[8];
    int counter = 0;

    public void showHideVolumePanel(GameObject panel)
    {
        counter++;

        if (counter % 2 == 1)
        {
            panel.gameObject.SetActive(true);
            for (int i = 0; i < 4; i++) buttons[i].gameObject.GetComponent<Button>().enabled = false;   
        }
        else
        {
            panel.gameObject.SetActive(false);
            for (int i = 0; i < 4; i++) buttons[i].gameObject.GetComponent<Button>().enabled = true;
        }
    }

    public void showHideVillagePanel(GameObject panel)
    {
        counter++;

        if (counter % 2 == 1)
        {
            panel.gameObject.SetActive(true);

            if (UIControl.currentScene != "Village")
            {
                if (panel.name == "SkillsPanel")
                {
                    for (int i = 0; i < 4; i++)
                        buttons[i].GetComponent<Button>().enabled = false;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        buttons[i].GetComponent<Button>().enabled = false;

                    buttons[7].GetComponent<Button>().enabled = false;
                }

            }
            else
            {
                if (panel.name == "SkillsPanel")
                {
                    for (int i = 0; i < 7; i++)
                        buttons[i].GetComponent<Button>().enabled = false;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                        buttons[i].GetComponent<Button>().enabled = false;
                }
            }
        }
        else
        {
            panel.gameObject.SetActive(false);

            if (UIControl.currentScene != "Village")
            {
                if (panel.name == "SkillsPanel")
                {
                    for (int i = 0; i < 4; i++)
                        buttons[i].GetComponent<Button>().enabled = true;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        buttons[i].GetComponent<Button>().enabled = true;

                    buttons[7].GetComponent<Button>().enabled = true;
                }

            }
            else
            {
                if (panel.name == "SkillsPanel")
                {
                    for (int i = 0; i < 7; i++)
                        buttons[i].GetComponent<Button>().enabled = true;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                        buttons[i].GetComponent<Button>().enabled = true;
                }
            }
        }
    }
}
