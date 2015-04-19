using UnityEngine;
using System.Collections;

public class LaserByCameraAndTimer : Photon.MonoBehaviour {
	public GameObject laserHitBurst;
//	public Camera cam;
	public GameObject oculusCamRef;

	public float rayDistance = 100.0f;
	public float explosionForceRadius = 10.0f;
	public float explosionForce = 30.0f;
	public float explosionForceUpwardModifier = 0.0f;
//	public float addForceRatio = 5.0f;
	public float timerToUseLaser = 5.0f;
	public float laserHitEffectTimerToDisappear = 7.0f;

	public int healthDamage = 30;

	LineRenderer line;
	Light light;

	RaycastHit hit;
	Ray ray;

	Platform platform;

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
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		line = GetComponent<LineRenderer>();
		line.enabled = false;
		
		light = GetComponent<Light>();
		light.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && platform == Platform.PC_Giant){
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
			photonView.RPC ("RPCInstantiateHitEffect", PhotonTargets.All, hit.point);
			hitObjectBehavior(hit.point);
		}
	}

	bool RenderLaserAndGetLaserHitInfo(){
		Vector3 laserStartTipToCamRayHit = Vector3.forward;
		
		if(GetLaserStartTipToCameraRayHitVector(ref laserStartTipToCamRayHit)){
			ray = new Ray(transform.position, laserStartTipToCamRayHit);
		} else {
			ray = new Ray(transform.position, oculusCamRef.transform.forward);
		}
		
		if(Physics.Raycast(ray, out hit, rayDistance)){
			photonView.RPC ("RPCRenderLaserOnHit", PhotonTargets.All, hit.point);
			return true;
		} else {
			photonView.RPC ("RPCRenderLaserOnMiss", PhotonTargets.All, null);
			return false;
		}
	}

	bool GetLaserStartTipToCameraRayHitVector(ref Vector3 laserStartTipToCamRayHit){
		if(oculusCamRef == null)
			return false;

		RaycastHit hit;
		if(Physics.Raycast(oculusCamRef.transform.position, oculusCamRef.transform.forward, out hit, Mathf.Infinity)){
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
	public void RPCRenderLaserOnHit(Vector3 hitPoint){
		line.SetPosition(0, transform.position);
		line.SetPosition(1, hitPoint);

	}

	[RPC]
	public void RPCRenderLaserOnMiss(){
		line.SetPosition(0, transform.position);
		line.SetPosition(1, oculusCamRef.transform.position + oculusCamRef.transform.forward.normalized * rayDistance);
	}

	[RPC]
	public void RPCInstantiateHitEffect(Vector3 hitPoint){
		if(laserHitBurst == null)
			return;

		GameObject gObject = Instantiate(laserHitBurst, hitPoint, Quaternion.FromToRotation(Vector3.forward, Vector3.up)) as GameObject;
//		if(hit.transform){
//			gObject.transform.parent = hit.transform;
//		}
		Destroy(gObject, laserHitEffectTimerToDisappear);
	}

	public void hitObjectBehavior(Vector3 hitPoint){
//		if(hit.rigidbody){
//			hit.rigidbody.AddForceAtPosition(oculusCamRef.transform.forward * addForceRatio, hit.point);
//		}
		Collider [] colliders = Physics.OverlapSphere(hitPoint, explosionForceRadius);
		foreach(Collider collider in colliders){
			PhotonView pView = collider.transform.root.gameObject.GetComponent<PhotonView>();
			if(!pView) continue;

			if(collider && collider.rigidbody){
				photonView.RPC("RPCCreateHitForce", PhotonTargets.All, pView.viewID, hitPoint);
			}

//			HealthSystem healthSystem = PhotonView.Find(pView.viewID).gameObject.GetComponent<HealthSystem>();
			if(collider.transform.root.gameObject.tag == "Player"){
				Debug.Log("hurtByLaser call to "+ pView.viewID);
				pView.RPC("hurtByLaser",pView.owner);
			}
		}
	}

	[RPC]
	public void RPCCreateHitForce(int viewID, Vector3 hitPoint){
		GameObject hitObject = PhotonView.Find(viewID).gameObject;
		if(!hitObject) return;

		hitObject.rigidbody.AddExplosionForce(explosionForce, hitPoint, explosionForceRadius, explosionForceUpwardModifier);
	}

//	[RPC]
//	public void 
}
