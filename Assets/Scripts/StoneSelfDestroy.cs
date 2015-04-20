using UnityEngine;
using System.Collections;

public class StoneSelfDestroy : MonoBehaviour {

	public float timeToDestroy = 20.0f;

	// Use this for initialization
	void Start () {
		Invoke ("selfDestroy", timeToDestroy);
	}

	
	// Update is called once per frame
	void Update () {

	}

	void selfDestroy(){
		this.GetComponent<PhotonView>().RPC("tellMasterToDestroy",this.GetComponent<PhotonView>().owner);
	}

	[RPC]
	void tellMasterToDestroy(){
		PhotonNetwork.Destroy(gameObject);
	}
}
