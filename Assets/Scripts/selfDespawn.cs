using UnityEngine;
using System.Collections;
using PathologicalGames;

public class selfDespawn : MonoBehaviour {

	private float despawnDuration = 5;
	private string poolName = null;

	// Use this for initialization
	void Start () {

	}

	public void despawnInFuture(string poolNameForDespawn) {
		poolName = poolNameForDespawn;
		if (poolName != null) {
			Invoke ("despawnSelf", despawnDuration);
		}
	}

	void despawnSelf() {
		PoolManager.Pools [poolName].Despawn (transform);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
