using UnityEngine;
using System.Collections;
using PathologicalGames;

public class PoolAPIWrapper : MonoBehaviour {


	public Transform spawn(string poolName, GameObject gameObj) {
		return PoolManager.Pools [poolName].Spawn (gameObj.transform);
	}

	public void despawn(string poolName, GameObject gameObj) {
		PoolManager.Pools [poolName].Despawn (gameObj.transform);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
