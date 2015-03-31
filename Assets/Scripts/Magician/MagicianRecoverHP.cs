using UnityEngine;
using System.Collections;

public class MagicianRecoverHP : MonoBehaviour {
	
	public int CostMP = 10;
	public int DefaultRecoveryHPvalue = 5;
	private MagicianHPPlayersIsInRange HPRange;
	public GameObject RecoverHPFX;

	// Use this for initialization
	void Start () {
		HPRange = transform.Find("HPHealthRange").GetComponent<MagicianHPPlayersIsInRange>();
	}
	
	public void CastSpell()
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {

			foreach(GameObject currentGameObject in HPRange.GameObjectsWithInRange)
			{
				Debug.Log(currentGameObject.name);
				PhotonView pv = currentGameObject.GetPhotonView();
				pv.RPC("RecoverHP", PhotonTargets.All, DefaultRecoveryHPvalue);
			}
			RecoverHPFX.SetActive(true);
			Invoke("TurnOffHPFX",3);
			this.gameObject.GetPhotonView().RPC("RecoverHP", PhotonTargets.All, DefaultRecoveryHPvalue);
		}
	}

	void TurnOffHPFX ()
	{
		RecoverHPFX.SetActive(false);
	}
}
