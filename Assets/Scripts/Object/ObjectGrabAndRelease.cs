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

	[RPC]
	public void RPCAttachPoint(int viewId)
	{
		parentGameObject = PhotonView.Find(viewId);
		transform.parent = parentGameObject.transform;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
//		collider.isTrigger = true;
	}

	[RPC]
	public void RPCDetachPoint()
	{
		transform.parent = null;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
//		collider.isTrigger = false;
	}

	public bool GrabObjectByGameObject(GameObject gObject){
		if(!IsGrabbableToGameObject(gObject)) return false;


		photonView.RPC ("RPCAttachPoint", PhotonTargets.All, gObject.GetComponent<PhotonView>().viewID);
		return true;
	}

	public void ReleaseObject(){
		if(parentGameObject != null){
			photonView.RPC ("RPCDetachPoint", PhotonTargets.All, null);
			parentGameObject = null;
		}
	}

	public bool IsGrabbableToGameObject(GameObject gObject){
		if(grabbableBodyPartTagSet.Contains(gObject.tag) && parentGameObject == null){
			return true;
		} else {
			return false;
		}
	}
}
