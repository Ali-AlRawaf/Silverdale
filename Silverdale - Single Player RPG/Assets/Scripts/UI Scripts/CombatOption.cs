using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatOption : MonoBehaviour
{
    public GameObject offenseButton;
    public GameObject defenseButton;
    public static bool isOffense = true;

    void Update()
    {
        if (isOffense)
        {
            offenseButton.SetActive(false);
            defenseButton.SetActive(true);
        }
        else
        {
            offenseButton.SetActive(true);
            defenseButton.SetActive(false);
        }
    }

    public void CombatOptionChanged(bool offense)
    {
        isOffense = offense;
    }
}
