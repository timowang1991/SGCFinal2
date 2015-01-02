using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	public string flammableLayerName = "Flammable";
	public float stayBurningTime;

	//bool isOnFire;
	float onFireTimer;

	// Use this for initialization
	void Start () {
		onFireTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (particleSystem.isPlaying) {
			onFireTimer += Time.deltaTime;

			if(onFireTimer < stayBurningTime){
				fixRotation();
			} else{
				particleSystem.Stop();
			}
		}
	}

	void fixRotation(){
		Quaternion target = Quaternion.Euler (-90.0f, 0, 0);
		transform.rotation = target;
	}

	void OnTriggerEnter(Collider other){
		if(particleSystem.isPlaying){
			OnParticleCollision (other.gameObject);
		}
	}

	void OnParticleCollision(GameObject other){
//		Debug.Log ("OnParticleCollision" + other.gameObject.name + " " + other.gameObject.layer);
		if(other.layer == LayerMask.NameToLayer(flammableLayerName)){
			Fire otherFire = other.GetComponentInChildren<Fire>();
			if(otherFire == null)
				return;
			otherFire.caughtFire();
		}

	}

	// called by other Fire objects
	public void caughtFire(){
		if(particleSystem.isPlaying){
			onFireTimer = 0.0f;
			return;
		}
		onFireTimer = 0.0f;
		particleSystem.Play ();
	}
}
