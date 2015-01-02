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
	private BigLittleGameLogic gameLogic;

	// Use this for initialization
    void Start()
    {
		PhotonNetwork.ConnectUsingSettings("0.1");
		gameLogic = GetComponent<BigLittleGameLogic>();
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

		if(GameObject.FindGameObjectWithTag("OVR") != null) {
			//init as giant
			//Vector3 giantPos = new Vector3(250,-70,119);
			//GameObject currPlayer = PhotonNetwork.Instantiate("Giant_Net", giantPos, Quaternion.identity, 0);
			GameObject.FindGameObjectWithTag("OVR").GetComponent<OculusFollower>().setOculusFollower();
			GameObject.Find ("Giant_Net").GetComponent<PhotonView>().RequestOwnership();

		}
		else {
			GameObject currPlayer = PhotonNetwork.Instantiate("Ninja_Net2_noCollider", startPoint.position, Quaternion.identity, 0);
			//currPlayer.GetComponent<CharacterNetSyncAlias>().correctPlayerPos = startPoint.position;
			HeroCtrlAlias netCtrl = currPlayer.GetComponent<HeroCtrlAlias>();
			netCtrl.isControllable = true;
			gameLogic.currPlayerCharCtrl = netCtrl;
			HeroCamAlias cameraLogic = currPlayer.GetComponent<HeroCamAlias>();
			if(cameraLogic == null) {
				currPlayer.AddComponent<HeroCamAlias>();
				cameraLogic = currPlayer.GetComponent<HeroCamAlias>();
			}
			Camera mainCam = Camera.main;
			if(mainCam == null) {
				mainCam = new Camera();
				mainCam.tag = "MainCamera";
			}
			cameraLogic.cam = mainCam.transform;
			cameraLogic.enabled = true;
			mainCam.transform.parent = currPlayer.transform;

			gameLogic.initVarsByRPC(netCtrl,PhotonTargets.Others);
		}
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
