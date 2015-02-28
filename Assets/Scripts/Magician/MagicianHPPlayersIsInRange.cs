using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicianHPPlayersIsInRange : MonoBehaviour {

	public readonly List<GameObject> GameObjectsWithInRange = new List<GameObject>();


	void OnTriggerEnter(Collider other) {
		//Debug.Log ("OnTriggerEnter" + other.gameObject.name);
		if(other.gameObject.tag == "Player")
		{
			this.GameObjectsWithInRange.Add (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		this.GameObjectsWithInRange.Remove (other.gameObject);
	}

}
