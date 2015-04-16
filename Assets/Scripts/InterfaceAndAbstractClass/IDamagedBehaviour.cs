using UnityEngine;

interface IDamagedBehaviour {

	string getDamagedClass(); //one can use this class to play some fx based on class 
	void damaged (IDamageOthersBehaviour damagingBehaviour, int currentHP);
	void dying (IDamageOthersBehaviour damagingBehaviour);

}