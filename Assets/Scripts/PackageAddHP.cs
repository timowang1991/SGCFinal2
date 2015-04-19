using UnityEngine;
using System.Collections;

public class PackageAddHP : MonoBehaviour {

	public float timeToDestroy = 10;
	public int addHP = 10;
	// Use this for initialization
	void Start () {
		Invoke ("selfDestroy", timeToDestroy);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision col){
//		print (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {
//			print (col.gameObject.name);
			if(col.gameObject.GetComponent<PhotonView>().isMine){
				//add HP by sending negative value to damage function
				col.gameObject.GetComponent<HealthSystem>().damage(-addHP);
				this.GetComponent<PhotonView>().RPC("tellMasterToDestroy",this.GetComponent<PhotonView>().owner);
				//PhotonNetwork.Destroy(gameObject);
			}
		}
	}
	
	[RPC]
	void tellMasterToDestroy()
	{
		PhotonNetwork.Destroy(gameObject);
	}
}
