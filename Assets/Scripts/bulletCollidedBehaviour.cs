using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class bulletCollidedBehaviour : MonoBehaviour{

	public GameObject BloodFXPrefab;
	private bool isCollided = false;
	private bool despawnSelfInADuration = true;
	private float despawnedDuration = 10;
	private HashSet<string> bloodyable;

	// Use this for initialization
	void Start () {
		isCollided = false;
		Invoke ("despawnSelf", despawnedDuration);
		bloodyable = GameObject.Find ("ConfigManager").GetComponent<GameConfig> ().bloodyable;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collision) {
		if (!isCollided) {
			isCollided = true;
			CancelInvoke();
			ContactPoint contact = collision.contacts[0];
			IDamagedBehaviour selfComponent = (IDamagedBehaviour)collision.gameObject.GetComponent(typeof(IDamagedBehaviour));
			IDamagedBehaviour parentComponent = (IDamagedBehaviour)collision.gameObject.GetComponentInParent(typeof(IDamagedBehaviour));
			if((selfComponent != null && bloodyable.Contains(selfComponent.getDamagedClass())) || (parentComponent != null && bloodyable.Contains(parentComponent.getDamagedClass()))) {
				Debug.Log ("bloody!!");
				Transform bloodFXObj = PoolManager.Pools [GameConfig.CopterPoolName].Spawn (BloodFXPrefab.transform);
				bloodFXObj.position = contact.point;
				bloodFXObj.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
				bloodFXObj.GetComponent<selfDespawn>().despawnInFuture(GameConfig.CopterPoolName);
			}
			Invoke ("despawnSelf", 2);
		}

	}

	void despawnSelf() {
		PoolManager.Pools [GameConfig.CopterPoolName].Despawn (gameObject.transform); //remove rocket)
		isCollided = false;
	}

}
