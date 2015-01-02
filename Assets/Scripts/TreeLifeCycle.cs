using UnityEngine;
using System.Collections;

public class TreeLifeCycle : Photon.MonoBehaviour {

	public float timeToDestroyAfterRelease = 30f;
	bool iscountDownDestroy = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void countDownDestroy(){
		if(iscountDownDestroy == false)
		{
			Invoke("PhotonSelfDestroy", timeToDestroyAfterRelease);
			iscountDownDestroy = true;
		}
	}

	public void cancelDestroy(){
		if(iscountDownDestroy == true)
		{
			CancelInvoke("PhotonSelfDestroy");
			iscountDownDestroy = false;
		}
	}

	void PhotonSelfDestroy(){
		//photonView.RPC("selfTreeDestroy", PhotonTargets.All, null);
		Debug.Log("PhotonSelfDestroy called :" + photonView.viewID);
		if(GameObject.FindGameObjectWithTag("OVR") !=null)
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
	}

	public void DetachPoint()
	{
		photonView.RPC("RPCDetachPoint", PhotonTargets.All, null);
	}

	public void AttachPoint(string AttachObjectName)
	{
		photonView.RPC("RPCAttachPoint", PhotonTargets.All, AttachObjectName);
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
