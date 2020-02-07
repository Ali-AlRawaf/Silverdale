using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItems : MonoBehaviour
{
    public GameObject[] buy = new GameObject[5], stock = new GameObject[5];
    public bool[] itemsBought = new bool[5];
    public Button purchaseButtonForDiary;

    public void CheckItemBought(int item)
    {
        if (item == 2)
        {
            InventoryManager.OnLoseItem(13, 1);
        }

        if ((item == 0 && PlayerEngine.pCash >= 30) || (item == 1 && PlayerEngine.pCash >= 75) || (item == 2 && PlayerEngine.pCash >= 100) || (item == 3 && PlayerEngine.pCash >= 300) || (item == 4 && PlayerEngine.pCash >= 500))
        {
            itemsBought[item] = true;
            buy[item].SetActive(false);
            stock[item].SetActive(true);
        }
    }

    public void AllowPurchaseOfDiary()
    {        
        purchaseButtonForDiary.interactable = true;
    }
}