using UnityEngine;
using System.Collections;

public class Eat : MonoBehaviour {

	public string edibleLayerName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.layer == LayerMask.NameToLayer(edibleLayerName)){

		}
	}
}
