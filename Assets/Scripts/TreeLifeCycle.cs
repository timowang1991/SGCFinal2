using UnityEngine;
using System.Collections;

public class TreeLifeCycle : Photon.MonoBehaviour {

	public float timeToDestroyAfterRelease = 30f;
	//public BigLittleGameLogic gameLogic;
	//private bool isGiantClient;//Need Identify
	private Platform platform;
	bool isPhotonSelfDestroy = false;
	void Awake() {
		//isGiantClient = (GameObject.FindGameObjectWithTag ("OVR") != null);
		//gameLogic = GameObject.Find ("NetworkManager").GetComponent<BigLittleGameLogic> ();
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
	/// <summary>
	/// Called by GrabAndReleaseTree, distory self after "timeToDestroyAfterRelease" using RPC to tell everyone to destory.
	/// </summary>
	public void countDownDestroy(){
		if (platform == Platform.PC_Giant && isPhotonSelfDestroy == false ) {
			Invoke ("PhotonSelfDestroy", timeToDestroyAfterRelease);
			isPhotonSelfDestroy = true;
		}
	}
	/// <summary>
	/// Called by GrabAndReleaseTree, cacel the destory if the tree hasn't destory.
	/// </summary>
	public void cancelDestroy(){
		if (platform == Platform.PC_Giant && isPhotonSelfDestroy == true) {
			CancelInvoke ("PhotonSelfDestroy");
			isPhotonSelfDestroy = false;
		}
	}

	/// <summary>
	/// Destory now!!![using RPC]
	/// </summary>
	void PhotonSelfDestroy(){
		//photonView.RPC("selfTreeDestroy", PhotonTargets.All, null);
		Debug.Log ("PhotonSelfDestroy");
		PhotonNetwork.Destroy(this.gameObject);

	}
	/// <summary>
	/// if is the giant, then Detach from point.
	/// </summary>
	public void DetachPoint()
	{
		if (platform == Platform.PC_Giant) {
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
		}
	}
	/// <summary>
	/// if is the giant, then Attach from point.
	/// </summary>
	public void AttachPoint(string AttachObjectName)
	{
		if (platform == Platform.PC_Giant) {
			photonView.RPC ("RPCAttachPoint", PhotonTargets.All, AttachObjectName);
		}
	}
	/// <summary>
	/// When attached, tree's gravity is false, and it's kinematic. AttachObjectTag tell's which point to attach.
	/// </summary>
	[RPC]
	public void RPCAttachPoint(string AttachObjectTag)
	{
		Debug.Log(AttachObjectTag);
		this.gameObject.transform.parent = GameObject.FindGameObjectWithTag(AttachObjectTag).transform;
		this.gameObject.rigidbody.isKinematic = true;
		this.gameObject.rigidbody.useGravity = false;
	}
	[RPC]
	public void RPCDetachPoint()
	{
		this.gameObject.transform.parent = null;
	}
}
