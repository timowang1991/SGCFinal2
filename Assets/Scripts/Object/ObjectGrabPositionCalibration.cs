using UnityEngine;
using System.Collections;

public class ObjectGrabPositionCalibration : MonoBehaviour {
	
	public Vector3 positionOffset;
	public Quaternion localRotationOffset;
	public string grabbedBodyPartTag;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision collision){
//		Debug.Log("OnTriggerStay : " + other.gameObject.name);
		if (collision.gameObject.tag == grabbedBodyPartTag) {
			transform.position = collision.transform.position + positionOffset;
//			transform.localRotation = other.transform.localRotation + localRotationOffset;
		}
	}
}
