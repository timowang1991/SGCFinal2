using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using PathologicalGames;

public class GameConfig : MonoBehaviour {

	public GameObject treeObject; //tree prefab

	public static string giantGameObjectName = "52bangd-T-Pose";
	public static string PCGiantTag = "OVR";
	public static string PhoneTag = "CNC";

	public static string CharCopter = "Copter";
	public static string CharMagician = "Magician";
	public static string CharArcher = "Archer";
	public static string CharCatapult = "Catapult";

	public static string CopterPoolName = "CopterRelated";

	public HashSet<string> bloodyable;

//	public static int maxNumBullets = 20;
//	public static int maxNumMissiles = 3;
//	public static int maxNumDetonator = 4;

	public static int fireDamageVal = 20;

	
	public static string GetFunctionName(Action method) {
		return method.Method.Name;
	}

	/// <summary>
	/// Just tell debug that there wasn't a object in the inspector
	/// </summary>
	void Awake() {
		if(treeObject == null) {
			Debug.LogError ("You forget to set tree object");
		}

		bloodyable = new HashSet<string> ();
		bloodyable.Add ("Human");
		bloodyable.Add ("Giant");

	}

}
