using UnityEngine;
using System.Collections;

public class Arrow_NetSync : Photon.MonoBehaviour {
	private float scaleScalar;
	private GameObject Arrow_clone;
	private Transform Bow;
	private Transform R_hand;
	private Transform mountedPosOnBow;
	private float speed;
	private float rotAngle;
	// Use this for initialization
	void Start () {
		if (photonView.instantiationData != null) {
			int characterViewID = (int)photonView.instantiationData[0];
			GameObject avatar = PhotonView.Find(characterViewID).gameObject;
			ArrowGenerator arrowGen = avatar.GetComponent<ArrowGenerator>();
			rotAngle = arrowGen.angleToRot;
			scaleScalar = arrowGen.scaleOfArrow;
			speed = arrowGen.Speed;
			Bow = arrowGen.Bow;
			R_hand = arrowGen.R_hand;
			mountedPosOnBow = arrowGen.mountedPosOnBow;
			Arrow_clone = gameObject;
			Arrow_clone.transform.localScale = new Vector3(scaleScalar,scaleScalar,scaleScalar);
			Arrow_clone.transform.parent = Bow.transform;
			Vector3 relativePos = Bow.position - R_hand.position;


			Arrow_clone.transform.LookAt(transform.position - mountedPosOnBow.forward);
			//Arrow_clone.transform.rotation = Quaternion.LookRotation(relativePos,R_hand.right);
			//Arrow_clone.transform.rotation *= Quaternion.Euler(rotAngle,0,0);
			Arrow_clone.collider.enabled = false;
			Arrow_clone.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.holding;
		}
	}

	[RPC]
	public void beShotOutRPC(Vector3 direction) {

		//direction = mountedPosOnBow.forward/mountedPosOnBow.forward.magnitude;
		direction = direction/direction.magnitude;

		Arrow_clone.transform.parent = null;
		Arrow_clone.rigidbody.isKinematic = false;
		Arrow_clone.rigidbody.useGravity = true;
		Arrow_clone.transform.LookAt(Arrow_clone.transform.position - direction);
		Arrow_clone.rigidbody.velocity = speed * direction;//test/test.magnitude;
		Arrow_clone.collider.enabled = true;
		Arrow_clone.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.shooted;
		//Destroy (Arrow_clone, 10);
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	public void invokeDestroy() {
		Invoke ("destroySelf", 10);
	}

	private void destroySelf() {
		PhotonNetwork.Destroy (Arrow_clone);
	}
}
