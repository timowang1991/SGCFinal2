using UnityEngine;
using System.Collections;

public class TestArray : MonoBehaviour {

	bool[] bArray = new bool[12];

	// Use this for initialization
	void Start () {
		for(int i = 0;i < 12;i++) {
			bArray[i] = false;
			/*
			if(bArray[i] == false) {
				Debug.Log ("yes,i == " + i);
			}
			*/
		}

		//Debug.Log ("toString:" + bArray.ToString());

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
