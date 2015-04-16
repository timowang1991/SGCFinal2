using UnityEngine;
using System.Collections;

public class defaultDamageBehaviour : MonoBehaviour,IDamageOthersBehaviour {

	public float basicDamage;
	public bool causingContinuousDamage;

	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		return basicDamage;	
	}
	
	bool IDamageOthersBehaviour.isContinuousDamage(IDamagedBehaviour damagable) {
		return causingContinuousDamage;
	}
	
	void IDamageOthersBehaviour.beforeDamaging (IDamagedBehaviour damagable) {}
	void IDamageOthersBehaviour.afterDamaging (IDamagedBehaviour damagable) {}

}
