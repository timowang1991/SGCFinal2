using UnityEngine;
using System.Collections;

public class GiantCircularProgressActivateLaser : MonoBehaviour {
	public bool activatingScript;

	CircularBarWithTimer [] circles;
	byte numOfCircleComplete = 0;
	bool completed = false;
	
	LaserByCameraAndTimer [] lasers;

	// Use this for initialization
	void Start () {
		circles = GetComponentsInChildren<CircularBarWithTimer>();
		circularProgressCallbackRegistration();
		findLaserGameObjects();
	}

	void circularProgressCallbackRegistration(){
		foreach(CircularBarWithTimer circle in circles){
			circle.OnEnter100PercentNotifier += Reach100Percent;
			circle.OnExit100PercentNotifier += Exit100Percent;
			circle.OnFadeCompleteNotifier += FadeComplete;
		}
	}

	void findLaserGameObjects(){
		GameObject [] laserGameObjects = GameObject.FindGameObjectsWithTag("Laser");
		lasers = new LaserByCameraAndTimer[laserGameObjects.Length];
		int i = 0;
		foreach(GameObject gObject in laserGameObjects){
			lasers[i++] = gObject.GetComponent<LaserByCameraAndTimer>();
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
				foreach(LaserByCameraAndTimer laser in lasers){
					laser.TimerToUseLaser = 5.0f;
				}
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
