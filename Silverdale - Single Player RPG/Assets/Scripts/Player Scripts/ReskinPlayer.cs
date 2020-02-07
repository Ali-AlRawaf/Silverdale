using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReskinPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public static Camera main;
    public GameObject[,] types = new GameObject[4, 3];
    public static GameObject[,] player;

    public GameObject[] none = new GameObject[3], leather = new GameObject[3], iron = new GameObject[3], gold = new GameObject[3];

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            types[0, i] = none[i];
            types[1, i] = leather[i];
            types[2, i] = iron[i];
            types[3, i] = gold[i];
        }

        player = types;
        main = mainCamera;
    }

    public static void Reskin(int def, int wep)
    {
        player[def - 1, wep - 1].SetActive(true);
        main.transform.SetParent(player[def - 1, wep - 1].transform, false);

        for (int d = 0; d < 4; d++)
            for (int w = 0; w < 3; w++)
                if (player[d, w] != player[def - 1, wep - 1])
                    player[d, w].SetActive(false); 
    }
}
