using UnityEngine;
using System.Collections;

public class GiantHealth : MonoBehaviour {

	public int healthPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void loseHealthPoint(int losePoint){
		healthPoint -= losePoint;
		if(healthPoint <= 0){
			death ();
		}
	}

	void death(){

	}
}
