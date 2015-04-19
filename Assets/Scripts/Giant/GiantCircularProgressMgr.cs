using UnityEngine;
using System.Collections;

public class GiantCircularProgressMgr : MonoBehaviour {

	public GameObject circularProgressCanvas_LeftEye;
	public GameObject circularProgressCanvas_RightEye;

	// Use this for initialization
	void Start () {
		if(circularProgressCanvas_LeftEye != null){
			GameObject cloneLeftEye = Instantiate(circularProgressCanvas_LeftEye) as GameObject;
			cloneLeftEye.SetActive(true);
		}

		if(circularProgressCanvas_RightEye != null){
			GameObject cloneRightEye = Instantiate(circularProgressCanvas_RightEye) as GameObject;
			cloneRightEye.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
