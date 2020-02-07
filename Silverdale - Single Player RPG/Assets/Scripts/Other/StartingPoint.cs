using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPoint : MonoBehaviour
{
    public string pointName;

    PlayerEngine pEng;
    ClickTarget click;

	void Start()
    {
        pEng = FindObjectOfType<PlayerEngine>();
        click = FindObjectOfType<ClickTarget>();
        if (pEng.startingPointName == pointName)
        {
            pEng.gameObject.transform.position = transform.position;
            click.gameObject.transform.position = pEng.gameObject.transform.position;
        }
	}
}
