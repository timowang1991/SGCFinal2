using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HealthSystem : Photon.MonoBehaviour {

	public int HealthPosition;
	public GameObject Health_UI;
	public AudioClip clip;
	public int damageIncurredByGiantHand;
	Animator a;

	void Awake() {
		audio.clip = clip;
	}

	// Use this for initialization
	void Start () {
		Health_UI = GameObject.FindWithTag("HP_UI");
		SetHealthValue (100);
		a = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	private bool isDead = false;
	/// <summary>
	/// Check if is the Giant layer and hurt the value which set in the Inspector
	/// </summary>
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.layer.Equals(10)) { //this layer hurt value = 10
			if(clip != null) {
				audio.Play();
			}

			damage(damageIncurredByGiantHand);
		}
	}

	/// <summary>
	/// Check if the player is dead or just hurt, then play the animation and destory itself if is dead(No RPC - so it won't tell everyone).
	/// </summary>
	public void damage(int hurtValue) {
		int tmp = Health_UI.GetComponent<EnergyBar> ().valueCurrent - hurtValue;
		
		if(tmp<=0 && !isDead)
		{
			isDead = true;
			if(gameObject.tag != "Catapult")
			{
				a.SetBool("Die",true);
			}
			SetHealthValue(0);
			Invoke("DestroySelf",10);
		}
		else
		{
			if(gameObject.tag != "Catapult")
			{
				a.SetTrigger("Gothit");
			}
			SetHealthValue(tmp);
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
	[RPC]
	void RecoverHP (int DefaultRecoveryHPvalue)
	{
		int TotalHP = HealthPosition + DefaultRecoveryHPvalue;
		if ( TotalHP > 100) {
			this.SetHealthValue(100);
		}
		else
		{
			this.SetHealthValue(TotalHP);
		}

	}
}
