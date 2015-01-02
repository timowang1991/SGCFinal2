using UnityEngine;
using System.Collections;

public class checkTreeExistance : Photon.MonoBehaviour {

	[HideInInspector]
	public GameObject treeBeSet;
	private string treeTag;
	//private BigLittleGameLogic gameLogic;//Need Identify
	private Platform platform;

	// Use this for initialization
	void Awake () {
		GameConfig config = GameObject.Find ("ConfigManager").GetComponent<GameConfig>();
		treeTag = config.treeObject.tag;
		treeBeSet = null;
		//gameLogic = GetComponentInParent<BigLittleGameLogic>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == treeTag) {
			Debug.Log ("tree entered:" + other.name);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == treeTag && other.gameObject == treeBeSet) {
			Debug.Log ("be set tree exited");
			//Debug.Log(gameLogic.giantID);
			if(platform == Platform.PC_Giant) {
				photonView.RPC ("setTreeToNull",PhotonTargets.All,null);
			}
		}
	}

	[RPC]
	public void setTreeToNull() {
		treeBeSet = null;
	}
}
