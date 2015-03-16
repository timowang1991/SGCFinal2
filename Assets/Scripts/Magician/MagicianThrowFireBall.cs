using UnityEngine;
using System.Collections;

public class MagicianThrowFireBall : Photon.MonoBehaviour {

	public int CostMP = 10; 
	public string FireBall_Prefab = "FireBall_Magician_Net";
	private Transform camTrans;
	public Transform MagicianPosition;
	public Transform FireBallRef;

	private float time = 0;

	void Start()
	{
		if (FireBall_Prefab == null) {
			Debug.LogError("NO FireBall_Prefab Loaded");
		}
		camTrans = Camera.main.transform;
		Debug.Log ("CamName:" + camTrans.name);
	}
	void Update()
	{
		time += Time.deltaTime;
	}
	public void CastSpell()
	{

		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP) && time > 1.0) {
			this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
			time = 0;
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
