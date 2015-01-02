using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour {

	public GameObject treeObject;

	void Awake() {
		if(treeObject == null) {
			Debug.LogError ("You forget to set tree object");
		}
	}

}
