using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggering : MonoBehaviour
{
    public GameObject weaponShopInteract, defenseShopInteract, craftInteract;
    [HideInInspector] public UIControl uiControl;

    private void Awake()
    {
        uiControl = GameObject.Find("Player UI Manager").GetComponent<UIControl>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "weaponShopTrigger")
            weaponShopInteract.SetActive(true);
        else if (collision.name == "craftTrigger")
            craftInteract.SetActive(true);
        else if (collision.name == "defenseShopTrigger")
            defenseShopInteract.SetActive(true);
        else if (collision.name == "enterCave")
        {
            uiControl.StartFade(false);
            uiControl.whichScene("Cave Interior");
        }
        else if (collision.name == "enterCastle")
        {
            uiControl.StartFade(false);
            uiControl.whichScene("Castle Interior");
        }
        else if (collision.name == "ReturnToVillage")
        {
            uiControl.StartFade(false);
            uiControl.whichScene("Village");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "weaponShopTrigger")
            weaponShopInteract.SetActive(false);
        else if (collision.name == "craftTrigger")
            craftInteract.SetActive(false);
        else if (collision.name == "defenseShopTrigger")
            defenseShopInteract.SetActive(false);
    }
}
