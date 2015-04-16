using UnityEngine;
using System.Collections;

public class CopterRPCs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//the below code should not depend on variable values initiated in Start or Awake
	
	[RPC]
	void LaunchMissile(Vector3 pos, Vector3 direction) {
		
	}
	
	[RPC]
	void FireMachineGun(Vector3 pos, Vector3 direction) {
		
	}

}
