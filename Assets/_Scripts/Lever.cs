using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Animator anim;
    public bool playingLeverAnimation;
    // Update is called once per frame
    void Update()
    {
        if (playingLeverAnimation)
        {
            anim.SetFloat("speed",1f);
        }
    }
}
