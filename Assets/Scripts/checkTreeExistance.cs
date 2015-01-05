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
		platform = GameObject.Find ("PlatformManager").GetComponent<PlatformIndicator>().platform;
		//gameLogic = GetComponentInParent<BigLittleGameLogic>();
	}

	void Start() {
		platform = GameObject.Find ("PlatformManager").GetComponent<PlatformIndicator>().platform;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(platform);
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == treeTag) {
			Debug.Log ("tree entered:" + other.name);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == treeTag && other.gameObject == treeBeSet) {
			Debug.Log ("be set tree exited");
			//Debug.Log(platform);
			if(platform == Platform.PC_Giant) {
				Debug.Log ("Be Set Tree is null");
				treeBeSet = null;
				//photonView.RPC ("setTreeToNull",PhotonTargets.All,null);
			}
		}
	}

	[RPC]
	public void setTreeToNull() {
		Debug.Log ("Be Set Tree is null in RPC");
		treeBeSet = null;
	}
}
