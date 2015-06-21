using UnityEngine;
using System.Collections;

public class PackageAddHP : MonoBehaviour {

	public float timeToDestroyAfterTouchGround = 10;
	public int addHP = 10;
	// Use this for initialization
	void Start () {
//		Invoke ("selfDestroy", timeToDestroy);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision col){

		print (col.gameObject.name);
		if (col.transform.root.gameObject.tag == "Player") {
//			print (col.gameObject.name);
			if(col.transform.root.gameObject.GetComponent<PhotonView>().isMine){
				//add HP by sending negative value to damage function
				col.transform.root.gameObject.GetComponent<HealthSystem>().damage(-addHP);
				this.GetComponent<PhotonView>().RPC("tellMasterToDestroy",this.GetComponent<PhotonView>().owner);
				//PhotonNetwork.Destroy(gameObject);
			}
		}
		else if(col.gameObject.tag == "Player"){
			if(col.gameObject.GetComponent<PhotonView>().isMine){
				//add HP by sending negative value to damage function
				col.gameObject.GetComponent<HealthSystem>().damage(-addHP);
				this.GetComponent<PhotonView>().RPC("tellMasterToDestroy",this.GetComponent<PhotonView>().owner);
				//PhotonNetwork.Destroy(gameObject);
			}
		}
		else if(col.gameObject.name == "Terrain"){
			Invoke("tellMasterToDestroy", timeToDestroyAfterTouchGround);
		}
	}
	
	[RPC]
	void tellMasterToDestroy()
	{
		CancelInvoke ("tellMasterToDestroy");
		PhotonNetwork.Destroy(gameObject);
	}
}
