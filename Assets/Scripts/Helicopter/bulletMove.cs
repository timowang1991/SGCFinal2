using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + transform.forward * speed * Time.deltaTime;
	}
}
