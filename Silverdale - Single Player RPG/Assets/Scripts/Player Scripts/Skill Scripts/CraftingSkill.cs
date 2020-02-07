using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSkill : MonoBehaviour
{
    public static int craftLvl = 1;
    public static float craftExp;
	
	void Start ()
    {
        craftExp = PlayerEngine.FindNextLevelExpRequirement(craftLvl);
	}

    public static void CraftingGiveExp(float exp)
    {
        if (PlayerEngine.MaxLevel(craftLvl))
            return;

        craftExp += exp;
        Debug.Log("Crafting Exp: " + craftExp);
        if (craftExp >= PlayerEngine.FindNextLevelExpRequirement(craftLvl + 1))
            CraftingLevel();
    }

    //Levels crafting
    static void CraftingLevel()
    {
        craftLvl++;
        PlayerEngine.theDM.dLines = new string[1];
        PlayerEngine.theDM.dLines[0] = "You have leveled up crafting. You are now at level: " + craftLvl.ToString() + ".";
        PlayerEngine.theDM.currentLine = 0;
        PlayerEngine.theDM.ShowDialog();
    }
}
