using UnityEngine;
using System.Collections;

public class LaserByCameraAndTimer : Photon.MonoBehaviour {
	public GameObject laserHitBurst;
	public Camera cam;

	public float rayDistance = 100.0f;
	public float addForceRatio = 5.0f;
	public float timerToUseLaser = 5.0f;
	public float laserHitEffectTimerToDisappear = 7.0f;
	
	LineRenderer line;
	Light light;

	RaycastHit hit;
	Ray ray;
	
	public float TimerToUseLaser {
		get{return timerToUseLaser;}
		set{
			timerToUseLaser = value;
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
	}

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		line.enabled = false;
		
		light = GetComponent<Light>();
		light.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
			TimerToUseLaser = 3.0f;
		}
	}

	IEnumerator FireLaser(){
		photonView.RPC ("RPCLaserLightsOn", PhotonTargets.All, null);
		
		while(timerToUseLaser > 0){
			timerToUseLaser -= Time.deltaTime;
			shootLaser();						
			yield return null;
		}
		
		photonView.RPC ("RPCLaserLightsOff", PhotonTargets.All, null);
	}

	void shootLaser(){
		// add more methods in here if want to add more features when hit
		if(RenderLaserAndGetLaserHitInfo()){
			photonView.RPC ("RPCInstantiateHitEffect", PhotonTargets.All, null);
			photonView.RPC ("RPCCreateHitForce", PhotonTargets.All, null);
		}
	}

	bool RenderLaserAndGetLaserHitInfo(){
		Vector3 laserStartTipToCamRayHit = Vector3.forward;
		
		if(GetLaserStartTipToCameraRayHitVector(ref laserStartTipToCamRayHit)){
			ray = new Ray(transform.position, laserStartTipToCamRayHit);
		} else {
			ray = new Ray(transform.position, cam.transform.forward);
		}
		
		if(Physics.Raycast(ray, out hit, rayDistance)){
			photonView.RPC ("RPCRenderLaserOnHit", PhotonTargets.All, null);
			return true;
		} else {
			photonView.RPC ("RPCRenderLaserOnMiss", PhotonTargets.All, null);
			return false;
		}
	}

	bool GetLaserStartTipToCameraRayHitVector(ref Vector3 laserStartTipToCamRayHit){
		if(cam == null)
			return false;

		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity)){
			laserStartTipToCamRayHit = (hit.point - transform.position).normalized;
			return true;
		}

		return false;
	}

	[RPC]
	public void RPCLaserLightsOn(){
		line.enabled = true;
		light.enabled = true;
	}

	[RPC]
	public void RPCLaserLightsOff(){
		light.enabled = false;
		line.enabled = false;
	}

	[RPC]
	public void RPCRenderLaserOnHit(){
		line.SetPosition(0, ray.origin);
		line.SetPosition(1, hit.point);
	}

	[RPC]
	public void RPCRenderLaserOnMiss(){
		line.SetPosition(0, ray.origin);
		line.SetPosition(1, ray.GetPoint(rayDistance));
	}

	[RPC]
	public void RPCInstantiateHitEffect(){
		if(laserHitBurst == null)
			return;

		GameObject gObject = Instantiate(laserHitBurst, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
		if(hit.transform){
			gObject.transform.parent = hit.transform;
		}
		Destroy(gObject, laserHitEffectTimerToDisappear);
	}

	[RPC]
	public void RPCCreateHitForce(){
		if(hit.rigidbody){
			hit.rigidbody.AddForceAtPosition(cam.transform.forward * addForceRatio, hit.point);
		}
	}

}
