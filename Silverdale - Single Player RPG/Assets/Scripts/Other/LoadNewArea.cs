using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour
{
    public string loadLevel;
    public string exitPoint;

    PlayerEngine pEng;
    ClickTarget click;

    [HideInInspector] public UIControl uiControl;

    private void Awake()
    {
        uiControl = FindObjectOfType<UIControl>();
    }

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            pEng = FindObjectOfType<PlayerEngine>();
            pEng.startingPointName = exitPoint;
            pEng.GetComponent<AIPath>().target = pEng.transform;            
            click = FindObjectOfType<ClickTarget>();
            click.transform.position = pEng.transform.position;
            click.gameObject.SetActive(false);
            //pEng.gameObject.GetComponent<AIPath>().enabled = false;
            uiControl.whichScene(loadLevel);
            uiControl.StartFade(false);
            //pEng.gameObject.GetComponent<AIPath>().enabled = true;
            click.gameObject.SetActive(true);
            pEng.GetComponent<AIPath>().target = pEng.transform;
            click.transform.position = pEng.transform.position;
        }
    }

}
