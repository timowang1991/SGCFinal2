using UnityEngine;
using System.Collections;
using HeroCameraAlias = HeroCamera_New;
public class CatapultsController : MonoBehaviour {

	Animator Catapults_animator;
	public GameObject player;
	public float distance;
	public GameObject Stone;


	public bool isLoaded = false;
	public bool isPlayerInside = false;
	int PlayerAround = Animator.StringToHash("PlayerAround");
	static int LoadedState = Animator.StringToHash("Base Layer.loaded");
	static int idleState = Animator.StringToHash("Base Layer.idle");

	public float ShootTime;
	public float Speed;
	public Transform eye;
	public Transform TargetPoint;
	Transform Cam;
	bool notToLoad = false;

	public float range;

	public enum PlayerState
	{
		PlayerInside,
		NoPlayer
	}
	public PlayerState playstate = PlayerState.NoPlayer;
	
	void OnTriggerEnter(Collider other) {
		Debug.Log ("enter"+other.gameObject.name);
		if(playstate == PlayerState.NoPlayer && other.tag == "Player")
		{
			player = other.gameObject;
			playstate = PlayerState.PlayerInside;
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("exit");
		//player = null;
		playstate = PlayerState.NoPlayer;
	}


	public void Shoot_Stone()
	{
		if(Stone_clone == null) {
			return;
		}
		Stone_clone.transform.parent = null;
		Stone_clone.rigidbody.isKinematic = false;
		Stone_clone.rigidbody.useGravity = true;
		//Vector3 tmp = new Vector3 (eye.transform.position.x + Random.Range(-range,range), eye.transform.position.y + Random.Range(-range,range), eye.transform.position.z + Random.Range(-range,range));
		//Vector3 tmp = new Vector3(TargetPoint.transform.position.x)
		Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		Vector3 test =  TargetPoint.transform.position - Cam.position;
		Stone_clone.rigidbody.velocity = Speed * test/test.magnitude;
		Destroy (Stone_clone, 10);
		Stone_clone = null;
	}
	
	// Use this for initialization
	void Start () {
		Catapults_animator = GetComponent<Animator>();
		//player = GameObject.FindGameObjectWithTag("Player");
	}

	[HideInInspector]
	public GameObject Stone_clone;
	// Update is called once per frame
	void Update () {
		//Debug.Log (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash);

		if (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash == LoadedState && isLoaded == false && Stone_clone==null) {

		    Stone_clone =  (GameObject)Instantiate (Stone, transform.Find("Catapult_Bone_Main/Catapult_Bone_03/StonePosition").position , transform.rotation);
			Stone_clone.transform.parent= transform.Find("Catapult_Bone_Main/Catapult_Bone_03/StonePosition").transform;
			isLoaded=true;
		} 

		if(Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash == idleState){
			isLoaded=false;
		}

		switch (playstate) {
			case PlayerState.PlayerInside:
				Catapults_animator.SetBool("PlayerAround",true);
				if(isPlayerInside==false)
				{
					isPlayerInside = true;
					player.GetComponent<HeroCameraAlias>().Catapults_notify(this.gameObject, isPlayerInside);
				}
				/*
				if(Distance() > distance)
				{
					playstate = PlayerState.NoPlayer;
				}
				*/
				break;
			case PlayerState.NoPlayer:
				notToLoad = true;
				Invoke("notToLoad_InSec",3);
				Catapults_animator.SetBool("PlayerAround",false);
				if(isPlayerInside == true)
				{
					isPlayerInside = false;
					player.GetComponent<HeroCameraAlias>().Catapults_notify(this.gameObject, isPlayerInside);
					player = null;
				}
				break;
		}

		if (Catapults_animator.GetBool ("Shoot_Stone")) {
			Invoke("Shoot_Stone",ShootTime);

		}
	}
	void notToLoad_InSec(){
		notToLoad = false;
	}
	float Distance()
	{
		return Vector3.Distance(this.transform.position, player.transform.position);
	}
}
