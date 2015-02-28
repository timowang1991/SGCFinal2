using UnityEngine;
using System.Collections;

public class MagicianMPManager : MonoBehaviour {

	public int initMPvalue = 100;
	public int currentValue;
	private MagicianUIManager UI;

	private float currentCountTime = 0;
	public float intervalRefreshMP = 1;
	public int HowManyMpAdd = 1;

	// Use this for initialization
	void Start () {
		currentValue = initMPvalue;
		UI = this.gameObject.GetComponent<MagicianUIManager> ();
		if ( UI== null) {
			Debug.LogError("UIManager not loaded");	
		}
	}

	void Update()
	{
		currentCountTime += Time.deltaTime;

		if(currentCountTime >= intervalRefreshMP)
		{
			currentCountTime = 0;
			currentValue += HowManyMpAdd;
			UI.UpdateUI(true);
		}
	}

	public bool UseMP (int value)
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
