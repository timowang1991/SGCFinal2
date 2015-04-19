using UnityEngine;
using System.Collections;

public class MagicianThrowTornado : Photon.MonoBehaviour {

	public int CostMP = 10; 
	public string Tornado_Prefab = "MagicianTornado";
	private Transform camTrans;
	public Transform TornadoPosition;
	
	void Start()
	{
		if (Tornado_Prefab == null) {
			Debug.LogError("NO MagicianTornado Loaded");
		}
		camTrans = Camera.main.transform;
		Debug.Log ("CamName:" + camTrans.name);
	}
	public void CastSpell()
	{
		
		if ( photonView.isMine && this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			//this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
			object[] data = new object[1];
			data[0] = photonView.viewID;
			GameObject Tornado = PhotonNetwork.Instantiate (Tornado_Prefab, TornadoPosition.position,Quaternion.identity,0,data);
//			Tornado.GetComponent<TornadoSelfScript>().PlayerPosition = this.transform;
//			Tornado.GetComponent<TornadoSelfScript>().PlayerViewId = photonView.viewID;
		}
	}
}
