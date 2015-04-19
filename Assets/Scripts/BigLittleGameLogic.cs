using UnityEngine;
using System.Collections;

public class BigLittleGameLogic : Photon.MonoBehaviour {
	
	public int giantID = -1;
	public HeroCtrl_Net2 currPlayerCharCtrl = null;
	public MagicianNet_Ctrl currPlayerCharCtrlMagician = null;
	private Platform platform;
	private GameObject Giant = null;
	private GiantHealth giantHealth;
	private PhotonView giantPhotonView;

	private bool testingMagician = false;
	private bool testingCopter = false;
	
	// Use this for initialization
	public void Start()
	{
		//never do this in Awake, it's the most suitable to do this start.
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if(Giant == null) {
			Giant = GameObject.Find (GameConfig.giantGameObjectName);
		}
		NetworkRoomLogic2 networkRoomLogic2 = GetComponent<NetworkRoomLogic2> ();
		testingCopter = networkRoomLogic2.testingCopter;
		testingMagician = networkRoomLogic2.testingMagician;
	}

	/// <summary>
	/// [RPC-like]This will be only called once while client entering (all connect peer).
	/// </summary>
	public void OnJoinedRoom()
	{
		if (platform == Platform.PC_Giant)//Need Identify
		{
			giantID = PhotonNetwork.player.ID;
			if(Giant == null) {
				Giant = GameObject.Find (GameConfig.giantGameObjectName);
			}
			giantPhotonView = Giant.GetComponent<PhotonView>(); 
			giantHealth = Giant.GetComponent<GiantHealth>();

			//notify everyone who is Giant
			NoticeWhoIsGiant(giantID);
		}
		
	}
	
	/// <summary>
	/// (Basiclly Called by NetworkRoomLogin2.cs and telling Others)Set which weapon is going to render, and render the next weapon.
	/// </summary>
	public void initVarsByRPC(HeroCtrl_Net2 netCtrl,PhotonTargets target) {
		//Function in  HeroCtrl_Net2(which is inherit from Photon.MonoBehaviour), set which weapon is going to render.
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",target,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;

		//Function to use when player want to change weapon
		netCtrl.photonView.RPC ("initVars",target,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}
	/// <summary>
	/// Called by self, send the info to all (? but why need everyone to tell this new player?)
	/// </summary>
	public void initVarsByRPC(HeroCtrl_Net2 netCtrl,PhotonPlayer player) {
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",player,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;
		netCtrl.photonView.RPC ("initVars",player,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}
	
	/// <summary>
	/// (Basiclly Called by NetworkRoomLogin2.cs and telling Others)Set which weapon is going to render, and render the next weapon.
	/// </summary>
	public void initVarsByRPC(MagicianNet_Ctrl netCtrl,PhotonTargets target) {
		//Function in  HeroCtrl_Net2(which is inherit from Photon.MonoBehaviour), set which weapon is going to render.
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",target,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;
		
		//Function to use when player want to change weapon
		netCtrl.photonView.RPC ("initVars",target,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}
	/// <summary>
	/// Called by self, send the info to all (? but why need everyone to tell this new player?)
	/// </summary>
	public void initVarsByRPC(MagicianNet_Ctrl netCtrl,PhotonPlayer player) {
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",player,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;
		netCtrl.photonView.RPC ("initVars",player,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}

	/// <summary>
	/// each time a player connected, 
	/// every player in the same room(including this latest player) would call this function
	/// </summary>
	public void OnPhotonPlayerConnected(PhotonPlayer player) { 
		Debug.Log("OnPhotonPlayerConnected: " + player);
		
		// when new players join, we send "who's it" to let them know
		// only one player will do this: the "master"
		if (platform == Platform.PC_Giant) { //if this client is Giant, it will tell the latest client its HP.
			giantPhotonView.RPC ("setInitialHP",player,giantHealth.healthPoint);
		}

		if (testingMagician) { //set the state variables for charactor controller
			if (platform == Platform.PC_NonGiant && currPlayerCharCtrlMagician != null && !player.isLocal) { 
				initVarsByRPC(currPlayerCharCtrlMagician, player);
			}
		} 
		else if(!testingCopter){
			if (platform == Platform.PC_NonGiant && currPlayerCharCtrl != null && !player.isLocal) { 
				initVarsByRPC(currPlayerCharCtrl, player);
			}
		}
	}
	/// <summary>
	/// Use OthersBuffered to tell who is giant (which can be told the clients who joind later)
	/// </summary>
	public void NoticeWhoIsGiant(int playerID)
	{
		Debug.Log ("message sent in giant");
		photonView.RPC("TagGiant", PhotonTargets.OthersBuffered, playerID);
	}
	
	[RPC]
	public void TagGiant(int playerID) {
		giantID = playerID;
		Debug.Log("TagGiant: " + playerID);
	}
	
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerDisconnected: " + player);
	}

}