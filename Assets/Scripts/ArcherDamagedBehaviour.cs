using UnityEngine;
using System.Collections;

public class ArcherDamagedBehaviour : MonoBehaviour,IDamagedBehaviour {

	Animator animator;
	
	void Awake() {
		animator = GetComponent<Animator> ();
	}
	
	string IDamagedBehaviour.getDamagedClass() {
		return "Human";
	}
	
	void IDamagedBehaviour.damaged(IDamageOthersBehaviour damagingBehaviour, int currentHP) {
		animator.SetTrigger("Gothit");
	}
	
	void IDamagedBehaviour.dying(IDamageOthersBehaviour damagingBehaviour) {
		animator.SetBool ("Die", true);
	}
}
