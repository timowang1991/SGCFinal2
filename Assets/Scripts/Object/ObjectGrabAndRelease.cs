using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGrabAndRelease : Photon.MonoBehaviour {

	public List<string> grabbableBodyPartTagList;
	HashSet<string> grabbableBodyPartTagSet;
	
	GameObject parentGameObject;
	
	// Use this for initialization
	void Start () {
		grabbableBodyPartTagSet = new HashSet<string>();
		foreach (string tagString in grabbableBodyPartTagList){
			grabbableBodyPartTagSet.Add(tagString);
		}
		grabbableBodyPartTagList = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

	}

	void OnTriggerExit(Collider other){

	}
	
	void OnCollisionExit(Collision collision){

	}

	void OnCollisionEnter(Collision collision){
//		Debug.Log("ObjectGrabAndRelease OnCollisionEnter " + collision.gameObject.name);
		if(grabbableBodyPartTagSet.Contains(collision.gameObject.tag) && parentGameObject == null){
			parentGameObject = collision.gameObject;
			photonView.RPC ("RPCAttachPoint", PhotonTargets.All, null);
		}
	}

	[RPC]
	public void RPCAttachPoint()
	{
		transform.parent = parentGameObject.transform;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		collider.isTrigger = true;
	}

	[RPC]
	public void RPCDetachPoint()
	{
		transform.parent = null;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		collider.isTrigger = false;
	}

//	public void GrabObjectByGameObject(GameObject gObject){
//		if(){
//
//		}
//	}

	public void ReleaseObject(){
		if(parentGameObject != null){
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
			parentGameObject = null;
		}
	}

	public bool isGrabbableToGameObject(GameObject gObject){
		if(grabbableBodyPartTagSet.Contains(gObject.tag)){ //&& parentGameObject == null){
			return true;
		} else {
			return false;
		}
	}
}
