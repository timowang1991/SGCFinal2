using UnityEngine;
using System.Collections;

public class ObjectDisappear : Photon.MonoBehaviour {
	public float timerToDestroy = 30f;
	bool isPhotonSelfDestruct = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Called by GrabAndReleaseTree, distory self after "timerToDestroy" using RPC to tell everyone to destory.
	/// </summary>
	public void countDownDestroy(){
		if (isPhotonSelfDestruct == false ) {
			Invoke ("PhotonSelfDestruct", timerToDestroy);
			isPhotonSelfDestruct = true;
		}
	}
	/// <summary>
	/// Called by GrabAndReleaseTree, cancel the destory if the tree hasn't destory.
	/// </summary>
	public void cancelDestroy(){
		if (isPhotonSelfDestruct == true) {
			CancelInvoke ("PhotonSelfDestruct");
			isPhotonSelfDestruct = false;
		}
	}
	
	/// <summary>
	/// Destory now!!![using RPC]
	/// </summary>
	void PhotonSelfDestruct(){
		//photonView.RPC("selfTreeDestroy", PhotonTargets.All, null);
		Debug.Log ("PhotonSelfDestruct");
		PhotonNetwork.Destroy(this.gameObject);
		
	}
}
