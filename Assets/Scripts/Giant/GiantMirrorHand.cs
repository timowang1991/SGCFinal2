﻿#if !UNITY_IOS
using UnityEngine;
using System.Collections;

public class GiantMirrorHand : Photon.MonoBehaviour {
	
	private AvatarController con;

	// Use this for initialization
	void Start () {
		con = this.GetComponent<AvatarController> ();
		con.mirroredMovement = false;
	}
	
	[RPC]
	void ChanageHands(int HowLongItLast)
	{
		con.mirroredMovement = true;
		Invoke ("ChangeMirrorBack", HowLongItLast);
	}

	void ChangeMirrorBack()
	{
		con.mirroredMovement = false;
	}
}
#endif