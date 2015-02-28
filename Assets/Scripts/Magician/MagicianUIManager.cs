using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicianUIManager : MonoBehaviour {

	private MagicianMPManager MP;
	private GameObject MP_Canvas;
	private EnergyBar MP_UI;
	private PhotonView pv;

	public GameObject MP_UI_prefab;

	// Use this for initialization
	void Start () {
		MP = this.gameObject.GetComponent<MagicianMPManager> ();
		pv = PhotonView.Get(this);
		if (pv.isMine) {
			MP_Canvas = (GameObject)GameObject.Instantiate (MP_UI_prefab);
			MP_UI = MP_Canvas.GetComponentInChildren<EnergyBar>();
		}
	}

	/// <summary>
	/// Updates the UI, and true means value is sufficient to change, otherwise show up UI to notify User not sufficient.
	/// </summary>
	/// <param name="sufficient">If set to <c>true</c> sufficient.</param>
	public void UpdateUI(bool sufficient)
	{
		if (pv.isMine) {
			if (sufficient == false) {
				Debug.LogWarning ("Notify User that MP is not sufficient");
			}
			else
			{
				MP_UI.SetValueCurrent(MP.currentValue);
			}
		}
	}
}
