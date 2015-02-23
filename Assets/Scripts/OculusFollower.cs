using UnityEngine;
using System.Collections;

public class OculusFollower : MonoBehaviour {

	GameObject oculusFollower;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(oculusFollower == null)
			return;
		transform.position = oculusFollower.transform.position;
//		Debug.Log("oculusFollower update");
	}

	public void setOculusFollower(){
		oculusFollower = GameObject.FindGameObjectWithTag("OculusFollower");
	}
}
