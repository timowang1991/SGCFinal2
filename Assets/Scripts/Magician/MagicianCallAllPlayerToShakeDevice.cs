using UnityEngine;
using System.Collections;

public class MagicianCallAllPlayerToShakeDevice : Photon.MonoBehaviour {

	public float CostMP = 50;

	public void CastSpell()
	{
		if (this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			this.photonView.RPC("ShakeDeviceToHurt", PhotonTargets.Others);
		}
	}

	[RPC]
	void ShakeDeviceToHurt()
	{
		if (photonView.isMine) {
			Debug.Log("ShakeDeviceToHurt: Self");
		}
		else
		{
			//add UI
		}
	}
}
