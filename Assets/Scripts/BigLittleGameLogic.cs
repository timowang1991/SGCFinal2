using UnityEngine;
using System.Collections;

public class BigLittleGameLogic : Photon.MonoBehaviour {

	public static int bigManID = -1;
	public HeroCtrl_Net2 currPlayerCharCtrl = null;

	//private static PhotonView ScenePhotonView;
	
	// Use this for initialization
	public void Start()
	{

	}
	
	public void OnJoinedRoom()
	{
		// game logic: if this is the only player, then this player is big man

		//if (PhotonNetwork.playerList.Length == 1)
		//{
		//	bigManID = PhotonNetwork.player.ID;
		//	Debug.Log("Client BigmanId" + bigManID);
		//}


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

		//if (player.ID == bigManID) {
		//NoticeWhoIsBigMan(bigManID);
		//}

		if (currPlayerCharCtrl != null && !player.isLocal) {
			initVarsByRPC(currPlayerCharCtrl, player);
		}
	}
	
	public void NoticeWhoIsBigMan(int playerID)
	{
		Debug.Log ("message sent in big man");
		photonView.RPC("TagBigMan", PhotonTargets.All, playerID);
	}
	public int GetbigManID()
	{
		return bigManID;
	}

	[RPC]
	public void TagBigMan(int playerID) {
		bigManID = playerID;
		Debug.Log("TagBigMan: " + playerID);
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
