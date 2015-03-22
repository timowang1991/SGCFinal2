using UnityEngine;
using System.Collections;

public class FireBallSelfScript : Photon.MonoBehaviour {


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
	void OnParticleCollision(GameObject other)
	{
		if(photonView.isMine)
		{
			if(other.gameObject.tag == "Weak")
			{
				Debug.Log ("Fireball: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(1,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weaker")
			{
				Debug.Log ("Fireball: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(2,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weakest")
			{
				Debug.Log ("Fireball: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(3,this.gameObject.tag);
			}
		}
	}

}
