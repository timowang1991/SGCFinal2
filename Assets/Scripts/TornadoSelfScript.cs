using UnityEngine;
using System.Collections;

public class TornadoSelfScript : Photon.MonoBehaviour {
	
	private int PlayerViewId;
	private Vector3 heading;
	public int speed = 30;
	public int timelimited = 10;
	// Use this for initialization
	void Start () {
		PlayerViewId = (int)photonView.instantiationData [0];
		heading = this.gameObject.transform.position - PhotonView.Find(PlayerViewId).gameObject.transform.position;
		this.gameObject.rigidbody.velocity = heading.normalized * speed;

		if(PhotonView.Find(PlayerViewId).isMine)
		{
			Invoke ("destroyself", timelimited);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("heading");
		this.GetComponentInChildren<ParticleSystem>().startSize += 0.1f;
	}
	void destroyself()
	{
		PhotonNetwork.Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.gameObject.name);
		//this.GetComponentInParent<TornadoSelfScript>()
		if(photonView.isMine)
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
