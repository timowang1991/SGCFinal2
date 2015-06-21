using UnityEngine;
using System.Collections;

public class KnightDamagedBehaviour : MonoBehaviour,IDamageOthersBehaviour {

	public float basicDamage;
	public bool causingContinuousDamage;

	static int attackState = Animator.StringToHash("Base Layer.attack");
	//

	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		AnimatorStateInfo currentBaseState = this.transform.root.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		print (currentBaseState.nameHash);
		if(currentBaseState.nameHash != attackState)
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