using UnityEngine;
using System.Collections;

public class Arrow_NetSync : Photon.MonoBehaviour {
	private const int scaleScalar = 10;
	private GameObject Arrow_clone;
	private Transform Bow;
	private Transform R_hand;
	private float speed;
	// Use this for initialization
	void Start () {
		if (photonView.instantiationData != null) {
			int characterViewID = (int)photonView.instantiationData[0];
			GameObject avatar = PhotonView.Find(characterViewID).gameObject;
			ArrowGenerator arrowGen = avatar.GetComponent<ArrowGenerator>();
			speed = arrowGen.Speed;
			Bow = arrowGen.Bow;
			R_hand = arrowGen.R_hand;
			Arrow_clone = gameObject;
			Arrow_clone.transform.localScale = new Vector3(scaleScalar,scaleScalar,scaleScalar);
			Arrow_clone.transform.parent = Bow.transform;
			Vector3 relativePos = Bow.position - R_hand.position;
			Arrow_clone.transform.rotation = Quaternion.LookRotation(relativePos,Bow.right);
			Arrow_clone.transform.rotation *= Quaternion.Euler(240, 0, 0); 
			Arrow_clone.collider.enabled = false;
			Arrow_clone.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.holding;
		}
	}

	[RPC]
	public void beShotOutRPC(Vector3 direction) {
		
		Arrow_clone.transform.parent = null;
		Arrow_clone.rigidbody.isKinematic = false;
		Arrow_clone.rigidbody.useGravity = true;
		Arrow_clone.rigidbody.velocity = speed * direction/direction.magnitude;//test/test.magnitude;
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
