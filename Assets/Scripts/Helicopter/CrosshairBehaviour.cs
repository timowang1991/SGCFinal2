using UnityEngine;
using System.Collections;

public class CrosshairBehaviour : MonoBehaviour {
	public GameObject[] crosshairs = new GameObject[2];
	public Transform launchPoint;
	public Vector3 originalPos;

	[HideInInspector]
	public float factorX = 0.3f;
	[HideInInspector]
	public float factorY = 1.72f;
	[HideInInspector]
	public int usingCHIndex;
	private GameObject _crosshair;

	// we'll enable this component after player initiating one's own character in NetworkRoomLogic2
	// Use this for initialization 
	void Start () {
		_crosshair = crosshairs [usingCHIndex];
		_crosshair.SetActive (true);
		originalPos = _crosshair.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (usingCHIndex == 1) {
			float angleForY = Vector3.Angle (Vector3.ProjectOnPlane(launchPoint.forward, Vector3.up),launchPoint.forward) * Mathf.Sign(launchPoint.forward.y) * factorY;
			//Debug.Log (angle * );
			float angleForX = Vector3.Angle (Vector3.ProjectOnPlane (launchPoint.up, Vector3.left), launchPoint.up) * Mathf.Sign(launchPoint.right.y) * factorX;
			//Debug.Log (angle * );
			_crosshair.transform.position = new Vector3(originalPos.x + angleForX, originalPos.y + angleForY, originalPos.z);
		}
	}
}
