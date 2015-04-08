using UnityEngine;
using System.Collections;

public class TornadoSelfScript : Photon.MonoBehaviour {

	public Transform PlayerPosition;
	private Vector3 heading;
	public int speed = 30;
	public int timelimited = 10;
	// Use this for initialization
	void Start () {
		//Debug.Log("start");
		heading = this.gameObject.transform.position - PlayerPosition.transform.position;
		Invoke ("destroyself", timelimited);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("heading");
		this.gameObject.rigidbody.velocity = heading.normalized * speed;
		this.GetComponentInChildren<ParticleSystem>().startSize += 0.1f;

	}
	void destroyself()
	{
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.gameObject.name);
		if(this.GetComponentInParent<TornadoSelfScript>().photonView.isMine)
		{
			if(other.gameObject.tag == "Weak")
			{
				Debug.Log ("Tornado: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(1,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weaker")
			{
				Debug.Log ("Tornado: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(2,this.gameObject.tag);
			}
			else if(other.gameObject.tag == "Weakest")
			{
				Debug.Log ("Tornado: " + other.gameObject.tag);
				GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().HurtGiant(3,this.gameObject.tag);
			}
		}
	}
}
