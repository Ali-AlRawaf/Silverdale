using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    public Animator anim;
    private Text dmgText;    

    void OnEnable()
    {
        DamageController.Initialize();
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        dmgText = anim.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        dmgText.text = text;
    }

}
