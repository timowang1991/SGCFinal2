using UnityEngine;
using System.Collections;

public class Package : MonoBehaviour {

	public float timeToDestory = 10;
	public int addHP = 10;
	// Use this for initialization
	void Start () {
		Invoke ("selfDestory", timeToDestory);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		print (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {
			print (col.gameObject.name);
			//add HP by sending negative value to damage function
			col.gameObject.GetComponent<HealthSystem>().damage(-addHP);
			//PhotonView.Destroy(gameObject);
			PhotonNetwork.Destroy(gameObject);
		}
		
	}


}

