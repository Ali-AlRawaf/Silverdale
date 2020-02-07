using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : MonoBehaviour
{
    public GameObject pTar;
    public string[] dialogLines;

    DialogManager dMan;    
	
	void Start()
    {
        dMan = FindObjectOfType<DialogManager>();
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!dMan.dActive)
                {
                    pTar.SetActive(false);
                    dMan.dLines = dialogLines;
                    dMan.currentLine = -1;                   
                    dMan.ShowDialog();
                }                   
            }
    }
}
