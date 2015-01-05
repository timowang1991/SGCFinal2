using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabScript : MonoBehaviour {

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
	void Update () {
		recordPosition ();
		computeVelocity ();
//		holdingObjects();
		throwObjects ();
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

//	void holdingObjects(){
//		foreach(Collider collider in colliderList){
//			collider.transform.position = transform.position;
//		}
//	}

	void OnTriggerEnter(Collider other){
		//Debug.Log ("OnTriggerEnter : object" + other.gameObject.name);
		if (other.gameObject.layer == LayerMask.NameToLayer (grabbableLayerName) &&
		    colliderList.Count < maxNumOfObjectsOnHand) {
			Debug.Log("OnTriggerEnter : " + other.gameObject.name + "after if");
			colliderList.Add (other);
//			other.transform.parent = this.transform;

			if(other.gameObject.tag == "BigTreeTrunk")
			{
//				bool isExist = false;
//				//Debug.Log("BigTreeTrunkAttach");
//				foreach(Collider collide in colliderList)
//				{
//					if(collide.gameObject.GetComponent<PhotonView>().viewID == other.gameObject.GetComponent<PhotonView>().viewID)
//					{
//						isExist = true;
//					}
//				}
////				Debug.Log
//				if(!isExist)
//				{
					Debug.Log("Attach to "+this.gameObject.tag +" ID: " +other.gameObject.GetComponent<PhotonView>().viewID);
					other.gameObject.GetComponent<TreeLifeCycle>().AttachPoint(this.gameObject.tag);
//				}
			}
			else if(other.tag == "Player") {
				other.rigidbody.AddForce(displacement * ratio);
			}
			//other.transform.position = this.transform.position;
//			other.rigidbody.isKinematic = true;
//			other.rigidbody.useGravity = false;
		}
	}


	void OnTriggerExit(Collider other){
		if(!colliderList.Contains(other)){
			return;
		}

		if(other.gameObject.layer == LayerMask.NameToLayer(grabbableLayerName)){
			colliderList.Remove (other);
//			other.transform.parent = null;
			other.rigidbody.useGravity = true;
			other.rigidbody.isKinematic = false;
			if(other.gameObject.tag == "BigTreeTrunk")
			{
				other.gameObject.GetComponent<TreeLifeCycle>().DetachPoint();
			}
		}
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
					collider.rigidbody.AddForce(handToEyeTarget * ratioForHandToEyeTarget);
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
			return true;;
		}

		return false;
	}
}
