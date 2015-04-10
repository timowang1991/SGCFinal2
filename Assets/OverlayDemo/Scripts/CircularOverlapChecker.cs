using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircularOverlapChecker : MonoBehaviour {
	public Camera cam;
	public Image circularRange;
	public Image circularProgress;
	
	CircularBarWithTimer circularBar;
	bool inRange = false;
	
	
	// Use this for initialization
	void Start () {
		circularBar = circularProgress.GetComponent<CircularBarWithTimer>();
	}
	
	// Update is called once per frame
	void Update () {
		//		Debug.Log("ball transform.position = " + transform.position);
		//		Debug.Log("ball" + Camera.main.WorldToScreenPoint(transform.position));
		//		Debug.Log("circularRange.rectTransform.position = " + circularRange.rectTransform.position);
		Vector3 ballScreenPos = cam.WorldToScreenPoint(transform.position);
		//		Debug.Log("ballScreenPos = " + ballScreenPos);
		if(transform.position.x >= circularRange.rectTransform.position.x - circularRange.rectTransform.rect.width/2 &&
		   transform.position.x <= circularRange.rectTransform.position.x + circularRange.rectTransform.rect.width/2 &&
		   transform.position.y >= circularRange.rectTransform.position.y - circularRange.rectTransform.rect.height/2 &&
		   transform.position.y <= circularRange.rectTransform.position.y + circularRange.rectTransform.rect.height/2){
			//			Debug.Log("ball in range");
			if(!inRange){
				circularBar.RestartLoading();
				inRange = true;
			}
		} else {
			if(inRange){
				circularBar.UnLoad();
			}
			inRange = false;
			
		}
	}
}
