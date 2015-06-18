using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GiantGrabAndReleaseObject : Photon.MonoBehaviour {

	public float speedValveToThrow = 100.0f;
	
	public float ratioForDisplacement = 2.0f;
	
	public float ratioForHandToEyeTarget = 1000.0f;
	
	public int maxNumOfObjectsOnHand = 1;

	List<GameObject> grabbedObjectList = new List<GameObject>();
	
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
		recordPosition ();
		computeVelocity ();
		throwObjects ();
//		Debug.Log("Giant grabbed Object List Count : " + grabbedObjectList.Count);
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
		GrabAnObjectOfGameObject(collision.gameObject);
	}
	
	void OnCollisionExit(Collision collision){
//		Debug.Log("Giant OnCollisionExit : object " + collision.gameObject.name);
	}
	
	void OnTriggerEnter(Collider other){
//		Debug.Log ("Giant OnTriggerEnter : object " + other.gameObject.name);
		GrabAnObjectOfGameObject(other.gameObject);
	}
	
	void OnTriggerExit(Collider other){
//		Debug.Log("Giant OnTriggerExit : object " + other.gameObject.name);
		if(grabbedObjectList.Contains(other.gameObject)){
			grabbedObjectList.Remove(other.gameObject);
			other.gameObject.GetComponent<ObjectGrabAndRelease>().ReleaseObject();
		}

		if (grabbedObjectList.Count == 0){
			photonView.RPC ("RPCGiantDidReleaseAllObjects", PhotonTargets.All, null);
		}
	}

	void GrabAnObjectOfGameObject(GameObject gObject){
		ObjectGrabAndRelease objectGrabAndRelease = gObject.GetComponent<ObjectGrabAndRelease>();

		if(objectGrabAndRelease != null &&
		   grabbedObjectList.Count < maxNumOfObjectsOnHand &&
		   objectGrabAndRelease.GrabObjectByGameObject(gameObject)){
			photonView.RPC ("RPCGiantDidGrabObject", PhotonTargets.All, null);
			grabbedObjectList.Add(gObject);
//			Debug.Log("grabbedObjectList Added new Object : " + gObject.name);
		}
	}

	void throwObjects(){
		if (grabbedObjectList.Count == 0)
			return;
		
		if(displacement.magnitude >= speedValveToThrow &&
		   /*displacement.y < 0 &&*/
		   displacement.z > 0){
//			Debug.Log("displacement.magnitude = " + displacement.magnitude);
			foreach(GameObject gObject in grabbedObjectList){
				gObject.GetComponent<ObjectGrabAndRelease>().ReleaseObject();

				Vector3 handToEyeTarget = Vector3.up;
				if(getHandToEyeTargetDirection(ref handToEyeTarget)){
					gObject.rigidbody.AddForce(handToEyeTarget * ratioForHandToEyeTarget * displacement.magnitude);
				} else {
					gObject.rigidbody.AddForce(displacement * ratioForDisplacement);
//					gObject.rigidbody.velocity = displacement * ratio;
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
	
	[RPC]
	public void RPCGiantDidGrabObject(){
		collider.isTrigger = true;
	}
	
	[RPC]
	public void RPCGiantDidReleaseAllObjects(){
		collider.isTrigger = false;
	}

}
