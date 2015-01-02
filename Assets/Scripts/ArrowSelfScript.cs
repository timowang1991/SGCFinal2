using UnityEngine;
using System.Collections;

public class ArrowSelfScript : MonoBehaviour {

	public int WeakPoint;
	public int WeakerPoint;
	public int WeakestPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.tag == "Weak")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakPoint);
		}
		else if(other.gameObject.tag == "Weaker")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakerPoint);
		}
		else if(other.gameObject.tag == "Weakest")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakestPoint);
		}
	}
}
