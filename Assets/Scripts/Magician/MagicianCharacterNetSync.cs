using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ExitGames.Client.Photon;

//dont forget to add to observed component in the photonView
using System;

public class MagicianCharacterNetSync : Photon.MonoBehaviour {

	[HideInInspector]
	//public Vector3 correctPlayerPos; // We lerp towards this and initially we need to set it to generating point
	//private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	private MagicianNet_Ctrl netCtrl;
	private float currentRecv_h = 0;
	private float currentRecv_v = 0;
	private float currentRecv_mX = 1;
	
	void Awake() {
		netCtrl = GetComponent<MagicianNet_Ctrl>();
	}
	
	/// <summary>
	/// If this is controlled by other players, then just lerp it to correct position.
	/// </summary>
	void Update()
	{
		if (!photonView.isMine)
		{
			//transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			//transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
			netCtrl.h = Mathf.Lerp(netCtrl.h, this.currentRecv_h, Time.deltaTime * 5);
			netCtrl.v = Mathf.Lerp(netCtrl.v, this.currentRecv_v, Time.deltaTime * 5);
			netCtrl.mX = Mathf.Lerp(netCtrl.mX, this.currentRecv_mX, Time.deltaTime * 5);
		}
	}
	
	/// <summary>
	/// Using isWriting to determine is sending data or receiving data.
	/// </summary>
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			//the part we need to lerp
			//stream.SendNext(transform.position);
			//stream.SendNext(transform.rotation);
			stream.SendNext(netCtrl.h);
			stream.SendNext(netCtrl.v);
			stream.SendNext(netCtrl.mX);
			//stream.SendNext(netCtrl.baseState);
			
		}
		else
		{
			// Network player, receive data
			//this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			//this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			this.currentRecv_h = (float)stream.ReceiveNext();
			this.currentRecv_v = (float)stream.ReceiveNext();
			this.currentRecv_mX = (float)stream.ReceiveNext();
			//netCtrl.baseState = (HeroCtrl_Net2.BaseState)stream.ReceiveNext();
			
		}
	}
}
