using UnityEngine;
using System.Collections;

public class HurtByLaser : MonoBehaviour {

	public int hurtValue = 30;

	[RPC]
	void hurtByLaser()
	{
		this.GetComponent<HealthSystem>().damage(hurtValue);
		this.GetComponent<Fire>().caughtFire();
	}
}
