using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	public GameObject laserHitBurst;

	LineRenderer line;
	Light light;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		line.enabled = false;

		light = GetComponent<Light>();
		light.enabled = false;

		Screen.lockCursor = true;
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
//			line.renderer.material.mainTextureOffset = new Vector2(0, Time.time);

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

			line.SetPosition(0, ray.origin);
			if(Physics.Raycast(ray, out hit, 100)){
				line.SetPosition(1, hit.point);
				if(hit.rigidbody){
					hit.rigidbody.AddForceAtPosition(transform.forward * 5, hit.point);
				}

				Instantiate(laserHitBurst, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
			}
			else
				line.SetPosition(1, ray.GetPoint(100));

			yield return null;
		}
		light.enabled = false;
		line.enabled = false;
	}
}
