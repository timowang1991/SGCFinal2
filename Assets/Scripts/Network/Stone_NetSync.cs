using UnityEngine;
using System.Collections;

public class Stone_NetSync : Photon.MonoBehaviour {

	[RPC]
	public void initRPC(int viewID) {
		GameObject cataGameObject = PhotonView.Find (viewID).gameObject;
		Transform stonePosTrans = cataGameObject.transform.FindChild("StonePosition");
		transform.parent = stonePosTrans;
	}

	// Use this for initialization
	void Start () {
		if (photonView.instantiationData != null) {
			int cataViewID = (int)photonView.instantiationData[0];
			GameObject cataGameObject = PhotonView.Find (cataViewID).gameObject;
			Transform stonePosTrans = cataGameObject.transform.Find("Catapult_Bone_Main/Catapult_Bone_03/StonePosition");
			transform.parent = stonePosTrans;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
