using UnityEngine;
using System.Collections;

public class StoneSelfDestroy : MonoBehaviour {

	public bool countDownAtStart;
	public float timeToDestroy;
	bool isPhotonSelfDestruct = false;

	// Use this for initialization
	void Start () {
		if(countDownAtStart)
			countDownDestroy();
	}

	
	// Update is called once per frame
	void Update () {

	}

	public void countDownDestroy(){
		if (isPhotonSelfDestruct == false ) {
			Invoke ("PhotonSelfDestruct", timeToDestroy);
			isPhotonSelfDestruct = true;
		}
	}

	void PhotonSelfDestruct(){
		if(transform.parent != null){
			isPhotonSelfDestruct = false;
			countDownDestroy();
			return;
		}

		this.GetComponent<PhotonView>().RPC("tellMasterToDestroy",this.GetComponent<PhotonView>().owner);
	}

	[RPC]
	void tellMasterToDestroy(){
		PhotonNetwork.Destroy(gameObject);
	}
}
