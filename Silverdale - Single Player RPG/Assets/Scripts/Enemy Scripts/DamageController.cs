using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private static DamageNumbers damageText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("DamageCanvas");
        if (damageText == null)
            damageText = Resources.Load<DamageNumbers>("Prefabs/edmgParent");
    }

    public static void CreateDamageText(string text, Transform location)
    {
        DamageNumbers instance = Instantiate(damageText);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = new Vector2(location.position.x + Random.Range(-0.1f, 0.1f), location.position.y + Random.Range(0.1f, 0.3f));
        instance.SetText(text);
    }
}
