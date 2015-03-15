using UnityEngine;
using System.Collections;

public class MagicianThrowFireBall : Photon.MonoBehaviour {

	public int CostMP = 10; 
	public string FireBall_Prefab = "FireBall_Magician_Net";
	private Transform camTrans;

	void Start()
	{
		if (FireBall_Prefab == null) {
			Debug.LogError("NO FireBall_Prefab Loaded");
		}
		camTrans = Camera.main.transform;
	}

	public void CastSpell()
	{

		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
		}
	}
	[RPC]
	void FireTheFireBall()
	{
		CapsuleCollider cc = this.gameObject.collider as CapsuleCollider;
		Vector3 v3 = camTrans.forward / camTrans.forward.magnitude;
		GameObject fireBall = PhotonNetwork.Instantiate (FireBall_Prefab, this.gameObject.transform.position, Quaternion.identity, 0);
		fireBall.transform.LookAt ( camTrans.position,Vector3.forward);
		Debug.Log(this.photonView.viewID + " is Firing the Fire ball");
	}

}
