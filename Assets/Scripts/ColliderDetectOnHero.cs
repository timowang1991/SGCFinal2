using UnityEngine;
using System.Collections;

public class ColliderDetectOnHero : MonoBehaviour {

	private int FlammableLayerID = -1;

	void Awake() {
		FlammableLayerID = LayerMask.NameToLayer("Flammable");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onTriggerEnter(Collider other) {
		GameObject objCollideWith = other.gameObject;
		if(objCollideWith.layer == FlammableLayerID) { //collide with Flammable Object
			if(objCollideWith.particleSystem != null && objCollideWith.particleSystem.isPlaying) { //it should have particleSystem

			}
		}
	}
}
