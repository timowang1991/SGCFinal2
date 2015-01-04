using UnityEngine;
using System.Collections;

public class BigLittleGameLogic : Photon.MonoBehaviour {
	
	public int giantID = -1;
	public HeroCtrl_Net2 currPlayerCharCtrl = null;
	private Platform platform;
	private GameObject Giant = null;
	private GiantHealth giantHealth;
	private PhotonView giantPhotonView;
	
	// Use this for initialization
	public void Start()
	{
		//never do this in Awake, it's the most suitable to do this start.
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if(Giant == null) {
			Giant = GameObject.Find ("Giant_Net");
		}
	}
	
	public void OnJoinedRoom() //this will be only called once while client entering 
	{
		if (platform == Platform.PC_Giant)//Need Identify
		{
			giantID = PhotonNetwork.player.ID;
			if(Giant == null) {
				Giant = GameObject.Find ("Giant_Net");
			}
			giantPhotonView = Giant.GetComponent<PhotonView>(); 
			giantHealth = Giant.GetComponent<GiantHealth>();
			NoticeWhoIsGiant(giantID);
		}
		
	}
	
	public void initVarsByRPC(HeroCtrl_Net2 netCtrl,PhotonTargets target) {
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",target,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;
		netCtrl.photonView.RPC ("initVars",target,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}
	
	public void initVarsByRPC(HeroCtrl_Net2 netCtrl,PhotonPlayer player) {
		netCtrl.photonView.RPC ("setWeaponRenderersRPC",player,HeroCtrl_Net2.WeaponState.None); //init
		int weaponIdx = (int)netCtrl.weaponState;
		netCtrl.photonView.RPC ("initVars",player,weaponIdx,netCtrl.renderersSet[weaponIdx-1,0].enabled,netCtrl.renderersSet[weaponIdx-1,1].enabled); //init current player's state
	}
	
	public void OnPhotonPlayerConnected(PhotonPlayer player) //every player enter would call this
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
		
		// when new players join, we send "who's it" to let them know
		// only one player will do this: the "master"
		if (platform == Platform.PC_Giant) { //if this client is Giant, it will tell others its HP.
			giantPhotonView.RPC ("setInitialHP",player,giantHealth.healthPoint);
		}

		if (platform == Platform.PC_Miniature && currPlayerCharCtrl != null && !player.isLocal) { 
			//tell newly entering player that own miniature's state.
			initVarsByRPC(currPlayerCharCtrl, player);
		}
	}
	
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