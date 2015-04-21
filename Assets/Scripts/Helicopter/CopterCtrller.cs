using UnityEngine;
using System.Collections;
using PathologicalGames;

//notice that this component would only be enabled in controllable copter
public class CopterCtrller : Photon.MonoBehaviour {

	public Transform refTrans; //the obj named ref on Helicopter
	public GameObject missileObj;
	public GameObject bulletObj;
	public GameObject detonatorObj;
	public GameObject crosshairObj;

	public Transform launchTrans;
	public Transform targetTrans;

	private float missileInitSpeed = 50;
	private float bulletInitSpeed = 100;
	
	public AudioClip launchSound;
	public GameObject muzzle;

	private static string MissileKeyName = "Fire2";
	private static string MachineGunKeyName = "Fire1";
	private Camera mainCam;
	private PrefabPool bulletsPool;
	private PrefabPool missilesPool;
	private PrefabPool detonatorPool;


	//private string launchMissileFuncName;
	//private string fireMachineGunFuncName;

	private HelifinNet helifin;
	private CopterDamagedBehaviour damagable;
	private float timer;
	private bool isMuzzleOn = false;

	void Awake() {
		helifin = gameObject.GetComponent<HelifinNet> ();
		timer = 0;
		missilesPool = PoolManager.Pools [GameConfig.CopterPoolName].GetPrefabPool (missileObj);
		bulletsPool = PoolManager.Pools [GameConfig.CopterPoolName].GetPrefabPool (bulletObj);
		detonatorPool = PoolManager.Pools [GameConfig.CopterPoolName].GetPrefabPool (detonatorObj);
	}

	//below code should only be run in local player

	// Use this for initialization
	void Start () {
		//this component came from js, pretty nasty. so the below code related to this component couldn't be normally indexed by code hinting tool
		damagable = GetComponent<CopterDamagedBehaviour> ();
		helifin.isControllable = true;
		mainCam = Camera.main;
		SmoothFollowCopy smoothFollowObj = mainCam.gameObject.AddComponent<SmoothFollowCopy>(); 
		smoothFollowObj.target = refTrans;
		smoothFollowObj.distance = 15;
		smoothFollowObj.height = 5;
		smoothFollowObj.heightDamping = 2;
		smoothFollowObj.rotationDamping = 3;
		mainCam.transform.parent = this.transform;

		GameObject posObj = GameObject.Find ("copterCrosshairPos");

		GameObject crosshair = (GameObject)Instantiate (crosshairObj, posObj.transform.position, posObj.transform.rotation);
		crosshair.transform.parent = posObj.transform;
		makeCrosshairMove chMove = crosshair.GetComponent<makeCrosshairMove> ();
		chMove.launchPoint = launchTrans;
		chMove.enabled = true;

	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		//Weapons

		if (Input.GetButtonDown (MissileKeyName) && 
		    missilesPool.spawned.Count < missilesPool.limitAmount &&
		    //detonatorPool.spawned.Count < detonatorPool.limitAmount && 
		    timer > 2) {
			//photonView.RPC ("LaunchMissile", PhotonTargets.All, launchTrans.position, (targetTrans.position - launchTrans.position));
			photonView.RPC ("LaunchMissile", PhotonTargets.All, launchTrans.position, targetTrans.position);
			timer = 0;
		} 

		if (Input.GetButtonDown (MachineGunKeyName)) {
			//photonView.RPC ("FireMachineGun", PhotonTargets.All, launchTrans.position, (targetTrans.position - launchTrans.position));
			muzzle.SetActive (true);
			if(bulletsPool.spawned.Count < bulletsPool.limitAmount) {
				photonView.RPC ("FireMachineGun", PhotonTargets.All, launchTrans.position, targetTrans.position);
			}
		}
		else {
			muzzle.SetActive (false);
		}

		//Movement Control
		helifin.vertical1 = Input.GetAxis ("Vertical");
		helifin.horizon1 = Input.GetAxis ("Horizontal");
		helifin.vertical2 = Input.GetAxis ("Vertical2");
		helifin.horizon2 = Input.GetAxis ("Horizontal2");

	}

//--

	//the below code should not depend on variable values initiated in Start or Awake

	void setPosDirSpeed(Transform objTrans, float speedFactor, Vector3 launchPos, Vector3 targetPos) {
		objTrans.position = launchPos;
		objTrans.LookAt (targetPos);
		objTrans.rigidbody.velocity = speedFactor * (targetPos - launchPos).normalized;
	}

	[RPC]
	void LaunchMissile(Vector3 launchPos, Vector3 targetPos) {
		Transform rocketTrans = missilesPool.spawnPool.Spawn (missileObj.transform);
		if (rocketTrans != null) {
			setPosDirSpeed (rocketTrans, missileInitSpeed, launchPos, targetPos);
			audio.PlayOneShot (launchSound);
		}
	}
	
	[RPC]
	void FireMachineGun(Vector3 launchPos, Vector3 targetPos) {
		Transform bulletTrans = bulletsPool.spawnPool.Spawn (bulletObj.transform);
		if (bulletTrans != null) {
			setPosDirSpeed (bulletTrans, bulletInitSpeed, launchPos, targetPos);
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(helifin.rotor_Velocity);
		}
		else
		{
			// Network player, receive data
			helifin.rotor_Velocity = (float) stream.ReceiveNext();
		}
	}
}
 