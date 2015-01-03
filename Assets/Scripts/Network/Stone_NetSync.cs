using UnityEngine;
using System.Collections;

public class Stone_NetSync : Photon.MonoBehaviour {
	private float Speed;
	public GameObject mountedCatapult;
	private GameObject Stone_clone;
	private CatapultsController cataCtrl;
	// Use this for initialization
	void Start () {
		if (photonView.instantiationData != null) {
			Stone_clone = gameObject;
			int cataViewID = (int)photonView.instantiationData[0];
			mountedCatapult = PhotonView.Find (cataViewID).gameObject;
			cataCtrl = mountedCatapult.GetComponent<CatapultsController>();
			Speed = cataCtrl.Speed;
			Transform stonePosTrans = mountedCatapult.transform.Find("Catapult_Bone_Main/Catapult_Bone_03/StonePosition");
			transform.parent = stonePosTrans;
		}
	
	}

	public void invokeDestroySelfOverNet(float duration){
		Invoke ("destroySelfOverNet", duration);
	}

	private void destroySelfOverNet() {
		PhotonNetwork.Destroy (Stone_clone);
	}

	[RPC]
	public void shootSelfRPC(Vector3 direction) {
		Debug.Log("shootSelfRPC");
		Stone_clone.transform.parent = null;
		Stone_clone.rigidbody.isKinematic = false;
		Stone_clone.rigidbody.useGravity = true;
		Stone_clone.rigidbody.velocity = Speed * direction/direction.magnitude;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
