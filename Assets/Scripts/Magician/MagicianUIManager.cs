using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicianUIManager : MonoBehaviour {

	private MagicianMPManager MP;

	// Use this for initialization
	void Start () {
		MP = this.gameObject.GetComponent<MagicianMPManager> ();
	}

	/// <summary>
	/// Updates the UI, and true means value is sufficient to change, otherwise show up UI to notify User not sufficient.
	/// </summary>
	/// <param name="sufficient">If set to <c>true</c> sufficient.</param>
	public void UpdateUI(bool sufficient)
	{
		Debug.LogWarning ("Update MP value is " + MP.currentValue + ", and Need to UpdateUI and implement");
	}
}
