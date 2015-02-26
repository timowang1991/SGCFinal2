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

	void OnTriggerStay(Collider other){
//		Debug.Log("OnTriggerStay : " + other.gameObject.name);
		if (other.gameObject.tag == grabbedBodyPartTag) {
			transform.position = other.transform.position + positionOffset;
//			transform.localRotation = other.transform.localRotation + localRotationOffset;
		}
	}
}
