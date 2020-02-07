using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Vector3 previous, velocity;
    public static Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update ()
    {
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;

        if (velocity.x < 0.05f && velocity.x > -0.05f && velocity.y < 0.05f && velocity.y > -0.05f)
            anim.SetInteger("Direction", 0);
        else if (velocity.y > 0.2f)
            anim.SetInteger("Direction", 1);
        else if (velocity.y < -0.2f)
            anim.SetInteger("Direction", 2);
        else if (velocity.x > 0.2f)
            anim.SetInteger("Direction", 3);
        else if (velocity.x < -0.2f)
            anim.SetInteger("Direction", 4);
    }
}
