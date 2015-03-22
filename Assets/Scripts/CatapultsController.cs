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
	private GameObject stoneClonedCache;
	/// <summary>
	/// Call RPC to tell everyone to shoot the stone at this direction. And tell the stone self destory after 10s.
	/// </summary>
	public void Shoot_Stone()//should be protected by isControllable
	{
		//if(photonView.isMine) {
			Debug.Log ("shoot stone called by me");
			stoneClonedCache = Stone_clone;
			Stone_clone = null;
			Vector3 direction =  TargetPoint.transform.position - Cam.position;
			stoneClonedCache.GetComponent<PhotonView> ().RPC ("shootSelfRPC", PhotonTargets.All, direction);
			stoneClonedCache.GetComponent<Stone_NetSync>().invokeDestroySelfOverNet (3);
			Camera.main.transform.parent = stoneClonedCache.transform;
			//Stone_clone = null;
		//}
	}

	private Transform stonePlacedTrans = null;

	/// <summary>
	/// Get the animation/Cam position/StonePosition.
	/// </summary>
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
	
	/// <summary>
	/// Get the Catapult animation state: (1)if the animation is loaded state and there wasn't a stone, then instantiate one. (2) if the animation is playing LoadingleState, then ignore it and set isLoaded to false. (3)if Shoot_Stone is true and no one had create one invoke, then invoke it after "ShootTime". 
	/// </summary>
	void Update () {
		//Debug.Log (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash);

		//if(isControllable) { //use enable/disable mechanism
		if (Catapults_animator.GetCurrentAnimatorStateInfo (0).nameHash == LoadedState && isLoaded == false && Stone_clone==null) {
			parasArrayForStoneInit[0] = photonView.viewID;
			Stone_clone = PhotonNetwork.Instantiate ("Stone_Net", stonePlacedTrans.position , transform.rotation , 0, parasArrayForStoneInit);
			Stone_clone.GetComponent<StoneSelfScript>().initCamTransform = transform.FindChild("CamToPut");
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
	/// <summary>
	/// no use for now
	/// </summary>
	void notToLoad_InSec(){
		notToLoad = false;
	}
	/// <summary>
	/// no use for now
	/// </summary>
	float Distance()
	{
		return Vector3.Distance(this.transform.position, player.transform.position);
	}
}
