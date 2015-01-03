using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour {

	public int HealthPosition;
	public int hurtValue;
	public GameObject Health_UI;

	Animator a;

	// Use this for initialization
	void Start () {
		Health_UI = GameObject.FindWithTag("HP_UI");
		SetHealthValue (100);
		a = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log (collision.gameObject.layer);
		if (collision.gameObject.layer.Equals(10)) {

			int tmp = Health_UI.GetComponent<EnergyBar> ().valueCurrent - hurtValue;

			if(tmp<=0)
			{
				a.SetBool("Die",true);
				SetHealthValue(0);
				Invoke("DestroySelf",10);
			}
			else
			{
				a.SetTrigger("Gothit");
				SetHealthValue(tmp);
			}
		}
	}

	void SetHealthValue(int Value)
	{
		HealthPosition = Value;
		Health_UI.GetComponent<EnergyBar> ().valueCurrent = Value;
	}

	void DestroySelf()
	{
		PhotonNetwork.Destroy(this.gameObject);
	}
}
