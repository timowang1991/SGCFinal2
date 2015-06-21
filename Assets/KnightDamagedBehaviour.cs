using UnityEngine;
using System.Collections;

public class KnightDamagedBehaviour : MonoBehaviour,IDamageOthersBehaviour {

	public float basicDamage;
	public bool causingContinuousDamage;

	static int attackState = Animator.StringToHash("Base.attack");
	//

	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		if(this.transform.root.gameObject.GetComponent<Animator>().GetHashCode() == attackState.GetHashCode())
		{
			return 0;
		}
		return basicDamage;	
	}
	
	bool IDamageOthersBehaviour.isContinuousDamage(IDamagedBehaviour damagable) {
		return causingContinuousDamage;
	}
	
	void IDamageOthersBehaviour.beforeDamaging (IDamagedBehaviour damagable) {}
	void IDamageOthersBehaviour.afterDamaging (IDamagedBehaviour damagable) {}
}