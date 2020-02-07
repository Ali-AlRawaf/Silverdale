using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    private static PlayerDamageNumbers damageText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("DamageCanvas");
        if (damageText == null)
            damageText = Resources.Load<PlayerDamageNumbers>("Prefabs/pdmgParent");
    }

    public static void CreateDamageText(string text, Transform location)
    {
        PlayerDamageNumbers instance = Instantiate(damageText);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = new Vector2(location.position.x + Random.Range(-0.1f, 0.1f), location.position.y + Random.Range(0.1f, 0.3f));
        instance.SetText(text);
    }
}
