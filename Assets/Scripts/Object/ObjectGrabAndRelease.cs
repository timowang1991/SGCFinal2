using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGrabAndRelease : Photon.MonoBehaviour {

	public List<string> grabbableBodyPartTags;
	
	GameObject parentGameObject;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

	}

	void OnTriggerExit(Collider other){
		if(parentGameObject != null){
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
			parentGameObject = null;
		}
	}

	/// <summary>
	/// then Detach from point.
	/// </summary>
	void OnCollisionExit(Collision collision)
	{
//		Debug.Log("ObjectGrabAndRelease OnCollisionExit " + collision.gameObject.name);
//		if(parentGameObject != null){
//			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
//			parentGameObject = null;
//		}
	}
	
	/// <summary>
	/// then Attach from point.
	/// </summary>
	void OnCollisionEnter(Collision collision)
	{
//		Debug.Log("ObjectGrabAndRelease OnCollisionEnter " + collision.gameObject.name);
		if(grabbableBodyPartTags.Contains(collision.gameObject.tag) && parentGameObject == null){
			parentGameObject = collision.gameObject;
			photonView.RPC ("RPCAttachPoint", PhotonTargets.All, null);
		}
	}
	
	/// <summary>
	/// When attached, tree's gravity is false, and it's kinematic. AttachObjectTag tell's which point to attach.
	/// </summary>
	[RPC]
	public void RPCAttachPoint()
	{
		this.gameObject.transform.parent = parentGameObject.transform;
		this.gameObject.rigidbody.isKinematic = true;
		this.gameObject.rigidbody.useGravity = false;
		collider.isTrigger = true;
	}

	[RPC]
	public void RPCDetachPoint()
	{
		this.gameObject.transform.parent = null;
		this.gameObject.rigidbody.isKinematic = false;
		this.gameObject.rigidbody.useGravity = true;
		collider.isTrigger = false;
	}
}
