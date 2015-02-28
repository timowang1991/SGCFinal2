using UnityEngine;
using System.Collections;

public class LaserByTimer : MonoBehaviour {

	public GameObject laserHitBurst;
	
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

	}
	
	IEnumerator FireLaser(){
		line.enabled = true;
		light.enabled = true;
		
		while(timerToUseLaser > 0){
			timerToUseLaser -= Time.deltaTime;
			ShootLaser();
			yield return null;
		}
		
		light.enabled = false;
		line.enabled = false;
	}

	void ShootLaser(){
		RaycastHit hitInfo = new RaycastHit();

		// add more methods in here if want to add more features when hit
		if(RenderLaserAndGetLaserHitInfo(ref hitInfo)){
			InstantiateHitEffect(hitInfo);
			CreateHitForce(hitInfo);
		}
	}

	bool RenderLaserAndGetLaserHitInfo(ref RaycastHit laserHitInfo){
		Ray ray = new Ray(transform.position, transform.forward);
		
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
		Destroy(gObject, laserHitEffectTimerToDisappear);
	}
	
	void CreateHitForce(RaycastHit hit){
		if(hit.rigidbody){
			hit.rigidbody.AddForceAtPosition(transform.forward * addForceRatio, hit.point);
		}
	}
}
