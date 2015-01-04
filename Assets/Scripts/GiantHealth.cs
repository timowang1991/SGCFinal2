using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiantHealth : MonoBehaviour {

	public int healthPoint;
	public float Factor;
	public int minHurtPoint;
	private static PhotonView GiantPhotonView;

	// Use this for initialization
	void Start () {
		GiantPhotonView = this.GetComponent<PhotonView>();
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
		if(GameObject.Find ("HP_UI_Giant/Bar4"))
		{
			GameObject.Find ("HP_UI_Giant/Bar4").GetComponent<EnergyBar> ().SetValueCurrent (healthPoint);
		}
		else if(GameObject.Find ("Text_Left"))
		{
			GameObject.Find ("Text_Left/Text").GetComponent<Text> ().text = "HP "+healthPoint +"/1000";
			GameObject.Find ("Text_Right/Text").GetComponent<Text> ().text = "HP "+healthPoint +"/1000";
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
