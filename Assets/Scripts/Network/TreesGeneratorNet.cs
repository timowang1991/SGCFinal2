using UnityEngine;
using System.Collections;
//[Giant Only] locate in NetworkManager
public class TreesGeneratorNet : Photon.MonoBehaviour {
	
	private const int numMaxGenPoints = 0;
	public GameObject[] treeGenPoints = new GameObject[numMaxGenPoints];
	private int numGenPoints = 0;
	private float timerForCheckingTreeNum;
	private string treeTag;
	private string treeName;
	
	/// <summary>
	/// Already put the treeGenPoints in Inspector, count the num and get the tree's tag and name from prefab.
	/// </summary>
	void Awake () {
		for(int i = 0;i < numMaxGenPoints;i++) {
			if(treeGenPoints[i] == null) {
				numGenPoints = i;
				break;
			}
		}
		//everyone has ConfigManager
		GameConfig config = GameObject.Find ("ConfigManager").GetComponent<GameConfig>();
		treeTag = config.treeObject.tag;
		treeName = config.treeObject.name;
		
		timerForCheckingTreeNum = 0;
		Random.seed = (int)Time.realtimeSinceStartup;
		
	}
	/// <summary>
	/// Enable this script only in Giant Client, and create trees to proper location that assign by the treeGenPoints, and every tree has a checkTreeExistance to check
	/// </summary>
	void Start() {
		//we assume there is no tree when we begin the game
		for(int i = 0;i < numGenPoints;i++) {
			GameObject treeObject = PhotonNetwork.Instantiate(treeName,treeGenPoints[i].transform.position,Quaternion.identity,0);
			treeGenPoints[i].GetComponent<checkTreeExistance>().treeBeSet = treeObject;
		}
	}
	//the lowest num of the tree
	private const int treesExistingLowerBoundInScene = 0;
	//check time
	private const float timeIntervalForChecking = 10;
	
	/// <summary>
	/// Check every "timeIntervalForChecking" time and ensure there is more than "treesExistingLowerBoundInScene" trees in the scene, and generate the trees at random assigned positions (before put it in, check if it is still exist)
	/// </summary>
	void Update () {
		//check is it out of time (timeIntervalForCheckings)
		if(timerForCheckingTreeNum > timeIntervalForChecking) { //time up
			Debug.Log ("Check for Tree existance");
			GameObject[] existedTrees = GameObject.FindGameObjectsWithTag(treeTag);
			Debug.Log ("num of tree existed:" + existedTrees.Length);
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
					PhotonNetwork.Instantiate(treeName,treeGenPoints[randIdx].transform.position,Quaternion.identity,0);
				}
				else {
					continue;
				}
				numTreesNeedToGen--;
			}
			timerForCheckingTreeNum = 0;
		}
		timerForCheckingTreeNum += Time.deltaTime;
	}
	
}