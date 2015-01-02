using UnityEngine;
using System.Collections;

public enum Platform{
	Phone,
	PC_Miniature,
	PC_Giant
};

public class PlatformIndicator : MonoBehaviour {

	[HideInInspector]
	public Platform platform;

	void Awake() {
		if (GameObject.FindGameObjectWithTag ("CNC")) { //phone
			Debug.Log ("mobile phone");
			platform = Platform.Phone;
		} 
		else if (GameObject.FindGameObjectWithTag ("OVR")) {//Giant
			Debug.Log ("Giant in PC");
			platform = Platform.PC_Giant;
		} 
		else {
			Debug.Log ("Miniature in PC");
			platform = Platform.PC_Miniature;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
