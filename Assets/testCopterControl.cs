using UnityEngine;
using System.Collections;

public class testCopterControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float value2 = Input.GetAxis("Horizontal2");
		rigidbody.velocity += (Vector3.ProjectOnPlane(transform.right,Vector3.up) * value2 * 100 * Time.deltaTime);
	}
}
