using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	public string flammableLayerName = "Flammable";
	public float stayBurningTime;

	public GameObject FireOnHeroObj;
	private ParticleSystem FireOnHeroPartical;

	//bool isOnFire;
	float onFireTimer;

	// Use this for initialization
	void Start () {
		onFireTimer = 0.0f;
		FireOnHeroPartical = FireOnHeroObj.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (FireOnHeroPartical.isPlaying) {
			onFireTimer += Time.deltaTime;

			if(onFireTimer < stayBurningTime){
//				fixRotation();
			} else{
				FireOnHeroPartical.Stop();
			}
		}
	}

//	void fixRotation(){
//		Quaternion target = Quaternion.Euler (-90.0f, 0, 0);
//		transform.rotation = target;
//	}
//
	void OnTriggerEnter(Collider other){
		if(other.gameObject.layer == LayerMask.NameToLayer(flammableLayerName)){
//			Fire otherFire = other.GetComponentInChildren<Fire>();
//			if(otherFire == null)
//				return;
			caughtFire();
		}
	}

//	void OnParticleCollision(GameObject other){
//		//Debug.Log (other.name);
////		Debug.Log ("OnParticleCollision" + other.gameObject.name + " " + other.gameObject.layer);
//
//
//	}

	// called by other Fire objects
	public void caughtFire(){
		if(FireOnHeroPartical.isPlaying){
			if(this == null)
			{
				onFireTimer = 0.0f;
			}
			return;
		}
		onFireTimer = 0.0f;
		FireOnHeroPartical.Play ();
	}
}
