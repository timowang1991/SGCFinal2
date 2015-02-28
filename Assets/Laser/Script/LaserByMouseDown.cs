using UnityEngine;
using System.Collections;

public class LaserByMouseDown : MonoBehaviour {
	
	public GameObject laserHitBurst;

	public float rayDistance = 100f;
	public float addForceRatio = 5.0f;
	public float laserHitEffectTimerToDisappear = 7.0f;

	LineRenderer line;
	Light light;
	
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
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
	}
	
	IEnumerator FireLaser(){
		line.enabled = true;
		light.enabled = true;
		
		while(Input.GetButton("Fire1")){
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
