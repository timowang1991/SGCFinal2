#if !UNITY_IOS
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GiantCircularProgressEvent : MonoBehaviour {
//	public string canvasStringPath = "Assets/CircularProgressBar/CircularProgressCanvas_OneEye";

	public GameObject circularProgessCanvas;

	GameObject circularProgressCanvas_LeftEye;
	GameObject circularProgressCanvas_RightEye;

	// Use this for initialization
	void Start () {
//		circularProgessCanvas = Resources.Load(canvasStringPath) as GameObject;
//		circularProgessCanvas = Resources.Load(canvasStringPath, typeof(GameObject)) as GameObject;
//		circularProgessCanvas = GameObject.FindGameObjectWithTag("CircularProgressCanvas");
		Debug.Log ("circularProgessCanvas : " + circularProgessCanvas.ToString());
		if(circularProgessCanvas){
			initLeftEyeCanvas();
			initRightEyeCanvas();
		}
	}

	void initLeftEyeCanvas(){
		circularProgressCanvas_LeftEye = Instantiate(circularProgessCanvas) as GameObject;
		circularProgressCanvas_LeftEye.name = "CircularProgressCanvas_LeftEye";

		Camera leftCam = GameObject.FindGameObjectWithTag("OVR_LeftEye").GetComponent<Camera>();

		Canvas canvas = circularProgressCanvas_LeftEye.GetComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = leftCam;
		canvas.planeDistance = 10.0f;

		GameObject leftHand = circularProgressCanvas_LeftEye.transform.FindChild("LeftHand").gameObject;
		GameObject rightHand = circularProgressCanvas_LeftEye.transform.FindChild("RightHand").gameObject;
		KinectOverlayer leftHandKinectOverlayer = leftHand.GetComponent<KinectOverlayer>();
		KinectOverlayer rightHandKinectOverlayer = rightHand.GetComponent<KinectOverlayer>();
		leftHandKinectOverlayer.cam = leftCam; 
		rightHandKinectOverlayer.cam = leftCam;
	}

	void initRightEyeCanvas(){
		circularProgressCanvas_RightEye = Instantiate(circularProgessCanvas) as GameObject;
		circularProgressCanvas_RightEye.name = "CircularProgressCanvas_RightEye";

		Camera rightCam = GameObject.FindGameObjectWithTag("OVR_RightEye").GetComponent<Camera>();

		Canvas canvas = circularProgressCanvas_RightEye.GetComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = rightCam;
		canvas.planeDistance = 10.0f;

		GameObject leftHand = circularProgressCanvas_RightEye.transform.FindChild("LeftHand").gameObject;
		GameObject rightHand = circularProgressCanvas_RightEye.transform.FindChild("RightHand").gameObject;
		KinectOverlayer leftHandKinectOverlayer = leftHand.GetComponent<KinectOverlayer>();
		KinectOverlayer rightHandKinectOverlayer = rightHand.GetComponent<KinectOverlayer>();
		leftHandKinectOverlayer.cam = rightCam;
		rightHandKinectOverlayer.cam = rightCam;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
#endif