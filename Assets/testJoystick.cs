using UnityEngine;
using System.Collections;

public class testJoystick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("j0")) {
			Debug.Log ("J0 down");
		}

		if (Input.GetButtonDown ("j1")) {
			Debug.Log ("J1 down");
		}

		if (Input.GetButtonDown ("j2")) {
			Debug.Log ("J2 down");
		}

		if (Input.GetButtonDown ("j3")) {
			Debug.Log ("J3 down");
		}

	}
}
