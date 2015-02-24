using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GiantGrabAndReleaseScript : MonoBehaviour {

	
	public float speedValveToThrow = 350.0f;
	
	public float ratio = 2.0f;
	
	public float ratioForHandToEyeTarget = 1000.0f;
	
	public int maxNumOfObjectsOnHand = 1;
	
	string grabbableLayerName = "Grabbable";
	
	List<Collider> colliderList = new List<Collider>();
	
	List<Vector3> positions = new List<Vector3> ();
	
	Vector3 displacement;
	private Platform platform; 
	
	Camera ovrCamera;
	int ovrCamRayLayer;
	
	// Use this for initialization
	void Start () {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if (platform == Platform.PC_Giant)
		{
			ovrCamera = GameObject.FindGameObjectWithTag("OVR_CenterEye").GetComponent<Camera>();
			ovrCamRayLayer = LayerMask.NameToLayer("Terrain");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		recordPosition ();
//		computeVelocity ();
//		throwObjects ();
	}
	
	void recordPosition(){
		positions.Add (transform.position);
		if (positions.Count > 10) {
			positions.RemoveAt(0);
		}
	}
	
	void computeVelocity(){
		displacement = transform.position - positions[0];
		//		Debug.Log ("displacement : " + displacement);
		//		Debug.Log ("displacement magnitude : " + displacement.magnitude);
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log("Giant OnCollisionEnter : object " + collision.gameObject.name);
		collider.isTrigger = true;
	}

	void OnCollisionExit(Collision collision){
		Debug.Log("Giant OnCollisionExit : object " + collision.gameObject.name);
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("Giant OnTriggerEnter : object " + other.gameObject.name);
	}
	
	
	void OnTriggerExit(Collider other){
		Debug.Log("Giant OnTriggerExit : object " + other.gameObject.name);
		collider.isTrigger = false;
	}
	
	void throwObjects(){
		if (colliderList.Count == 0)
			return;
		
		if(displacement.magnitude >= speedValveToThrow &&
		   displacement.y < 0 &&
		   displacement.z > 0){
			foreach(Collider collider in colliderList){
				if(collider.gameObject.tag == "BigTreeTrunk")
				{
					collider.gameObject.GetComponent<TreeLifeCycle>().DetachPoint();
				}
				//colliderList.Remove (collider);
				collider.rigidbody.useGravity = true;
				collider.rigidbody.isKinematic = false;
				//				collider.rigidbody.velocity = displacement * ratio;
				
				Vector3 handToEyeTarget = Vector3.up;
				if(getHandToEyeTargetDirection(ref handToEyeTarget)){
					collider.rigidbody.AddForce(handToEyeTarget * ratioForHandToEyeTarget * displacement.magnitude);
				} else {
					collider.rigidbody.AddForce(displacement * ratio);
				}
			}
		}
	}
	
	bool getHandToEyeTargetDirection(ref Vector3 handToEyeTarget){
		if(ovrCamera == null)
			return false;
		
		//Debug.Log("rayOrigin : " + ovrCamera.transform.position + " rayDirection : " + ovrCamera.transform.forward);
		
		RaycastHit floorHit;
		if(Physics.Raycast(ovrCamera.transform.position, ovrCamera.transform.forward,
		                   out floorHit, Mathf.Infinity)){
			//Debug.Log("floorHit : " + floorHit.point);
			handToEyeTarget = (floorHit.point - transform.position).normalized;
			return true;
		}
		
		return false;
	}
}
