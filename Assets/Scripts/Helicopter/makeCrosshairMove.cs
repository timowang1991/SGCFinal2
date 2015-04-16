using UnityEngine;
using System.Collections;

public class makeCrosshairMove : MonoBehaviour {

	public Transform launchPoint;
	public Vector3 originalPos;
	public float factorX;
	public float factorY;
	
	// Use this for initialization
	void Start () {
		originalPos = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
		float angleForY = Vector3.Angle (Vector3.ProjectOnPlane(launchPoint.forward, Vector3.up),launchPoint.forward) * Mathf.Sign(launchPoint.forward.y) * factorY;
		//Debug.Log (angle * );
		float angleForX = Vector3.Angle (Vector3.ProjectOnPlane (launchPoint.up, Vector3.left), launchPoint.up) * Mathf.Sign(launchPoint.right.y) * factorX;
		//Debug.Log (angle * );
		transform.position = new Vector3(originalPos.x + angleForX, originalPos.y + angleForY, originalPos.z);
	}
}
