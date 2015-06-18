using UnityEngine;
using System.Collections;

public class ObjectDisappear : Photon.MonoBehaviour {
	public bool countDownAtStart;
	public float timerToDestroy;
	bool isPhotonSelfDestruct = false;

//	public delegate void ObjectDestroyNotify();
//	public event ObjectDestroyNotify OnObjectDestroyNotify;

	// Use this for initialization
	void Start () {
		if(countDownAtStart)
			countDownDestroy();
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
//		Debug.Log ("PhotonSelfDestruct transform.parent = " + transform.parent);
		if(transform.parent != null){
			isPhotonSelfDestruct = false;
			countDownDestroy();
			return;
		}
		Debug.Log ("PhotonSelfDestruct");

		PhotonNetwork.Destroy(this.gameObject);
		
	}
}

