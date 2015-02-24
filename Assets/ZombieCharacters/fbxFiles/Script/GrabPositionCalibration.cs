using UnityEngine;
using System.Collections;

public class GrabPositionCalibration : MonoBehaviour {

	public Vector3 positionOffset;
	public Quaternion localRotation;
	public string objectTagName;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		//Debug.Log ("Grab OnTriggerEnter : " + other.gameObject.tag);
		if (other.gameObject.tag == objectTagName) {
			//Debug.Log("Grab" + other.gameObject.name +"/"+other.gameObject.tag);
			other.transform.position = transform.position + positionOffset;
			other.transform.localRotation = localRotation;
		}
	}
	
	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == objectTagName) {
			other.transform.position = transform.position + positionOffset;
			other.transform.localRotation = localRotation;
		}
	}
}
