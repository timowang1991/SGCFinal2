using UnityEngine;
using System.Collections;

public class TreesGeneratorNet : Photon.MonoBehaviour {

	private const int numMaxGenPoints = 10;
	public GameObject[] treeGenPoints = new GameObject[numMaxGenPoints];
	private int numGenPoints = 0;
	private float timerForCheckingTreeNum;
	private string treeTag;
	private string treeName;

	// Use this for initialization
	void Awake () {
		for(int i = 0;i < numMaxGenPoints;i++) {
			if(treeGenPoints[i] == null) {
				numGenPoints = i;
				break;
			}
		}

		GameConfig config = GameObject.Find ("ConfigManager").GetComponent<GameConfig>();
		treeTag = config.treeObject.tag;
		treeName = config.treeObject.name;

		timerForCheckingTreeNum = 0;
		Random.seed = (int)Time.realtimeSinceStartup;

	}
	//enable this script only in Giant Client
	void Start() {
		//we assume there is no tree when we begin the game

	}

	void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom TreeGenerator");
		for(int i = 0;i < numGenPoints;i++) {
			GameObject treeObject = PhotonNetwork.Instantiate(treeName,treeGenPoints[i].transform.position,Quaternion.identity,0);
			treeGenPoints[i].GetComponent<checkTreeExistance>().treeBeSet = treeObject;
		}
	}

	private const int treesExistingLowerBoundInScene = 3;
	private const float timeIntervalForChecking = 10;

	// Update is called once per frame
	void Update () {
		if(timerForCheckingTreeNum > timeIntervalForChecking) { //time up
			Debug.Log (timerForCheckingTreeNum);
			GameObject[] existedTrees = GameObject.FindGameObjectsWithTag(treeTag);
			int numTreesNeedToGen = 0;
			int numExistedTreesInScene = existedTrees.Length;
			if(existedTrees == null) { //num trees = 0
				numTreesNeedToGen = treesExistingLowerBoundInScene;
			}
			else if(numExistedTreesInScene < treesExistingLowerBoundInScene) {
				numTreesNeedToGen = treesExistingLowerBoundInScene - numExistedTreesInScene;
			}

			while(numTreesNeedToGen > 0) {
				int randIdx = ((int)Random.value * numGenPoints);
				if(randIdx == numGenPoints) {
					randIdx--;
				}
				if(treeGenPoints[randIdx].GetComponent<checkTreeExistance>().treeBeSet == null) {
					GameObject treeObject = PhotonNetwork.Instantiate(treeName,treeGenPoints[randIdx].transform.position,Quaternion.identity,0);
					treeGenPoints[randIdx].GetComponent<checkTreeExistance>().treeBeSet = treeObject;
				}
				else {
					continue;
				}
				numTreesNeedToGen--;
			}
			timerForCheckingTreeNum = 0;
		}
		//Debug.Log (timerForCheckingTreeNum);
		timerForCheckingTreeNum += Time.deltaTime;

	}

}
