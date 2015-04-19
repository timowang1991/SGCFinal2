using UnityEngine;
using System.Collections;

public class CopterDamagedBehaviour : Photon.MonoBehaviour,IDamagedBehaviour {

	public AudioClip sparksound;
	public GameObject detonator;
	public GameObject explosion;
	public GameObject rotor;
	public GameObject refr;
	public GameObject particlefire;
	public GameObject scorchMarkPrefab;
	public GameObject brokenfire;
	public Texture normalTexture;
	public Texture burnTexture;
	public Material mainMaterial;
	
	//[HideInInspector]
	//public bool isControllable = false;

	[HideInInspector]
	public bool state = false;
	[HideInInspector]
	public HealthSystem hpSys = null;

	private HelifinNet helifin;
	private float DamageFactor = 0.3f;
	private float Dieingtimer = 0;
	private float Deathtimer = 0;
	private Rigidbody rigidbody;
	private int mainrotor_axis = 3;
	private bool isOnFire = false;

	void Awake() {
		particlefire.SetActive (false);
		mainMaterial.mainTexture = normalTexture;
		helifin = GetComponent<HelifinNet> ();
		//hpSys = GetComponent<HealthSystem> (); 
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update() {
		if (state) {
			falling ();
		}
	}

	[RPC]
	void copterOnFire() {
		brokenfire.SetActive(true); //on fire
	}

	[RPC]
	void copterStartFalling() {
		state = true; //start to fall
		helifin.isInSpark = true;
		particlefire.SetActive(true);
	}

	string IDamagedBehaviour.getDamagedClass() {
		return "Machine";
	}

	void IDamagedBehaviour.damaged(IDamageOthersBehaviour damagingBehaviour, int currentHP) {
		if( currentHP < 20 && !isOnFire) {
			isOnFire = true;
			photonView.RPC (GameConfig.GetFunctionName(copterOnFire), PhotonTargets.All, null);
		}
	}

	void IDamagedBehaviour.dying(IDamageOthersBehaviour damagingBehaviour) {
		photonView.RPC (GameConfig.GetFunctionName(copterStartFalling), PhotonTargets.All, null);
	}

	void falling() {

		switch(mainrotor_axis) {	
			case 1:
				rotor.transform.Rotate(1000*Random.value,0,0);
				break;
			case 2:
				rotor.transform.Rotate(0,1000*Random.value,0);
				break;
			case 3:
				rotor.transform.Rotate(0,0,1000*Random.value);
				break;
		}

		if (hpSys != null) {
			rigidbody.useGravity = true;
			rigidbody.AddForce (Vector3.down * 10000);
			audio.pitch = Random.Range (0.1f, 0.6f);
		}

		Deathtimer += Time.deltaTime;
		if (Deathtimer > 10 || helifin.altitude < 2 || Vector3.Angle(refr.transform.up,Vector3.down)<30) {
			explode();
		}
	}

	void explode() {

		if (hpSys != null) {
			audio.Stop();
			rigidbody.AddExplosionForce(427600,transform.position,100);
		}

		Instantiate(detonator,explosion.transform.position,transform.rotation);
		mainMaterial.mainTexture=burnTexture;
		GameObject scorchMark = (GameObject)Instantiate(scorchMarkPrefab,Vector3.zero,Quaternion.identity);
		Vector3 scorchPosition = refr.collider.ClosestPointOnBounds (transform.position - Vector3.up * 100);
		scorchMark.transform.position = scorchPosition + Vector3.up * 1.1f;
		scorchMark.transform.eulerAngles = new Vector3(scorchMark.transform.eulerAngles.x, Random.Range (0.0f, 90.0f), scorchMark.transform.eulerAngles.z);
		if (hpSys != null) {
			Destroy (hpSys);
		}
		Destroy (this);
	}
	
	void OnCollisionEnter(Collision collision) {
		firstDamage (collision.gameObject);
	}

	void OnTriggerEnter(Collider collider) {
		firstDamage (collider.gameObject);
	}
	
	void OnCollisionStay(Collision collision) {
		continuousDamage (collision.gameObject);
	}

	void OnTriggerStay(Collider collider) {
		continuousDamage (collider.gameObject);
	}
	
	void firstDamage(GameObject objCausingDamage) {
		if (hpSys != null) {
			if(helifin.altitude > 2.4) {
				audio.PlayOneShot(sparksound);
				hpSys.damage ((int)(20*DamageFactor));
				Dieingtimer=0;
			}
		}
	}

	void continuousDamage(GameObject objCausingDamage) {
		if (hpSys != null) {
			Dieingtimer += Time.deltaTime;
			if(helifin.altitude > 2.4 && Dieingtimer > 1) {
				hpSys.damage ((int)(10*DamageFactor));
				Dieingtimer = 0;
			}
		}
	}

}

/*

void FixedUpdate() {
	if(isControllable && state==true){
		rigidbody.AddRelativeTorque(0,30000,0);
		rigidbody.AddForce(0,-15000,0);
	}
}

 */