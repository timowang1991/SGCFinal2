using UnityEngine;
using System.Collections;

public class TornadoCollision : MonoBehaviour {

	void OnParticleCollision(GameObject other) {
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
