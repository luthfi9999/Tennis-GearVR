using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Kelas untuk mengatur animasi
public class playerTennis : MonoBehaviour
{
    public Animator anim;
    public static int state=4;

    // Update is called once per frame
    void Update()
    {
        //lob
        if (state == 0)
        {
            anim.SetTrigger("Slide right");
            state = 4;
        }
        //forehand
        else if (state == 1)
        {
            anim.SetTrigger("Hit right");
            state = 4;
        }
        //smash
        else if (state == 2)
        {
            anim.SetTrigger("Serve");
            state = 4;
        }
        //backhand
        else if (state == 3)
        {
            anim.SetTrigger("Hit left");
            state = 4;
        }
    }
}
