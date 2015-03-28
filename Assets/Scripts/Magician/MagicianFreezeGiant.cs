using UnityEngine;
using System.Collections;

public class MagicianFreezeGiant : MonoBehaviour {

	private PhotonView giant;
	public int CostMP = 50;
	public int HowLongItLast = 3;
	// Use this for initialization
	void Start () {
		giant = GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<PhotonView>();
		if (giant == null) {
			Debug.LogError("Giant canoot find");
		}
	}

	public void CastSpell()
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			giant.RPC ("FreezeGiant", PhotonTargets.MasterClient, HowLongItLast);
		}
	}
}
