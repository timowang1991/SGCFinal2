﻿using UnityEngine;
using System.Collections;

public class StoneSelfScript : MonoBehaviour {

	public PhotonView CatapultPhotonView;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other) {
		if(CatapultPhotonView.viewID)
		{
			Debug.Log("OnTriggerEnter "+other.gameObject.name +"/tag: "+ other.gameObject.tag);
			
			if(other.gameObject.tag == "Weak")
			{
				Debug.Log ("Stone: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(1,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weaker")
			{
				Debug.Log ("Stone: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(2,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weakest")
			{
				Debug.Log ("Stone: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(3,this.gameObject.tag);
			}
		}
	}
}
