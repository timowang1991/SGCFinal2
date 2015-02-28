using UnityEngine;
using System.Collections;

public class MagicianThrowFireBall : Photon.MonoBehaviour {

	public int CostMP = 10; 

	public void CastSpell(int photonViewId)
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
		}
	}
	[RPC]
	void FireTheFireBall()
	{
		Debug.Log(this.photonView.viewID + " is Firing the Fire ball");
	}

}
