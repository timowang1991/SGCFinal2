using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabAndReleaseForObjects : Photon.MonoBehaviour {
	public List<GameObject> grabbableCharacters;
	public bool grabbed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// then Detach from point.
	/// </summary>
	public void OnCollisionExit(Collision collision)
	{
		if(grabbableCharacters.Contains(collision.gameObject) && grabbed){
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
			grabbed = false;
		}
	}

	/// <summary>
	/// then Attach from point.
	/// </summary>
	public void OnCollisionEnter(Collision collision)
	{
		if(grabbableCharacters.Contains(collision.gameObject)){
			photonView.RPC ("RPCAttachPoint", PhotonTargets.All, collision.gameObject.tag);
			grabbed = true;
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
//		this.gameObject.rigidbody.isKinematic = false;
//		this.gameObject.rigidbody.useGravity = true;
	}
}
