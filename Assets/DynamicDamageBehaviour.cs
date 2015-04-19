using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DynamicDamageBehaviour : MonoBehaviour,IDamageOthersBehaviour {

	public float basicDamage = 50;
	private bool causingContinuousDamage = false;
	private SphereCollider sCollider;

	float IDamageOthersBehaviour.getDamageVal(IDamagedBehaviour damagable) {
		return basicDamage;	
	}
	
	bool IDamageOthersBehaviour.isContinuousDamage(IDamagedBehaviour damagable) {
		return causingContinuousDamage;
	}
	
	void IDamageOthersBehaviour.beforeDamaging (IDamagedBehaviour damagable) {}
	void IDamageOthersBehaviour.afterDamaging (IDamagedBehaviour damagable) {}

	// Use this for initialization
	void Start () {
		sCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		sCollider.radius += (3 * Time.deltaTime);
		basicDamage = basicDamage / sCollider.radius;
	}



}
