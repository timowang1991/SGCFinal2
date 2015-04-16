using UnityEngine;

interface IDamageOthersBehaviour {

	float getDamageVal(IDamagedBehaviour damagable);
	bool isContinuousDamage(IDamagedBehaviour damagable);
	//callback
	void beforeDamaging (IDamagedBehaviour damagable);
	void afterDamaging (IDamagedBehaviour damagable);

}