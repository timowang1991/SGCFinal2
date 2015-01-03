using UnityEngine;
using System.Collections;

public class ArrowSelfScript : MonoBehaviour {

	public enum ArrowState
	{
		holding,
		shooted,
		touched
	}
	public ArrowState state;

	// Use this for initialization
	void Start () {
		state = ArrowState.holding;
		this.gameObject.transform.localScale = new Vector3(1f,1f,1f);
		//Destroy (this.gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if (state == ArrowState.shooted) {
			if(transform.localScale.x < 30)
			{
				transform.localScale += new Vector3 (1F,1F,1F);
			}
		}
	}

	void OnCollisionEnter(Collision other) {

		if(other.gameObject.tag == "Weak")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(1,this.gameObject.tag);
		}
		else if(other.gameObject.tag == "Weaker")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(2,this.gameObject.tag);
		}
		else if(other.gameObject.tag == "Weakest")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(3,this.gameObject.tag);
		}
	}
}
