using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class racketRotation : MonoBehaviour {
    Vector3 lob,spin,smash,back,lobTarget,spinTarget,smashTarget,backTarget;
    float speed = 0.8F;
    public static int state;
    float t = 0;
    bool trig;
    public Text hasilR;
    // Use this for initialization
    void Start () {
        state = 4;
        hasilR.text = "";
        lob = new Vector3(190f, 0f, 0f);
        spin = new Vector3(0f, 60f, 270f);
        smash = new Vector3(-60f, 0f, 0f);
        back = new Vector3(0f, 170f, 270f);
        lobTarget = new Vector3(70f, 0f, 0f);
        spinTarget = new Vector3(0f, 300f, 270f);
        smashTarget = new Vector3(76f,180f, 180f);
        smashTarget = new Vector3(76f, 180f, 180f);
        backTarget = new Vector3(0f, 300f, 270f);
        trig = false;
	}
	
	// Update is called once per frame
	void Update () {
           t += Time.deltaTime;
           if (!trig)
           {
               if (state == 0)
               {
                   // hasilR.text += "0";
                   transform.rotation = Quaternion.Euler(lob);
               }
               else if (state == 1)
               {
                   //hasilR.text += "1";
                   transform.rotation = Quaternion.Euler(spin);
               }
               else if (state == 2)
               {
                   // hasilR.text += "2";
                   transform.rotation = Quaternion.Euler(smash);
               }
               else if (state == 3)
               {
                   transform.rotation = Quaternion.Euler(back);
               }
               trig = true;
           }

           else
           {
               if (Vector3.Distance(lobTarget, transform.rotation.eulerAngles) >= 1f && state == 0)
               {

                   transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lobTarget), Time.deltaTime * speed);
               }
               else if (Vector3.Distance(spinTarget, transform.rotation.eulerAngles) >= 1f && state == 1)
               {

                   transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(spinTarget), Time.deltaTime * speed);
               }
               else if (Vector3.Distance(smashTarget, transform.rotation.eulerAngles) >= 1f && state == 2)
               {
                   transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(smashTarget), Time.deltaTime * speed);
               }
               else if (Vector3.Distance(backTarget, transform.rotation.eulerAngles) >= 1f && state == 3)
               {
                   transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(backTarget), Time.deltaTime * speed);
               }
               else
               {
                   transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                   state = 4;
                   trig = false;
               }

           }


	}
}
