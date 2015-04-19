using UnityEngine;
using System.Collections;

public class HurtByLaser : MonoBehaviour {

	public float timeInterval = 1.0f;
	private float nowTime;
	public int hurtValue = 30;


	void Start()
	{
		nowTime = Time.time;
	}

	[RPC]
	void hurtByLaser()
	{
		if(Time.time > nowTime)
		{
			this.GetComponent<HealthSystem>().damage(hurtValue);
			this.GetComponent<Fire>().caughtFire();
			nowTime = Time.time + timeInterval;
		}
	}
}
