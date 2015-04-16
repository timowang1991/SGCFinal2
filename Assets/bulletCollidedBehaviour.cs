using UnityEngine;
using System.Collections;
using PathologicalGames;

public class bulletCollidedBehaviour : MonoBehaviour {

	private bool isCollided = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit(Collision collision) {
		if (!isCollided) {
			isCollided = true;
			Invoke ("despawnSelf", 2);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (!isCollided) {
			isCollided = true;
			Invoke ("despawnSelf", 2);
		}
	}
	
	void despawnSelf() {
		PoolManager.Pools [GameConfig.CopterPoolName].Despawn (gameObject.transform); //remove rocket
		isCollided = false;
	}
	
}
