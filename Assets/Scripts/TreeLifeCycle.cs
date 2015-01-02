using UnityEngine;
using System.Collections;

public class TreeLifeCycle : Photon.MonoBehaviour {

	public float timeToDestroyAfterRelease = 30f;
	//public BigLittleGameLogic gameLogic;
	private bool isGiantClient;
	void Awake() {
		isGiantClient = (GameObject.FindGameObjectWithTag ("OVR") != null);
		//gameLogic = GameObject.Find ("NetworkManager").GetComponent<BigLittleGameLogic> ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void countDownDestroy(){
		if (isGiantClient) {
			Invoke ("PhotonSelfDestroy", timeToDestroyAfterRelease);
		}
	}

	public void cancelDestroy(){
		if (isGiantClient) {
			CancelInvoke ("PhotonSelfDestroy");
		}
	}

	void PhotonSelfDestroy(){
		//photonView.RPC("selfTreeDestroy", PhotonTargets.All, null);
		Debug.Log ("PhotonSelfDestroy");
		PhotonNetwork.Destroy(this.gameObject);

	}

	public void DetachPoint()
	{
		if (isGiantClient) {
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
		}
	}

	public void AttachPoint(string AttachObjectName)
	{
		if (isGiantClient) {
			photonView.RPC ("RPCAttachPoint", PhotonTargets.All, AttachObjectName);
		}
	}
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
