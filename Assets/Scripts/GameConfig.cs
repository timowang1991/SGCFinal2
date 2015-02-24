using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour {

	public GameObject treeObject;

	public static string giantGameObjectName = "52bangd-T-Pose";

	/// <summary>
	/// Just tell debug that there wasn't a object in the inspector
	/// </summary>
	void Awake() {
		if(treeObject == null) {
			Debug.LogError ("You forget to set tree object");
		}
	}

}
