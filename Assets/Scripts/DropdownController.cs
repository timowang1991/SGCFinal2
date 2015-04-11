using UnityEngine;
using System.Collections;

public class DropdownController : Photon.MonoBehaviour {
	
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
		Vector3 packagePosition = new Vector3 (Random.Range (170, 330), 120, Random.Range (270, 370));
		GameObject package = PhotonNetwork.Instantiate (packagePrefab, packagePosition, Quaternion.identity, 0);

	}

}
