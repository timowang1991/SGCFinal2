using UnityEngine;
using System.Collections;

public class KnightDamagedBehaviour : MonoBehaviour,IDamageOthersBehaviour {

	public float basicDamage;
	public bool causingContinuousDamage;
	int animatorAttackCode = Animator.StringToHash("Base.attack");
	
	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		if (this.transform.root.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).GetHashCode () == animatorAttackCode) {
			return basicDamage;
		}
		else
		{
			return 0;
		}
	}
	
	bool IDamageOthersBehaviour.isContinuousDamage(IDamagedBehaviour damagable) {
		return causingContinuousDamage;
	}
	
	void IDamageOthersBehaviour.beforeDamaging (IDamagedBehaviour damagable) {}
	void IDamageOthersBehaviour.afterDamaging (IDamagedBehaviour damagable) {}
}