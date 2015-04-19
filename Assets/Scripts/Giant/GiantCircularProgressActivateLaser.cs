using UnityEngine;
using System.Collections;

public class GiantCircularProgressActivateLaser : MonoBehaviour {
	public bool activatingScript;

	CircularBarWithTimer [] circles;
	byte numOfCircleComplete = 0;
	bool completed = false;

	// Use this for initialization
	void Start () {
		circles = GetComponentsInChildren<CircularBarWithTimer>();
		foreach(CircularBarWithTimer circle in circles){
			circle.OnEnter100PercentNotifier += Reach100Percent;
			circle.OnExit100PercentNotifier += Exit100Percent;
			circle.OnFadeCompleteNotifier += FadeComplete;
		}
	}

	void Update(){}

	void Reach100Percent(){
		numOfCircleComplete++;
		if(numOfCircleComplete == 2 && !completed){
			completed = true;
			foreach(CircularBarWithTimer circle in circles){
				circle.Fade();
			}

			if(activatingScript){

			}
		}
	}

	void Exit100Percent(){
		numOfCircleComplete--;
	}

	void FadeComplete(){
		Destroy(this.gameObject);
	}
}
