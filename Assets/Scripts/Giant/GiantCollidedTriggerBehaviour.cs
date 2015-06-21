using UnityEngine;
using System.Collections;

public class GiantCollidedTriggerBehaviour : MonoBehaviour {
	GiantHealth giantHealth;
	
	// Use this for initialization
	void Start () {
		GameObject giantObj = GameObject.Find (GameConfig.giantGameObjectName);
		giantHealth = giantObj.GetComponent<GiantHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)col.gameObject.GetComponent(typeof(IDamageOthersBehaviour));
		if (damagingBehaviour != null) {
			giantHealth.damage (col.gameObject);
		}
	}
}
