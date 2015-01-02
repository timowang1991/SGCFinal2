using UnityEngine;
using System.Collections;

public class GrabAndReleaseTree : MonoBehaviour {

	public float timeToDestroyTree;
	//private bool isGiantClient = false;
	void Awake() {
	//	isGiantClient = (GameObject.FindGameObjectWithTag ("OVR") != null);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "BigTreeTrunk"){
			other.gameObject.GetComponent<TreeLifeCycle>().cancelDestroy();
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "BigTreeTrunk"){
			//Debug.Log(other.gameObject.tag);
			other.gameObject.GetComponent<TreeLifeCycle>().countDownDestroy();
		}
	}
}
