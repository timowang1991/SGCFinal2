using UnityEngine;
using System.Collections;

public class BigLittleGameLogic : Photon.MonoBehaviour {
	
	public int giantID = -1;
	public HeroCtrl_Net2 currPlayerCharCtrl = null;
	private Platform platform;
	
	// Use this for initialization
	public void Start()
	{
		
	}
	
	public void OnJoinedRoom()
	{
		// game logic: if this is the only player, then this player is big man
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if (platform == Platform.PC_Giant)//Need Identify
		{
			giantID = PhotonNetwork.player.ID;
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
	
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
		
		// when new players join, we send "who's it" to let them know
		// only one player will do this: the "master"
		
		if (currPlayerCharCtrl != null && !player.isLocal) {
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
		
		/*
		if (player.ID == bigManID)
		{
			//do something
		}
		*/
	}
	
	//	public void OnMasterClientSwitched()
	//	{
	//		Debug.Log("OnMasterClientSwitched");
	//	}
}