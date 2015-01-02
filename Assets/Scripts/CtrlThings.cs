using UnityEngine;
using System.Collections;
using HeroCtrlAlias = HeroCtrl_Net2;

public class CtrlThings : MonoBehaviour {
	
	Animator a;
	Transform hero;
	Rigidbody rb;
	CapsuleCollider col;
	
	[HideInInspector]
	public float h;
	[HideInInspector]
	public float v;
	[HideInInspector]
	public float mX;
	bool doJumpDown;
	//bool doJump;
	bool doAtk1Down;
	bool doAtk1;
	bool doAtk2Down;
	bool doAtk2;
	bool doFwd;
	bool doBack;
	bool doLeft;
	bool doRight;
	bool doNextWeapon;
	bool doCombat;
	bool doFly;
	bool doClimb;
	bool doWalk;
	bool doSprint;
	bool doSneak;
	bool doLShift;
	bool doDance1;	
	bool doDance2;
	bool doDance3;
	bool doPullLever;
	bool doPushButton;
	bool doThrow;

	GameObject cam;

	HeroCtrlAlias.BaseState tmp;
	HeroCtrlAlias netCtrl;

	enum BaseState
	{
		Base,
		Climb,
		Swim,
		Fly,
		Combat,
		PhysX,
		Catapults
	}

	void Awake() {
		netCtrl = GetComponent<HeroCtrlAlias>();
	}

	// Use this for initialization
	void Start () {
		a = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		hero = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();

		cam = GameObject.FindGameObjectWithTag("MainCamera"); // Your characters's camera tag

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		mX = Input.GetAxis("Mouse X"); // Mouse X is 0 if leftShift is held down
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");	
		doJumpDown = Input.GetButtonDown("Jump");
		doFwd = Input.GetKeyDown(KeyCode.W);
		doBack = Input.GetKeyDown(KeyCode.S);
		doLeft = Input.GetKeyDown(KeyCode.A);
		doRight = Input.GetKeyDown(KeyCode.D);

		switch (netCtrl.baseState) {
			case HeroCtrlAlias.BaseState.Catapults:
				//Debug.Log("Catapults");
				Catapult();
				break;
		}
	}
	
	
	//self add start
	
	GameObject ThingToControl;
	public void ChangeToCatapult(GameObject gameobject)
	{
		ThingToControl = gameobject;
		tmp = netCtrl.baseState;
		netCtrl.baseState = HeroCtrlAlias.BaseState.Catapults;

		col.enabled = false;
		rb.useGravity = false;
	}
	void setShootNo()
	{
		ThingToControl.GetComponent<Animator>().SetBool("Shoot_Stone",false);
	}
	
	public void Catapult()
	{
		//Debug.Log (ThingToControl.transform.Find("HeroPosition").transform.position);
		hero.position = ThingToControl.transform.Find("HeroPosition").transform.position;
		
		cam.transform.position = ThingToControl.transform.Find ("CamToPut").transform.position;
		if(mX != 0.0f)
		{
			hero.Rotate (0, mX * Time.fixedDeltaTime * 1 * 55, 0, Space.Self);
			ThingToControl.transform.rotation = hero.rotation;
		}
		if (doJumpDown) {
			//Debug.Log ("shoot");
			ThingToControl.GetComponent<Animator>().SetBool("Shoot_Stone",true);
			Invoke("setShootNo",2);
			//a.SetBool("Shoot",true);
		}
		else if(doFwd || doBack || doLeft || doRight)
		{
			//a.runtimeAnimatorController = animCtrl.baseC;
			
			a.SetBool("Bow", true);
			netCtrl.baseState = tmp;
			
			col.enabled = true;
			rb.useGravity = true;
			
			//hero.renderer.enabled = true;
		}
	}
	//self add end
}
