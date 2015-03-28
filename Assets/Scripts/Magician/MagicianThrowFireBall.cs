using UnityEngine;
using System.Collections;

public class MagicianThrowFireBall : Photon.MonoBehaviour {

	public int CostMP = 10; 
	public string FireBall_Prefab = "FireBall_Magician_Net";
	private Transform camTrans;
	public Transform MagicianPosition;
	public Transform FireBallRef;

	void Start()
	{
		if (FireBall_Prefab == null) {
			Debug.LogError("NO FireBall_Prefab Loaded");
		}
		camTrans = Camera.main.transform;
		Debug.Log ("CamName:" + camTrans.name);
	}
	public void CastSpell()
	{

		if ( photonView.isMine && this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			//this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
			GameObject fireBall = PhotonNetwork.Instantiate (FireBall_Prefab, MagicianPosition.position,camTrans.rotation, 0);
		}
	}

//	public float factor = 200;
//	[RPC]
//	void FireTheFireBall()
//	{
//		CapsuleCollider cc = this.gameObject.collider as CapsuleCollider;
//
//		
//		Debug.Log(this.photonView.viewID + " is Firing the Fire ball");
//	}

}
