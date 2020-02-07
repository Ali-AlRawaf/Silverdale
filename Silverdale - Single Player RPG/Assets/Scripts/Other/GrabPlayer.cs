using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPlayer : MonoBehaviour
{
    PlayerEngine player;	

	void Start()
    {
        player = FindObjectOfType<PlayerEngine>();
        player.GetComponent<MiningSkill>().pickaxe = gameObject;
	}
}
