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
		if(transform.parent != null && transform.parent.gameObject.tag == grabbedBodyPartTag){
			transform.position = transform.parent.position + positionOffset;
			transform.localRotation = localRotationOffset;
		}
	}
}
