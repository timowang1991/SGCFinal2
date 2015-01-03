using UnityEngine;
using System.Collections;
using HeroCameraAlias = HeroCamera_New;
public class CatapultsController : Photon.MonoBehaviour {

	Animator Catapults_animator;
	public GameObject player;
	public float distance;
	public bool isLoaded = false;
	public bool isPlayerInside = false;
	int PlayerAround = Animator.StringToHash("PlayerAround");
	static int LoadedState = Animator.StringToHash("Base Layer.loaded");
	static int LoadingleState = Animator.StringToHash("Base Layer.Loading");

	public float ShootTime;
	public float Speed;
	//[HideInInspector]
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
	
//	void OnTriggerEnter(Collider other) {
//		Debug.Log ("enter"+other.gameObject.name);
//		if(playstate == PlayerState.NoPlayer && other.tag == "Player")
//		{
//			player = other.gameObject;
//			playstate = PlayerState.PlayerInside;
//		}
//	}
//
//	void OnTriggerExit(Collider other) {
//		Debug.Log ("exit");
//		//player = null;
//		playstate = PlayerState.NoPlayer;
//	}
	[HideInInspector]
	public GameObject Stone_clone;

	public void Shoot_Stone()//should be protected by isControllable
	{
		//if(photonView.isMine) {
			Debug.Log ("shoot stone called by me");
			Vector3 direction =  TargetPoint.transform.position - Cam.position;
			Stone_clone.GetComponent<PhotonView> ().RPC ("shootSelfRPC", PhotonTargets.All, direction);
			Stone_clone.GetComponent<Stone_NetSync>().invokeDestroySelfOverNet (10);
			Stone_clone = null;
		//}
	}

	private Transform stonePlacedTrans = null;

	// Use this for initialization
	void Start () {
		Catapults_animator = GetComponent<Animator>();
		Cam = Camera.main.transform;
		//player = GameObject.FindGameObjectWithTag("Player");
		stonePlacedTrans = transform.Find ("Catapult_Bone_Main/Catapult_Bone_03/StonePosition");
	}
	//[HideInInspector]
	//public bool isControllable = false;

	// Update is called once per frame
	private object[] parasArrayForStoneInit = new object[1];
	
	void Update () {
		//Debug.Log (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash);

		//if(isControllable) { //use enable/disable mechanism
		if (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash == LoadedState && isLoaded == false && Stone_clone==null) {
			parasArrayForStoneInit[0] = photonView.viewID;
			Stone_clone =  PhotonNetwork.Instantiate ("Stone_Net", stonePlacedTrans.position , transform.rotation , 0, parasArrayForStoneInit);
			Stone_clone.GetComponent<StoneSelfScript>().CatapultPhotonView = photonView;
			isLoaded=true;
		} 

		if(Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash == LoadingleState){
			isLoaded=false;
		}
		
//		switch (playstate) {
//			case PlayerState.PlayerInside:
//				Catapults_animator.SetBool("PlayerAround",true);
//				if(isPlayerInside==false)
//				{
//					isPlayerInside = true;
//					player.GetComponent<HeroCameraAlias>().Catapults_notify(this.gameObject, isPlayerInside);
//				}
//				/*
//				if(Distance() > distance)
//				{
//					playstate = PlayerState.NoPlayer;
//				}
//				*/
//				break;
//			case PlayerState.NoPlayer:
//				notToLoad = true;
//				Invoke("notToLoad_InSec",3);
//				Catapults_animator.SetBool("PlayerAround",false);
//				if(isPlayerInside == true)
//				{
//					isPlayerInside = false;
//					player.GetComponent<HeroCameraAlias>().Catapults_notify(this.gameObject, isPlayerInside);
//					player = null;
//				}
//				break;
//		}

		if (Catapults_animator.GetBool ("Shoot_Stone") && !IsInvoking("Shoot_Stone") && Stone_clone != null) {
			Invoke("Shoot_Stone",ShootTime);
		}
		//}
	}
	void notToLoad_InSec(){
		notToLoad = false;
	}
	float Distance()
	{
		return Vector3.Distance(this.transform.position, player.transform.position);
	}
}
