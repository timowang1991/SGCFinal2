using UnityEngine;
using System.Collections;

public class CatapultDamagedBehaviour : MonoBehaviour,IDamagedBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	string IDamagedBehaviour.getDamagedClass() {
		return "Catapult";
	}
	
	void IDamagedBehaviour.damaged(IDamageOthersBehaviour damagingBehaviour, int currentHP) {
		
	}
	
	void IDamagedBehaviour.dying(IDamageOthersBehaviour damagingBehaviour) {
		
	}
	
	
}
