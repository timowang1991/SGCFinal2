using UnityEngine;
using System.Collections;

public class MagicianRecoverHP : MonoBehaviour {
	
	public int CostMP = 10;
	public int DefaultRecoveryHPvalue = 5;

	// Use this for initialization
	void Start () {
	}
	
	public void CastSpell(int photonViewId)
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			PhotonView.Find(photonViewId).RPC("RecoverHP", PhotonTargets.All, DefaultRecoveryHPvalue);
		}
	}
}
