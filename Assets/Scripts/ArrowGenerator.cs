using UnityEngine;
using System.Collections;

public class ArrowGenerator : Photon.MonoBehaviour {
	
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
	public float scaleOfArrow;
	public float angleToRot;
	private Transform camTrans;

	public Transform mountedPosOnBow;
	private Transform placeToPutArrow;
	
	// Use this for initialization
	void Start () {
		a = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		hero = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		parasArrayForClonedArrow [0] = photonView.viewID;
		camTrans = Camera.main.transform;
		placeToPutArrow = mountedPosOnBow;
		//placeToPutArrow.position += placeToPutArrow.forward * 1f;
	}

	GameObject Arrow_clone;
	private object[] parasArrayForClonedArrow = new object[1];
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<HeroCtrl_Net2> ().baseState == HeroCtrl_Net2.BaseState.Combat && a.GetBool ("Attack1") && isLoadArrow == false && Arrow_clone==null) {
			Debug.Log("Attack");
//			Arrow_clone =  (GameObject)Instantiate (Arrow, Bow.position , transform.rotation);
			Arrow_clone = PhotonNetwork.Instantiate("Arrow_Net", placeToPutArrow.position, Quaternion.identity, 0, parasArrayForClonedArrow);
			//transfer arrow_clone setup into Arrow_NetSync on the Arrow

			isLoadArrow=true;
			Invoke("Shoot_Arrow",ShootTime);
		}
	}
	public void Shoot_Arrow()
	{
		Vector3 direction =  camTrans.forward;
		Arrow_clone.GetComponent<PhotonView> ().RPC ("beShotOutRPC",PhotonTargets.All,direction);
		isLoadArrow=false;
		Arrow_clone.GetComponent<Arrow_NetSync> ().invokeDestroy ();
		Arrow_clone = null;
	}
	
}

