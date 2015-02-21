using UnityEngine;
using System.Collections;
//locate below the treeGenPoints.
public class checkTreeExistance : Photon.MonoBehaviour {

	[HideInInspector]
	public GameObject treeBeSet;
	private string treeTag;
	//private BigLittleGameLogic gameLogic;//Need Identify
	private Platform platform;

	/// <summary>
	/// Know the tag name and set treeBeSet to null and wait for generator to collide.
	/// </summary>
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
	/// <summary>
	/// [Collider call from system] check if it is the tree's tag, then debug print 
	/// </summary>
	void OnTriggerEnter(Collider other) {
		if(other.tag == treeTag) {
			Debug.Log ("tree entered:" + other.name);
		}
	}

	/// <summary>
	/// [Collider call from system] check is it the tree this class belong and take off by the giant. 
	/// </summary>
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


	/// <summary>
	/// No one call this for now, maybe because we sync the tree's position.
	/// </summary>
	[RPC]
	public void setTreeToNull() {
		Debug.Log ("Be Set Tree is null in RPC");
		treeBeSet = null;
	}
}
