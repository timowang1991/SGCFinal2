using UnityEngine;
using System.Collections;

public class GiantCollidedBehaviour : MonoBehaviour {

	GiantHealth giantHealth;


	// Use this for initialization
	void Start () {
		GameObject giantObj = GameObject.Find (GameConfig.giantGameObjectName);
		giantHealth = giantObj.GetComponent<GiantHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)collision.gameObject.GetComponent(typeof(IDamageOthersBehaviour));
		if (damagingBehaviour != null) {
			giantHealth.damage (collision.gameObject);
		}
	}

}
