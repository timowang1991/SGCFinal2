using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(IDamagedBehaviour))]
public class HealthSystem : Photon.MonoBehaviour {

	public int HealthVal; //the health value
	public GameObject Health_UI;
	private EnergyBar energyBar;
	[HideInInspector]
	private IDamagedBehaviour damagable;

	void Awake() {
		if (Health_UI == null) {
			Health_UI = GameObject.FindWithTag("HP_UI");
		}
	}

	// Use this for initialization
	void Start () {
		/*
		if (HealthVal > 0) {
			SetHealthValue (HealthVal);
		} else {
			SetHealthValue (100);
		}
		*/
		damagable = (IDamagedBehaviour) GetComponent (typeof(IDamagedBehaviour));
	}

	private bool isDead = false;

	void OnCollisionEnter(Collision collision) {
		firstDamage (collision.gameObject);
	}
	
	void OnTriggerEnter(Collider collider) {
		firstDamage (collider.gameObject);
	}
	
	void OnCollisionStay(Collision collision) {
		continuousDamage (collision.gameObject);
	}
	
	void OnTriggerStay(Collider collider) {
		continuousDamage (collider.gameObject);
	}

	void firstDamage(GameObject objCausingDamage) {

		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)objCausingDamage.GetComponent(typeof(IDamageOthersBehaviour));
		if(damagingBehaviour != null) {
			this.damage (objCausingDamage);	
		}

	}
	
	void continuousDamage(GameObject objCausingDamage) {

		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)objCausingDamage.GetComponent(typeof(IDamageOthersBehaviour));
		if(damagingBehaviour != null && damagingBehaviour.isContinuousDamage(damagable)) {
			this.damage(objCausingDamage);
		}

	}

	/// <summary>
	/// Check if the player is dead or just hurt, then play the animation and destory itself if is dead(No RPC - so it won't tell everyone).
	/// </summary>
	public void damage (GameObject objCausingDamage) {
		if (isDead) {
			return;
		}
		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)objCausingDamage.GetComponent(typeof(IDamageOthersBehaviour));

		damagingBehaviour.beforeDamaging (damagable);
		
		int tmp = Health_UI.GetComponent<EnergyBar> ().valueCurrent - (int)damagingBehaviour.getDamageVal(damagable);
		if (tmp > 0) { 
			damagable.damaged (damagingBehaviour, tmp);
			SetHealthValue (tmp);
		} 
		else if(!isDead){
			isDead = true;
			damagable.dying (damagingBehaviour);
			SetHealthValue (0);
		}
		
		damagingBehaviour.afterDamaging (damagable);
		
	}

	public void damage(int hurtValue) {
		if (isDead) {
			return;
		}

		int tmp = Health_UI.GetComponent<EnergyBar> ().valueCurrent - hurtValue;

		if (tmp > 0) { 
				damagable.damaged (null, tmp);
				SetHealthValue (tmp);
		} else if (!isDead) {
				isDead = true;
				damagable.dying (null);
				SetHealthValue (0);
		}

	}

	//it should be only use in initialization
	public void initHP(int value) {
		HealthVal = value;
		if (energyBar == null) {
			energyBar = Health_UI.GetComponent<EnergyBar> ();
		}
		energyBar.valueCurrent = value;
		energyBar.valueMax = value;
	}

	public void SetHealthValue(int Value)
	{
		HealthVal = Value;
		if (energyBar == null) {
			energyBar = Health_UI.GetComponent<EnergyBar> ();
		}
		energyBar.valueCurrent = Value;
	}

	void DestroySelf()
	{
		PhotonNetwork.Destroy(this.gameObject);
	}

	[RPC]
	void RecoverHP (int DefaultRecoveryHPvalue)
	{
		int TotalHP = HealthVal + DefaultRecoveryHPvalue;
		if ( TotalHP > 300) {
			this.SetHealthValue(300);
		}
		else
		{
			this.SetHealthValue(TotalHP);
		}

	}
}
