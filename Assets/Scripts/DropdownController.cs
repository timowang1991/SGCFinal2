using UnityEngine;
using System.Collections;

public class DropdownController : Photon.MonoBehaviour {
	public Vector3 minPosition = new Vector3(100,200,100);
	public Vector3 maxPosition = new Vector3(101,201,101);
//	public Vector3 maxPosition = new Vector3(400,200,400);

	public float minInterval = 5.0f;
	public float maxInterval = 10.0f;

	//public Transform packagePrefab;
	public string packagePrefab = "Package";
	Platform platform;
	

	// Use this for initialization
	void Start () {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		StartCoroutine("DropPackageInRandomTimeInterval");
	}
	
	// Update is called once per frame
	void Update () {
		if (/*platform == Platform.PC_Giant &&*/ Input.GetKeyDown(KeyCode.P) ) {
			Drop ();

		}
	}

	IEnumerator DropPackageInRandomTimeInterval(){
		while(true){
			yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));
			Drop();
		}
	}

	void Drop(){
		Vector3 packagePosition = new Vector3 (Random.Range (minPosition.x, maxPosition.x),
		                                       Random.Range (minPosition.y, maxPosition.y),
		                                       Random.Range (minPosition.z, maxPosition.z));
		GameObject package = PhotonNetwork.Instantiate (packagePrefab, packagePosition, Quaternion.identity, 0);
	}

}
