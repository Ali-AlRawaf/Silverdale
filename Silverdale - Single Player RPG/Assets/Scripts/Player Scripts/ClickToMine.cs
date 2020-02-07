using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMine : MonoBehaviour
{     
    public GameObject emptyObject;

    int lvlReq;
    float expReward;
    float respawnTime;
    bool minedOut = false;

    AIPath aiPath;
    GameObject player;
    Sprite original;
    Sprite empty;
    MiningSkill mineSkill;
    SpriteRenderer spriteR;

    void Awake()
    {
        player = FindObjectOfType<PlayerEngine>().gameObject;

        switch (transform.tag)
        {
            case "Bronze":
                lvlReq = 1;
                expReward = 17.5f;
                respawnTime = 5f;
                break;
            case "Silver":
                lvlReq = 5;
                expReward = 26.5f;
                respawnTime = 15f;
                break;
            case "Gold":
                lvlReq = 10;
                expReward = 65f;
                respawnTime = 80f;
                break;
            case "Sapphire":
                expReward = 80;
                lvlReq = 15;
                respawnTime = 90f;
                break;
            case "Ruby":
                expReward = 125f;
                lvlReq = 20;
                respawnTime = 90f;
                break;
        }

        spriteR = GetComponent<SpriteRenderer>();
        aiPath = player.GetComponent<AIPath>();
        mineSkill = player.GetComponent<MiningSkill>();

        original = spriteR.sprite;
        empty = emptyObject.GetComponent<SpriteRenderer>().sprite;
    }

    public IEnumerator RespawnOre()
    {
        spriteR.sprite = empty;
        minedOut = true;
        yield return new WaitForSeconds(respawnTime);
        spriteR.sprite = original;
        minedOut = false;
    }

    public float exp
    {
        get
        {
            return expReward;
        }
    }
    

    void OnMouseDown()
    {
        MiningSkill.clickedToMine = true;
        if (!minedOut)
        {            
            if (Vector3.Distance(player.transform.position, transform.position) <= 1f)
                aiPath.target = player.transform;
            else
                aiPath.target = transform;

            mineSkill.GetOre(transform, lvlReq);
        }
        else
        {
            PlayerEngine.theDM.dLines = new string[1];
            PlayerEngine.theDM.dLines[0] = "This ore has already been mined out!";
            PlayerEngine.theDM.currentLine = 0;
            PlayerEngine.theDM.ShowDialog();
        }            
    }
}