using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dBox;        
    public Text dText;

    [HideInInspector]
    public bool dActive = false;
    [HideInInspector]
    public string[] dLines;
    [HideInInspector]
    public int currentLine;

    ClickTarget pTar;
    PlayerEngine player;
    AIPath aiPath;

    void Start()
    {
        player = FindObjectOfType<PlayerEngine>();
        pTar = FindObjectOfType<ClickTarget>();
        aiPath = player.gameObject.GetComponent<AIPath>();
    }

    void Update()
    {
        if (dActive && Input.GetKeyUp(KeyCode.Space))
            currentLine++;
            
        if (currentLine >= dLines.Length)
        {
            dBox.SetActive(false);
            pTar.gameObject.SetActive(true);
            dActive = false;
            currentLine = -1;
        }

        if (dActive)
            dText.text = dLines[currentLine];
    }

    public void ShowDialog()
    {        
        dBox.SetActive(true);
        dActive = true;
        aiPath.target = player.transform;
        pTar.transform.position = player.transform.position;
        pTar.gameObject.SetActive(false);
    }

}
