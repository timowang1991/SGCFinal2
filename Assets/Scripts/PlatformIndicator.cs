using UnityEngine;
using System.Collections;

public enum Platform{
	Phone,
	PC_NonGiant,
	PC_Giant,
};

public class PlatformIndicator : MonoBehaviour {

	[HideInInspector]
	public Platform platform;

	//if find then which platform is it, please ensure everyone has this.
	void Awake() {
		if (GameObject.FindGameObjectWithTag (GameConfig.PhoneTag)) {
			Debug.Log ("mobile phone");
			platform = Platform.Phone;
		} 
		else if (GameObject.FindGameObjectWithTag (GameConfig.PCGiantTag)) {
			Debug.Log ("Giant PC");
			platform = Platform.PC_Giant;
		}
		else {
			Debug.Log ("Non-Giant PC");
			platform = Platform.PC_NonGiant;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
