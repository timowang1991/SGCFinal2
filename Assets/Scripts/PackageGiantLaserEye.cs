using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PackageGiantLaserEye : MonoBehaviour {

	GameObject circularProgressCanvas_LeftEye;
	GameObject circularProgressCanvas_RightEye;

	public float timeToDestroyAfterTouchGround = 10.0f;
	// Use this for initialization
	void Start () {

//		Platform platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
//
//		if(platform != Platform.PC_Giant)
//		{
//			this.enabled = false;
//		}

		circularProgressCanvas_LeftEye = GameObject.FindGameObjectWithTag("CircularProgressCanvas_LeftEye");
		circularProgressCanvas_RightEye = GameObject.FindGameObjectWithTag("CircularProgressCanvas_RightEye");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		print (col.gameObject.tag);
		if(col.transform.root.tag == "GiantPlayer"){
			GameObject canvasLeftEye = Instantiate(circularProgressCanvas_LeftEye) as GameObject;
			GameObject canvasRightEye = Instantiate(circularProgressCanvas_RightEye) as GameObject;
			canvasLeftEye.GetComponent<Canvas>().enabled = true;
			canvasRightEye.GetComponent<Canvas>().enabled = true;
			
			foreach(Transform childTransform in canvasLeftEye.transform){
				childTransform.gameObject.SetActive(true);
			}
			
			foreach(Transform childTransform in canvasRightEye.transform){
				childTransform.gameObject.SetActive(true);
			}

			canvasLeftEye.AddComponent<GiantCircularProgressActivateLaser>();
			canvasRightEye.AddComponent<GiantCircularProgressActivateLaser>();

			GiantCircularProgressActivateLaser activateLaser = canvasLeftEye.GetComponent<GiantCircularProgressActivateLaser>();
			activateLaser.activatingScript = true;

			if(this.GetComponent<PhotonView>().isMine){
				tellMasterToDestroy();
			}
		}

		if(col.gameObject.name == "Terrain"){
			Invoke("tellMasterToDestroy", timeToDestroyAfterTouchGround);
		}
	}
	
	void tellMasterToDestroy()
	{
		PhotonNetwork.Destroy(gameObject);
	}
}
