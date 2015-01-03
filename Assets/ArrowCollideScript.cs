using UnityEngine;
using System.Collections;

public class ArrowCollideScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log (other.gameObject.name);
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Arrow")
		{
			ContactPoint contact = collision.contacts[0];

			//Vector3 pos = contact.point;
			collision.gameObject.transform.position = contact.point;
			collision.gameObject.rigidbody.isKinematic = true;
			collision.gameObject.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.touched;
		}

	}
}
