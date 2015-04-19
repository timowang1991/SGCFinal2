using UnityEngine;
using System.Collections;

public class GiantDamagedBehaviour : MonoBehaviour,IDamagedBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string IDamagedBehaviour.getDamagedClass() {
		return "Giant";
	}
	
	void IDamagedBehaviour.damaged(IDamageOthersBehaviour damagingBehaviour, int currentHP) {

	}
	
	void IDamagedBehaviour.dying(IDamageOthersBehaviour damagingBehaviour) {

	}


}
