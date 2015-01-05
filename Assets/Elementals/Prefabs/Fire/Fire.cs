using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	public string flammableLayerName = "Flammable";
	public float stayBurningTime;
	public GameObject creature = null;
	public int fireDamageOnHero;
	private HealthSystem healthSys;

	//bool isOnFire;
	float onFireTimer;

	void Awake () {
		if(creature != null) {
			healthSys = creature.GetComponent<HealthSystem>();
			if(healthSys != null) {
				Debug.Log("Fire script attach to creature with HP");
			}
			else {
				Debug.Log ("Fire script attach to creature with NO HP");
			}
		}
	}

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
	private void caughtFire(){
		if(particleSystem.isPlaying){
			if(creature == null)
			{
				onFireTimer = 0.0f;
			}
			return;
		}
		onFireTimer = 0.0f;
		particleSystem.Play ();
		if(healthSys != null) {
			healthSys.damage(fireDamageOnHero);	
		}
	}
}
