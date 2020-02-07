using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [HideInInspector] public bool fade = false;
    [HideInInspector] public string scene;
    public Color loadToColor = Color.black;

    public void whichScene(string sceneName)
    {
        scene = sceneName;
    }

    public void StartFade()
    {
        fade = true;
    }

    void OnGUI()
    {
        if (fade)
        {
            Initiate.Fade(scene, loadToColor, 0.5f);
            fade = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
