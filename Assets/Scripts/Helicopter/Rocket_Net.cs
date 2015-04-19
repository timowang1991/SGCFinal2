using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Rocket_Net : Photon.MonoBehaviour,IDamageOthersBehaviour {

	private string poolName = GameConfig.CopterPoolName;
	public GameObject detonatorPrefab;
	private bool isCollided = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//void OnTriggerEnter(Collider other) {
	//}

	void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts [0];
//		if(photonView.isMine && !isCollided) {
//			isCollided = true;
//			photonView.RPC ("rocketExplodeAndDestroy", PhotonTargets.All, contact.normal, contact.point);
//		}
		if (!isCollided) {
			isCollided = true;
			rocketExplodeAndDestroy (contact.normal, contact.point);
		}
	}

	//[RPC]
	void rocketExplodeAndDestroy(Vector3 contactNormal, Vector3 contactPos) {
		//Debug.Log ("RPC:rocketExplodeAndDestroy");
		Transform detonatorObj = PoolManager.Pools [poolName].Spawn (detonatorPrefab.transform); //explode
		detonatorObj.position = contactPos;
		detonatorObj.rotation = Quaternion.FromToRotation (Vector3.up, contactNormal);

		//damage
		Invoke ("despawnSelf", 1);

	}

	void despawnSelf() {
		PoolManager.Pools [poolName].Despawn (gameObject.transform); //remove rocket
		isCollided = false;
	}


	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		return 20;
	}

	bool IDamageOthersBehaviour.isContinuousDamage(IDamagedBehaviour damagable) {
		return false;
	}

	//callback
	void IDamageOthersBehaviour.beforeDamaging (IDamagedBehaviour damagable) {}
	void IDamageOthersBehaviour.afterDamaging (IDamagedBehaviour damagable) {}

}
