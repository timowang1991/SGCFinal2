using UnityEngine;
using System.Collections;

public class MagicianChangeHand : MonoBehaviour {

	private PhotonView giant;
	public int CostMP;

	// Use this for initialization
	void Start () {
		giant = GameObject.Find ("52bangd-T-Pose").GetComponent<PhotonView>();
		if (giant == null) {
			Debug.LogError("Giant canoot find");
		}
	}

	public void CastSpell()
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			giant.RPC ("ChanageHands", PhotonTargets.MasterClient, true);
		}
	}
}