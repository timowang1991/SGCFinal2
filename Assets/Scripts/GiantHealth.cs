using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiantHealth : MonoBehaviour {

	public int healthPoint;
	public float Factor;
	public int minHurtPoint;
	private PhotonView GiantPhotonView;
	private EnergyBar energyBar = null;
	private Text leftTxt = null;
	private Text rightTxt = null;
	private Platform platform;
	private IDamagedBehaviour damagable;

	/// <summary>
	/// Called by BigLittleGameLogic, and just do ui thing and set default HP
	/// </summary>
	[RPC]
	public void setInitialHP(int HP) {
		Debug.Log ("set initial HP");
		healthPoint = HP;
		if(energyBar == null) {
			energyBar = GameObject.Find ("HP_UI_Giant/Bar4").GetComponent<EnergyBar>();
		}
		energyBar.SetValueCurrent (healthPoint);
	}

	// Use this for initialization
	// 
	/// <summary>
	/// Get the platform, determine which UI has to be shown at the screen.
	/// </summary>
	void Start () {
		GiantPhotonView = this.GetComponent<PhotonView>();
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if (platform == Platform.PC_Giant)
		{
			leftTxt = GameObject.Find ("Text_Left/Text").GetComponent<Text> ();
			rightTxt = GameObject.Find ("Text_Right/Text").GetComponent<Text> ();
		}
		else
		{
			energyBar = GameObject.Find ("HP_UI_Giant/Bar4").GetComponent<EnergyBar>();
		}
		damagable = (IDamagedBehaviour)GetComponent(typeof(IDamagedBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
//		if(GetComponent<Animator> ().enabled == true)
//		{
//			this.gameObject.transform.parent.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.parent.gameObject.transform.position, new Vector3(250,40,-100),  Time.deltaTime*1.5f);
//		}
	}


	/// <summary>
	/// lose how many HP and Update UI, and if Giant dead, play the animation to dead (since the Giant default animation is dead, so it only plays the dead animation.)
	/// </summary>
	void loseHealthPoint(int losePoint){

		if(healthPoint - losePoint <= 0)
		{
			healthPoint = 0;
			death();
		}
		else
		{
			healthPoint -= losePoint;
		}
		if(energyBar != null)
		{
			energyBar.SetValueCurrent (healthPoint);
		}
//		else if(leftTxt != null && rightTxt != null)
//		{
//			leftTxt.text = "HP "+healthPoint +"/1000";
//			rightTxt.text = "HP "+healthPoint +"/1000";
//		}
	}

	/// <summary>
	/// Enable animation (Giant)
	/// </summary>
	void death(){
		//GetComponent<Animator> ().enabled = true;
	}

	public void damage(GameObject objCausingDamage) {
		Debug.Log (objCausingDamage.name);
		IDamageOthersBehaviour damagingBehaviour = (IDamageOthersBehaviour)objCausingDamage.GetComponent (typeof(IDamageOthersBehaviour));
		if(objCausingDamage.tag == "GiantGrabbableStone"){
			return;
		}
		damagingBehaviour.beforeDamaging (damagable);
		if (GiantPhotonView.isMine) { //send RPC only in Giant-Controlled Scene, so make sure collision is normally happen in Giant-Controlled Scene
			GiantPhotonView.RPC ("damageRPC", PhotonTargets.All, damagingBehaviour.getDamageVal (damagable));
		}
		damagingBehaviour.afterDamaging (damagable);
	}

	public void damage(float damageVal) {
		if (GiantPhotonView.isMine) {
			GiantPhotonView.RPC ("damageRPC", PhotonTargets.All, damageVal);
		}
	}

	[RPC]
	void damageRPC(float damageVal) {
		int currentHP = healthPoint - (int)damageVal;
		if (currentHP < 0) {
			currentHP = 0;
			damagable.dying (null);
		} else {
			damagable.damaged (null, currentHP);
		}

		if(energyBar != null)
		{
			energyBar.SetValueCurrent (currentHP);
		}
		else if(leftTxt != null && rightTxt != null)
		{
			leftTxt.text = "HP "+currentHP +"/1000";
			rightTxt.text = "HP "+currentHP +"/1000";
		}

		healthPoint = currentHP;
	}
	
	// int valus 1=Weak 2=Weaker 3=Weakest
	/// <summary>
	/// Called by StoneSelfScript and ArrowSelfScript, only that player call this to tell everyone to hurt the giant's HP
	/// </summary>
	public void HurtGiant(int HurtWeakLevel , string Thing)
	{
		GiantPhotonView.RPC("RPCHurtGiant",PhotonTargets.All ,HurtWeakLevel,Thing);
	}

	/// <summary>
	/// Everyone plays this, and hurt different value by different object
	/// </summary>
	[RPC]
	void RPCHurtGiant(int HurtWeakLevel , string Thing)
	{
		if(Thing == "Stone")
		{
			loseHealthPoint((int)(minHurtPoint * HurtWeakLevel * Factor));
		}
		else if(Thing == "Arrow")
		{
			loseHealthPoint(minHurtPoint * HurtWeakLevel);
		}
		else if(Thing == "FireBall")
		{
			loseHealthPoint(minHurtPoint * HurtWeakLevel);
		}
		else if(Thing == "Tornado")
		{
			loseHealthPoint(minHurtPoint * HurtWeakLevel);
		}
	}


}
