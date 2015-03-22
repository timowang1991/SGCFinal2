using UnityEngine;
using System.Collections;
using HeroCtrlAlias = HeroCtrl_Net2;
using HeroCamAlias = HeroCamera_New;
using CharacterNetSyncAlias = CharacterNetSync2;
public class NetworkRoomLogic2 : Photon.MonoBehaviour{

	//private PhotonView ScenePhotonView;
	//private PhotonView playerPhotonView;
	//private string roomName = "bigLittleBattle";
	public Transform startPoint;
	public Transform[] catapultPoints = new Transform[2];
	private Vector3[] cataPointsPos = new Vector3[2];
	private float catapultXDiff = 0;
	private float catapultZDiff = 0;
	private Vector3 randPos = new Vector3();

	private BigLittleGameLogic gameLogic;//Need Identify
	private Platform platform;

	private const float highestYCoordinateInScene = 245;
	public bool testingMagician = false;

	void Awake() {
		Random.seed = (int)(Time.realtimeSinceStartup + PhotonNetwork.GetPing() * Random.value);
		cataPointsPos [0] = catapultPoints [0].position;
		cataPointsPos [1] = catapultPoints [1].position;
		catapultXDiff = cataPointsPos[0].x - cataPointsPos[1].x;
		catapultZDiff = cataPointsPos[0].z - cataPointsPos[1].z;
		randPos.y = highestYCoordinateInScene; 
	}

	// Use this for initialization
    void Start()
    {
		PhotonNetwork.ConnectUsingSettings("0.1");
		gameLogic = GetComponent<BigLittleGameLogic>();
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
    }

    /// <summary>
    /// Joing the room.
    /// </summary>
	void OnJoinedLobby()
	{
		Debug.Log("JoinRandom");
		PhotonNetwork.JoinRandomRoom();
	}
	/// <summary>
	/// If fail to join a random room, then create one.
	/// </summary>
	void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);
	}

	/// <summary>
	/// Create successd.
	/// </summary>
	void OnCreatedRoom() {
		Debug.Log("created room");
	}


	/// <summary>
	/// Joining Room Callback, determine by Platform and set properly component to enable.
	/// </summary>
	void OnJoinedRoom()
    {
		Debug.Log("joined room");

		if (platform == Platform.PC_Giant) {//Need Identify
						//init as giant
						//Vector3 giantPos = new Vector3(250,-70,119);
						//GameObject currPlayer = PhotonNetwork.Instantiate("Giant_Net", giantPos, Quaternion.identity, 0);
						GameObject.FindGameObjectWithTag ("OVR").GetComponent<OculusFollower> ().setOculusFollower ();

						//to request ownership - no matter which setting the PhotonView has.
						GameObject.Find ("52bangd-T-Pose").GetComponent<PhotonView> ().RequestOwnership ();
						GetComponent<TreesGeneratorNet> ().enabled = true;
				} else if (platform == Platform.PC_Miniature) {
						GameObject currPlayer;
						if(testingMagician)
						{
							currPlayer = PhotonNetwork.Instantiate ("Magician", startPoint.position, Quaternion.identity, 0);	
							
							//currPlayer.GetComponent<CharacterNetSyncAlias>().correctPlayerPos = startPoint.position;
							MagicianNet_Ctrl netCtrl = currPlayer.GetComponent<MagicianNet_Ctrl> ();
							//isControllable is hide for inspector, and set to false by default
							netCtrl.isControllable = true;
							gameLogic.currPlayerCharCtrlMagician= netCtrl;

							//init complete and tell everyone which weapon to take
							gameLogic.initVarsByRPC (netCtrl, PhotonTargets.Others);
						}
						else
						{
							currPlayer = PhotonNetwork.Instantiate ("Ninja_Net2_noCollider", startPoint.position, Quaternion.identity, 0);
							currPlayer.GetComponent<ArrowGenerator>().enabled = true;
							
							//currPlayer.GetComponent<CharacterNetSyncAlias>().correctPlayerPos = startPoint.position;
							HeroCtrlAlias netCtrl = currPlayer.GetComponent<HeroCtrlAlias> ();
							//isControllable is hide for inspector, and set to false by default
							netCtrl.isControllable = true;
							gameLogic.currPlayerCharCtrl = netCtrl;

							//init complete and tell everyone which weapon to take
							gameLogic.initVarsByRPC (netCtrl, PhotonTargets.Others);
						}

						currPlayer.GetComponent<HealthSystem>().enabled = true;
						
						//add a HeroCamera_New to currPlayer (HeroCamera_New to fine the head position of the player)
						HeroCamAlias cameraLogic = currPlayer.GetComponent<HeroCamAlias> ();
						if (cameraLogic == null) {
								currPlayer.AddComponent<HeroCamAlias> ();
								cameraLogic = currPlayer.GetComponent<HeroCamAlias> ();
						}
						//Camera.main == The first enabled camera tagged "MainCamera" (Read Only).
						Camera mainCam = Camera.main;
						if (mainCam == null) {
								mainCam = new Camera ();
								mainCam.tag = "MainCamera";
						}
						//set the transform of the camera
						cameraLogic.cam = mainCam.transform;
						cameraLogic.enabled = true;
						mainCam.transform.parent = currPlayer.transform;
						
				} 
				//if is the phone
				else if (platform == Platform.Phone) {
						//Instantiate Catapult_Net at a appropriate position.
						GameObject myCatapult =PhotonNetwork.Instantiate("Catapult_Net",getPuttableCataPos(),Quaternion.identity,0);
						myCatapult.GetComponent<CatapultsController>().enabled = true;
						Transform camToPutTrans = myCatapult.transform.FindChild("CamToPut");
						Transform targetPointTrans = myCatapult.transform.Find("initialTargetPoint/TargetPoint");
						//Transform initTargetPointTrans = myCatapult.transform.FindChild("initialTargetPoint");
						//targetPointTrans.transform.position = initTargetPointTrans.position;
						
						//set camera position
						Camera.main.transform.position = camToPutTrans.position;
						Camera.main.transform.rotation = camToPutTrans.rotation;
						Camera.main.transform.parent = myCatapult.transform;
						targetPointTrans.parent = Camera.main.transform;
						MobileController mCtrl = Camera.main.GetComponent<MobileController>();
						//mCtrl.isControllable = true;
			Debug.Log(myCatapult.name);
						//let the MC know which Catapult is controllable
						mCtrl.setCatapult(myCatapult);

				}
    }

	public float spaceBetweenCatapult;

	/// <summary>
	/// Use collider to determine the position is appropriate for the catapult, and return the random position(x,y,z)
	/// </summary>
	private Vector3 getPuttableCataPos() {
		bool catapultsInTheRange = true;
		//use collider to determine the position is appropriate for the catapult
		while (catapultsInTheRange) {
			randPos.x = cataPointsPos[1].x + Random.value * (1-Random.value) * catapultXDiff;
			randPos.z = cataPointsPos[1].z + Random.value * (1-Random.value) * catapultZDiff;
			randPos.y = 25; //sphere cast must be done near ground
			Collider[] colliders = Physics.OverlapSphere(randPos, spaceBetweenCatapult);
			bool toReRandom = false;
			foreach(Collider collider in colliders) {
				if(collider.tag == "Catapult") {
					toReRandom = true; //re-random
					break;
				}
			}
			if(!toReRandom) {
				catapultsInTheRange = false;
			}
		}

		randPos.y = highestYCoordinateInScene;
		//use raycast to find proper ground height;
		RaycastHit[] hits;
		hits = Physics.RaycastAll(randPos,-Vector3.up,300);
		bool isHitTerrain = false;
		if(hits != null) {
			foreach(RaycastHit hit in hits) {
				if(hit.collider.name == "Terrain") {
					randPos.y = hit.point.y;
					isHitTerrain = true;
					break;
				}
			}
		}
		if(!isHitTerrain) {
			randPos.y = (cataPointsPos[0].y + cataPointsPos[1].y)/2;
		}
		return randPos;
	}

	/// <summary>
	/// Fail to Join the room
	/// </summary>
	void OnPhotonJoinRoomFailed() {
		Debug.Log ("Joining Room Failed");
	}

	// Update is called once per frame
	void Update () {}
	
	//nothing here
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
//		
//		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
//		{
//			bool shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;
//			
//			if (shoutMarco && GUILayout.Button("Marco!"))
//			{
//				myPhotonView.RPC("Marco", PhotonTargets.All);
//			}
//			if (!shoutMarco && GUILayout.Button("Polo!"))
//			{
//				myPhotonView.RPC("Polo", PhotonTargets.All);
//			}
//		}
	}
}
