using UnityEngine;
using System.Collections;

public class MagicianMPManager : MonoBehaviour {

	public float initMPvalue = 100;
	public float currentValue;
	private MagicianUIManager UI;

	// Use this for initialization
	void Start () {
		currentValue = initMPvalue;
		UI = this.gameObject.GetComponent<MagicianUIManager> ();
		if ( UI== null) {
			Debug.LogError("UIManager not loaded");	
		}
	}
	public bool UseMP (float value)
	{
		if (currentValue - value < 0) {
			UI.UpdateUI(false);
			return false;
		}
		else
		{
			currentValue -= value;
			UI.UpdateUI(true);
			return true;
		}
	}
}
