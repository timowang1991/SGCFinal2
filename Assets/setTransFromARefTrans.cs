using UnityEngine;
using System.Collections;



public class setTransFromARefTrans : MonoBehaviour {
	public Transform refObjTrans;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = refObjTrans.position;
		transform.rotation = refObjTrans.rotation;
	}
}
