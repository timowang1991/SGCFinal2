using UnityEngine;
using System.Collections;

public class FireBallSelfScript : MonoBehaviour {


	public float timeToDestory = 10;
	// Use this for initialization
	void Start () {
		Invoke ("selfDestory", timeToDestory);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void selfDestory()
	{
		Destroy (this.gameObject);
	}
}
