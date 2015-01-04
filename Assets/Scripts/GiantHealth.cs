using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiantHealth : MonoBehaviour {

	public int healthPoint;
	public float Factor;
	public int minHurtPoint;
	private static PhotonView GiantPhotonView;
	private EnergyBar energyBar = null;
	private Text leftTxt = null;
	private Text rightTxt = null;
	private Platform platform;

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
	void Start () {
		GiantPhotonView = this.GetComponent<PhotonView>();
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		energyBar = GameObject.Find ("HP_UI_Giant/Bar4").GetComponent<EnergyBar>();
		if (platform == Platform.PC_Giant)
		{
			leftTxt = GameObject.Find ("Text_Left/Text").GetComponent<Text> ();
			rightTxt = GameObject.Find ("Text_Right/Text").GetComponent<Text> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

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
		else if(leftTxt != null && rightTxt != null)
		{
			leftTxt.text = "HP "+healthPoint +"/1000";
			rightTxt.text = "HP "+healthPoint +"/1000";
		}
	}

	void death(){

	}
	
	// int valus 1=Weak 2=Weaker 3=Weakest
	public void HurtGiant(int HurtWeakLevel , string Thing)
	{
		GiantPhotonView.RPC("RPCHurtGiant",PhotonTargets.All ,HurtWeakLevel,Thing);
	}

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
	}

}
