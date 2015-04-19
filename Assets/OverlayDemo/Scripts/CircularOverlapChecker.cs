using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircularOverlapChecker : MonoBehaviour {
	public Image hand;
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
//		Debug.Log("overlaps check : " + circularRange.rectTransform.rect.Overlaps(hand.rectTransform.rect));
//		Debug.Log("circularRange.rectTransform.rect : " + circularRange.rectTransform.rect);
//		Debug.Log ("hand.rectTransform.rect : " + hand.rectTransform.rect);
//		if(circularRange.rectTransform.rect.Overlaps(hand.rectTransform.rect)){
//			if(!inRange){
//				circularBar.RestartLoading();
//				inRange = true;
//			}
//		} else {
//			if(inRange){
//				circularBar.UnLoad();
//			}
//			inRange = false;
//		}
		//		Debug.Log("ball transform.position = " + transform.position);
		//		Debug.Log("ball" + Camera.main.WorldToScreenPoint(transform.position));
		//		Debug.Log("circularRange.rectTransform.position = " + circularRange.rectTransform.position);
//		Vector3 ballScreenPos = cam.WorldToScreenPoint(transform.position);
		//		Debug.Log("ballScreenPos = " + ballScreenPos);
//		if(transform.position.x >= circularRange.rectTransform.position.x - circularRange.rectTransform.rect.width/2 &&
//		   transform.position.x <= circularRange.rectTransform.position.x + circularRange.rectTransform.rect.width/2 &&
//		   transform.position.y >= circularRange.rectTransform.position.y - circularRange.rectTransform.rect.height/2 &&
//		   transform.position.y <= circularRange.rectTransform.position.y + circularRange.rectTransform.rect.height/2){
//			//			Debug.Log("ball in range");
//			if(!inRange){
//				circularBar.RestartLoading();
//				inRange = true;
//			}
//		} else {
//			if(inRange){
//				circularBar.UnLoad();
//			}
//			inRange = false;
//			
//		}
//		Debug.Log ("hand.rectTransform.localPosition.x = " + hand.rectTransform.localPosition.x);
//		Debug.Log ("hand.rectTransform.localPosition.y = " + hand.rectTransform.localPosition.y);
//		Debug.Log ("circularRange.rectTransform.localPosition.x = " + circularRange.rectTransform.localPosition.x);
//		Debug.Log ("circularRange.rectTransform.localPosition.y = " + circularRange.rectTransform.localPosition.y);
		if(hand.rectTransform.localPosition.x >= circularRange.rectTransform.localPosition.x - circularRange.rectTransform.rect.width/2 &&
		   hand.rectTransform.localPosition.x <= circularRange.rectTransform.localPosition.x + circularRange.rectTransform.rect.width/2 &&
		   hand.rectTransform.localPosition.y >= circularRange.rectTransform.localPosition.y - circularRange.rectTransform.rect.height/2 &&
		   hand.rectTransform.localPosition.y <= circularRange.rectTransform.localPosition.y + circularRange.rectTransform.rect.height/2){
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
