using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	//Kelas untuk menggerakan Remote GearVR
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        transform.rotation= OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
    }
}
