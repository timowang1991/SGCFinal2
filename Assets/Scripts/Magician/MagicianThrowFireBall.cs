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

		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
		}
	}

	public float factor = 200;
	[RPC]
	void FireTheFireBall()
	{
		CapsuleCollider cc = this.gameObject.collider as CapsuleCollider;
		GameObject fireBall = PhotonNetwork.Instantiate (FireBall_Prefab, MagicianPosition.position,Quaternion.identity, 0);
		fireBall.transform.LookAt ( FireBallRef ,FireBallRef.up);
		Debug.Log(this.photonView.viewID + " is Firing the Fire ball");
	}

}
