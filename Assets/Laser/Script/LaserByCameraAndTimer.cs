using UnityEngine;
using System.Collections;

public class LaserByCameraAndTimer : MonoBehaviour {
	public GameObject laserHitBurst;
	public Camera cam;

	public float rayDistance = 100.0f;
	public float addForceRatio = 5.0f;
	public float timerToUseLaser = 5.0f;
	public float laserHitEffectTimerToDisappear = 7.0f;
	
	LineRenderer line;
	Light light;
	
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
		line.enabled = true;
		light.enabled = true;
		
		while(timerToUseLaser > 0){
			timerToUseLaser -= Time.deltaTime;
			shootLaser();						
			yield return null;
		}
		
		light.enabled = false;
		line.enabled = false;
	}

	void shootLaser(){
		RaycastHit hitInfo = new RaycastHit();

		// add more methods in here if want to add more features when hit
		if(RenderLaserAndGetLaserHitInfo(ref hitInfo)){
			InstantiateHitEffect(hitInfo);
			CreateHitForce(hitInfo);
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

	bool RenderLaserAndGetLaserHitInfo(ref RaycastHit laserHitInfo){
		Vector3 laserStartTipToCamRayHit = Vector3.forward;
		Ray ray;

		if(GetLaserStartTipToCameraRayHitVector(ref laserStartTipToCamRayHit)){
			ray = new Ray(transform.position, laserStartTipToCamRayHit);
		} else {
			ray = new Ray(transform.position, cam.transform.forward);
		}

		line.SetPosition(0, ray.origin);

		if(Physics.Raycast(ray, out laserHitInfo, rayDistance)){
			line.SetPosition(1, laserHitInfo.point);
			return true;
		} else {
			line.SetPosition(1, ray.GetPoint(rayDistance));
			return false;
		}
	}

	void InstantiateHitEffect(RaycastHit hit){
		if(laserHitBurst == null)
			return;

		GameObject gObject = Instantiate(laserHitBurst, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
		if(hit.transform){
			gObject.transform.parent = hit.transform;
		}
		Destroy(gObject, laserHitEffectTimerToDisappear);
	}

	void CreateHitForce(RaycastHit hit){
		if(hit.rigidbody){
			hit.rigidbody.AddForceAtPosition(cam.transform.forward * addForceRatio, hit.point);
		}
	}
}
