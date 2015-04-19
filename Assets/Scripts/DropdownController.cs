using UnityEngine;
using System.Collections;

public class DropdownController : Photon.MonoBehaviour {
	public Vector3 minPosition;
	public Vector3 maxPosition;

	public float minInterval;
	public float maxInterval;

	//public Transform packagePrefab;
	public string packagePrefab = "Package";
	Platform platform;
	

	// Use this for initialization
	void Start () {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
	}
	
	// Update is called once per frame
	void Update () {
		if (/*platform == Platform.PC_Giant &&*/ Input.GetKeyDown(KeyCode.P) ) {
			Drop ();

		}
	}



	void Drop(){
		Vector3 packagePosition = new Vector3 (Random.Range (minPosition.x, maxPosition.x),
		                                       Random.Range (minPosition.y, maxPosition.y),
		                                       Random.Range (minPosition.z, maxPosition.z));
		GameObject package = PhotonNetwork.Instantiate (packagePrefab, packagePosition, Quaternion.identity, 0);
	}

}
