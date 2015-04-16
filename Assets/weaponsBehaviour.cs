using UnityEngine;
using System.Collections;

public class weaponsBehaviour : MonoBehaviour {

	public int forceIntensity = 100;
	public Transform launchTrams;
	public Transform towardTrans;
	public GameObject rocketPrefab;
	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			Vector3 launchDir = (towardTrans.position - launchTrams.position).normalized;
			Rigidbody rocket = Instantiate(rocketPrefab, launchTrams.position, Quaternion.identity) as Rigidbody;
			rocket.rotation = Quaternion.FromToRotation(rocket.transform.forward,launchDir);
			rocket.AddForce(launchDir * forceIntensity);
		}

		if (Input.GetKey ("Fire1")) {


		}
	}


	void launchRocket() {
	
	}

}
