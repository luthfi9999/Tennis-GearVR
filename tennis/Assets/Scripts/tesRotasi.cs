using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesRotasi : MonoBehaviour {
    Quaternion target, awal,rotation;
    bool t1, t2;
    Vector3 sel;
	// Use this for initialization
	void Start () {
        awal = transform.rotation;
        target.Set(0.3f, 0.1f, -0.5f, 0.8f);
        t1 = false;
        t2 = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!t1)
        {
            Debug.Log(transform.rotation);
            rotation = target * Quaternion.Inverse(transform.rotation);
            transform.rotation = rotation * transform.rotation;
            Debug.Log("rotation : " + rotation);
            Debug.Log(transform.rotation);
            //hitung = target * Quaternion.Inverse(awal);
            //hitung2 = Quaternion.Inverse(awal) * target;

            //hitung3.Set((target.x-awal.x), (target.y - awal.y), (target.z - awal.z), (target.w - awal.w));

            //Debug.Log("Sebelum rotasi :     " + transform.rotation);
            //transform.rotation = target;
            //Debug.Log("Setelah rotasi :     " + transform.rotation);
            //Debug.Log("Sd :     " + hitung3);
            ////Debug.Log("Sebelum rotasi :     "+transform.rotation);
            //Debug.Log("euler sebelum  :     " + transform.eulerAngles);
            //Debug.Log("quaternion ke euler sebelum         :" + awal.eulerAngles);
            t1 = true;
        }
        //transform.rotation = target*Quaternion.Inverse(awal);
        //if (!t2)
        //{
        //    sel.Set((transform.eulerAngles.x - awal.eulerAngles.x),(transform.eulerAngles.y - awal.eulerAngles.y),(transform.eulerAngles.z - awal.eulerAngles.z));
        //    Debug.Log("Setelah rotasi :     "+transform.rotation);
        //    Debug.Log("hasil euler  :       " + transform.eulerAngles);
        //    Debug.Log("perhitungan selisih euler  :" + sel);   
        //    t2 = true;
        //}
    }
}
