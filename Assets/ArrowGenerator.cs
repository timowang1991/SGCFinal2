using UnityEngine;
using System.Collections;

public class ArrowGenerator : MonoBehaviour {


	Animator a;
	CapsuleCollider col;
	Transform hero;
	Rigidbody rb;
	bool isLoadArrow = false;
	public GameObject Arrow;
	public Transform Bow;
	public Transform R_hand;
	public float Speed;
	public Transform TargetPoint;
	public float ShootTime;


	public int WeakPoint;
	public int WeakerPoint;
	public int WeakestPoint;

	// Use this for initialization
	void Start () {
		a = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		hero = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
	}

	GameObject Arrow_clone;

	// Update is called once per frame
	void Update () {
		if (this.GetComponent<HeroCtrl_Net2> ().baseState == HeroCtrl_Net2.BaseState.Combat && a.GetBool ("Attack1") && isLoadArrow == false && Arrow_clone==null) {
			Debug.Log("Attack");
			Arrow_clone =  (GameObject)Instantiate (Arrow, Bow.position , transform.rotation);
//			Arrow_clone.transform.localScale = new Vector3(10,10,10);

			Arrow_clone.transform.parent= Bow.transform;
			isLoadArrow=true;

			Vector3 relativePos = Bow.transform.position - R_hand.transform.position;
			Arrow_clone.transform.rotation = Quaternion.LookRotation(relativePos,Bow.transform.right);//;//,Bow.transform.right);
			Arrow_clone.transform.rotation *= Quaternion.Euler(240, 0, 0); 
			Arrow_clone.collider.enabled = false;

			Arrow_clone.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.holding;

			Invoke("Shoot_Arrow",ShootTime);
		}
	}
	public void Shoot_Arrow()
	{
		isLoadArrow=false;
		Arrow_clone.transform.parent = null;
		Arrow_clone.rigidbody.isKinematic = false;
		Arrow_clone.rigidbody.useGravity = true;

		Vector3 test =  TargetPoint.transform.position - R_hand.transform.position;

		Arrow_clone.rigidbody.velocity = Speed * transform.forward;//test/test.magnitude;
		Arrow_clone.collider.enabled = true;
		Arrow_clone.GetComponent<ArrowSelfScript>().state = ArrowSelfScript.ArrowState.shooted;
		//Destroy (Arrow_clone, 10);
		Arrow_clone = null;
	}

	void OnTriggerEnter(Collider other) {
//		if(other.gameObject.tag == "Weak")
//		{
//			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakPoint);
//		}
//		else if(other.gameObject.tag == "Weaker")
//		{
//			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakerPoint);
//		}
//		else if(other.gameObject.tag == "Weakest")
//		{
//			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().loseHealthPoint(WeakestPoint);
//		}
	}
}
