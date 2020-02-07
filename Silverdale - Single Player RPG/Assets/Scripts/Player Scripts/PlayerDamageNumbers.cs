using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageNumbers : MonoBehaviour
{
    public Animator anim;
    private Text dmgText;

    void OnEnable()
    {
        PlayerDamageController.Initialize();
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length - 0.01f);
        dmgText = anim.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        dmgText.text = text;
    }
}
