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

	void OnJoinedLobby()
	{
		Debug.Log("JoinRandom");
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);
	}

	void OnCreatedRoom() {
		Debug.Log("created room");
	}



	void OnJoinedRoom()
    {
		Debug.Log("joined room");

		if (platform == Platform.PC_Giant) {//Need Identify
						//init as giant
						//Vector3 giantPos = new Vector3(250,-70,119);
						//GameObject currPlayer = PhotonNetwork.Instantiate("Giant_Net", giantPos, Quaternion.identity, 0);
						GameObject.FindGameObjectWithTag ("OVR").GetComponent<OculusFollower> ().setOculusFollower ();
						GameObject.Find ("Giant_Net").GetComponent<PhotonView> ().RequestOwnership ();
						GetComponent<TreesGeneratorNet> ().enabled = true;
				} else if (platform == Platform.PC_Miniature) {
						GameObject currPlayer = PhotonNetwork.Instantiate ("Ninja_Net2_noCollider", startPoint.position, Quaternion.identity, 0);
						currPlayer.GetComponent<HealthSystem>().enabled = true;
						currPlayer.GetComponent<ArrowGenerator>().enabled = true;
						//currPlayer.GetComponent<CharacterNetSyncAlias>().correctPlayerPos = startPoint.position;
						HeroCtrlAlias netCtrl = currPlayer.GetComponent<HeroCtrlAlias> ();
						netCtrl.isControllable = true;
						gameLogic.currPlayerCharCtrl = netCtrl;
						HeroCamAlias cameraLogic = currPlayer.GetComponent<HeroCamAlias> ();
						if (cameraLogic == null) {
								currPlayer.AddComponent<HeroCamAlias> ();
								cameraLogic = currPlayer.GetComponent<HeroCamAlias> ();
						}
						Camera mainCam = Camera.main;
						if (mainCam == null) {
								mainCam = new Camera ();
								mainCam.tag = "MainCamera";
						}
						cameraLogic.cam = mainCam.transform;
						cameraLogic.enabled = true;
						mainCam.transform.parent = currPlayer.transform;

						gameLogic.initVarsByRPC (netCtrl, PhotonTargets.Others);
				} 
				else if (platform == Platform.Phone) {
						GameObject myCatapult =PhotonNetwork.Instantiate("Catapult_Net",getPuttableCataPos(),Quaternion.identity,0);
						myCatapult.GetComponent<CatapultsController>().enabled = true;
						Transform camToPutTrans = myCatapult.transform.FindChild("CamToPut");
						Transform targetPointTrans = myCatapult.transform.Find("initialTargetPoint/TargetPoint");
						//Transform initTargetPointTrans = myCatapult.transform.FindChild("initialTargetPoint");
						//targetPointTrans.transform.position = initTargetPointTrans.position;
						Camera.main.transform.position = camToPutTrans.position;
						Camera.main.transform.rotation = camToPutTrans.rotation;
						Camera.main.transform.parent = myCatapult.transform;
						targetPointTrans.parent = Camera.main.transform;
						MobileController mCtrl = Camera.main.GetComponent<MobileController>();
						//mCtrl.isControllable = true;
						mCtrl.setCatapult(myCatapult);

				}
    }

	public float spaceBetweenCatapult;

	private Vector3 getPuttableCataPos() {
		bool catapultsInTheRange = true;
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

	void OnPhotonJoinRoomFailed() {
		Debug.Log ("Joining Room Failed");
	}

	// Update is called once per frame
	void Update () {}
	
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
