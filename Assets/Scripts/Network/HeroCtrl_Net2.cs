using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]  
[RequireComponent(typeof(CapsuleCollider))]  
[RequireComponent(typeof(Rigidbody))]  

public class HeroCtrl_Net2 : Photon.MonoBehaviour 
{
	[HideInInspector]
	public bool isControllable = false;

	public LayerMask groundLayers = -1;
	public LayerMask wallRunLayers = -1;
	public bool canRotate = true;
	public float rotSpeed = 90.0f;
	public float baseDrag = 0;
	public float animSpeed = 1.2f;
	public float moveSpeed = 1.5f;
	public bool canJump = true;
	public float jumpHeight = 4.0f;
	public float groundedDistance = 1.5f; // from capsule center
	public float setFloatDampTime = 0.15f;
	
	// Double Tap ----------------------- //
	public bool canEvade = true;
	public float doubleTapSpeed = 0.3f; // Time between the taps frames*2
	bool isDoubleTap = false;
	bool is2Fwd = false;
	bool is2Back = false;
	bool is2Left = false;
	bool is2Right = false;
	
	// WallRun ----------------------- //
	public bool canWallRun = true;
	
	
	public enum BaseState
	{
		Base,
		Climb,
		Swim,
		Fly,
		Combat,
		PhysX,
		Catapults
	}
	public BaseState baseState = BaseState.Base;
	
	public enum WeaponState
	{
		None,
		Sword,
		Bow,
		Rifle,
		Pistol,
		//Unarmed,
		//Throw
	}
	[HideInInspector]
	public WeaponState weaponState = WeaponState.None;
	
	
	// Possible Weapon sets ----------------------- //
	public Weapons weapons;
	[System.Serializable]
	public class Weapons
	{
		public GameObject sword_Hand = null;
		public GameObject sword_Holster = null;
		public GameObject bow_Hand = null;
		public GameObject bow_Holster = null;
		public GameObject bow_Quiver = null;
		public GameObject rifle_Hand = null;
		public GameObject rifle_Holster = null;
		public GameObject pistol_Hand = null;
		public GameObject pistol_Holster = null;
	}
	
	
	// Animator state hash
	//static int JUMP_Tree = Animator.StringToHash("AirTree.JumpTree");
	
	bool grounded;
	public bool Grounded { get { return grounded; } }
	
	protected Animator a;
	Transform hero;
	Rigidbody rb;
	CapsuleCollider col;
	RaycastHit groundHit;
	bool canNextWeapon = true;
	bool canDrawHolster = true;
	bool climbUp = false; // Up or Down?
	bool climbLong = false; // Short or Long?
	bool isClimb = false;
	
	
	// Cached Input or AI  ----------------------- //
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
	
	AnimatorStateInfo st;
	
	// Anim Controllers   ----------------------- //
	public AnimControllers animCtrl;
	[System.Serializable]
	public class AnimControllers
	{
		public RuntimeAnimatorController baseC;
		public RuntimeAnimatorController climb;
		public RuntimeAnimatorController swim;
		public RuntimeAnimatorController fly;
	}

	
	// Climb ----------------------- //
	public bool canClimb = true;
	public LayerMask climbLayers = 1 << 9; // Layer 9
	public float climbSpeed = 0.7f;
	public float climbCheckRadius = 0.4f;
	public float climbCheckDistance = 3.0f;
	public float climbOffsetToWall = 0.2f;
	public float heightOffsetToEdge = 2.12f;
	public float smoothDistanceSpeed = 2.0f;
	public float cornerSideOffset = 0.4f;
	
	public enum ClimbState
	{
		Climb,
		Top,
		Corner,
		Wall,
		Area,
		Edge,
		None
	}
	public ClimbState climbState = ClimbState.None;
	
	Transform curT = null;
	Vector3 nextPoint;
	Quaternion rot;
	float cacheDist = 0.0f;
	bool reset = false;
	bool isTop = false;
	bool isOverhang = false;
	bool jumpOffNext = false;
	float cornerReach = 0.4f;
	
	
	// Swim ----------------------- //
	public bool canSwim = true;
	public float swimSpeed = 2.5f;
	public float diveRotSpeed = 1.0f;
	public float waterDrag = 1.4f;
	public float offsetToSurf = 1.6f;
	public Vector3 liftVector;

	GameObject cam;
	float distY = 0.0f;
	float curAngle = 0.0f;
	Transform[] bones;
	
	
	// Fly ----------------------- //
	public bool canFly = true;
	public float flySpeed = 7.5f;
	public float flyRotSpeed = 1.0f;
	public float flyDrag = 1.4f;
	float groundTime = 0;
	
	// PhysX --------------------- //
	public bool canRagdoll = true;
	public float startRagTime = 0.4f;
	public float endRagForce = 0.1f;
	public bool startRagdoll = false;
	public bool endRagdoll = false;
	
	// IK Feet Placement --------------------- //
	public bool useIdleFeetPlacement = true;
	
	 // Assign your character's root bone here ----------------- //
	public Transform rootBone = null;
	private string[] weaponNames;

	void Awake() {
		isControllable = false;
		weaponNames = new string[]{"Sword","Bow","Rifle","Pistol"};
		renderersSet = new Renderer[,]{
			{weapons.sword_Holster.renderer,weapons.sword_Hand.renderer},
			{weapons.bow_Holster.renderer,weapons.bow_Hand.renderer},
			{weapons.rifle_Holster.renderer,weapons.rifle_Hand.renderer},
			{weapons.pistol_Holster.renderer,weapons.pistol_Hand.renderer}
		};
		weaponRendererSettings = new bool[,]{
			{false,false,false,false,false,false,false,false,false}, //None
			{false,true,false,false,false,false,false,false,false}, //Sword
			{false,false,false,true,true,false,false,false,false},
			{false,false,false,false,false,false,true,false,false},
			{false,false,false,false,false,false,false,false,true}
		};

		weaponState = WeaponState.Bow; //may be random later
		setWeaponRenderersRPC((int)weaponState);
	}

	//=================================================================================================================o
	void Start () 
	{
		a = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		hero = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		
		a.speed = animSpeed;
		
		if(rootBone == null)
			Debug.Log ("Root Bone not assigned! -  Please assign your character's root bone to the Root Bone field");
		
		if (a.layerCount > 1)
		{
			a.SetLayerWeight(1, 1.0f); // Leg layer, IK feet placement
		}
//		col.center = new Vector3(0, 1, 0);
//		col.height = 2.0f;
		
		rb.mass = 3.0f;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		
		Physics.IgnoreLayerCollision(8,9); // ignore player / climb collision
		// Climb
		cacheDist = climbCheckDistance;
		// Swim
		cam = GameObject.FindGameObjectWithTag("MainCamera"); // Your characters's camera tag
		bones = GetComponentsInChildren<Transform> () as Transform[];
		foreach (Transform t in bones)
		{
			if(t.rigidbody && t.rigidbody != rb)
				t.rigidbody.isKinematic = true;
			if(t.collider && t.collider != hero.collider)
				t.collider.isTrigger = true;
		}
		liftVector = new Vector3 (0, 2.0f, 0);
		// Enable correct weapon setup
		//StartCoroutine(NextWeapon());

	}
	//=================================================================================================================o
	
	void OnAnimatorMove ()
	{
		// Set up for a rigidbody - set the RB position equal to the animator deltaPosition and increase by MoveSpeed
		if(baseState == BaseState.Base || baseState == BaseState.Combat)
		{
			rb.position += a.deltaPosition * moveSpeed;
			hero.rotation *= a.deltaRotation; 	
		}
	}
	//=================================================================================================================o
	
	void FixedUpdate () 
	{

		if(isControllable) {
			// Grab Input each frame --- Handy for your custom input setting and AI
			doLShift = Input.GetKey(KeyCode.LeftShift);
			mX = doLShift ? 0 : Input.GetAxis("Mouse X"); // Mouse X is 0 if leftShift is held down
			h = Input.GetAxis("Horizontal");
	        v = Input.GetAxis("Vertical");	
			doJumpDown = Input.GetButtonDown("Jump");
			//doJump = Input.GetButton("Jump");
			doAtk1Down = Input.GetMouseButtonDown(0);
			doAtk1 = Input.GetMouseButton(0);
			doAtk2Down = Input.GetMouseButtonDown(1);
			doAtk2 = Input.GetMouseButton(1);
			doFwd = Input.GetKeyDown(KeyCode.W);
			doBack = Input.GetKeyDown(KeyCode.S);
			doLeft = Input.GetKeyDown(KeyCode.A);
			doRight = Input.GetKeyDown(KeyCode.D);
			doNextWeapon = Input.GetKeyDown(KeyCode.Q);
			doCombat = Input.GetKeyDown(KeyCode.C);
			doFly = Input.GetKeyDown(KeyCode.Z);
			doClimb = Input.GetKeyDown(KeyCode.E);
			doWalk = Input.GetKeyDown(KeyCode.X);
			doSprint = Input.GetKeyDown(KeyCode.LeftShift);
			doSneak = Input.GetKeyDown(KeyCode.V);
			doDance1 = Input.GetKeyDown(KeyCode.H);	
			doDance2 = Input.GetKeyDown(KeyCode.J);
			doDance3 = Input.GetKeyDown(KeyCode.K);
			doPullLever = Input.GetKeyDown(KeyCode.L);
			doPushButton = Input.GetKeyDown(KeyCode.P);
			doThrow = Input.GetKeyDown(KeyCode.G);
		}

		if(baseState == BaseState.Catapults) { //do something in this state and disable the remaining part.
			mX = 0;
			h = 0;
			v = 0;
		}

		if(a)
		{
			grounded = Physics.Raycast (hero.position + hero.up * col.center.y,
				hero.up * -1, out groundHit, groundedDistance, groundLayers);

			// Set Animator parameters
			a.SetFloat ("AxisY", v, setFloatDampTime, Time.deltaTime);
			a.SetFloat("MouseX", mX, setFloatDampTime * 4, Time.deltaTime);
			a.SetFloat("AxisX", h, setFloatDampTime, Time.deltaTime);
			a.SetBool("Grounded", grounded);

			/*
			if(!photonView.isMine) {
				Debug.Log (weaponState);
				Debug.Log (baseState);
			}
			*/
			
			switch(baseState)
			{
			case BaseState.Base:
				_Base();
				break;
			case BaseState.Climb:
				_Climb();
				break;
			case BaseState.Swim:
				_Swim();
				break;
			case BaseState.Fly:
				_Fly();
				break;
			case BaseState.Combat:
				_Combat();
				break;
			case BaseState.PhysX:
				_PhysX();
				break;
			}
		}
	}
	//=================================================================================================================o
	
	void LateUpdate ()
	{
		// Procedural animation 
		if(canSwim && baseState == BaseState.Swim)
		{
			DiveRotation(diveRotSpeed);
		}
		else if(canFly && baseState == BaseState.Fly)
		{
			DiveRotation(flyRotSpeed);
		}
	}

	[RPC]
	public void initVars(int inputWeaponState, bool r1Setting, bool r2Setting) {
		weaponState = (WeaponState)inputWeaponState;
		renderersSet[inputWeaponState - 1,0].enabled = r1Setting;
		renderersSet[inputWeaponState - 1,1].enabled = r2Setting;
	}


	bool[,] weaponRendererSettings;
	
	[RPC]
	public void setWeaponRenderersRPC(int weaponStateInInt) {
		//if(baseState == BaseState.Base) {
		if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = weaponRendererSettings[weaponStateInInt,0];
		if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = weaponRendererSettings[weaponStateInInt,1];
		if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = weaponRendererSettings[weaponStateInInt,2];
		if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = weaponRendererSettings[weaponStateInInt,3];
		if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = weaponRendererSettings[weaponStateInInt,4];
		if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = weaponRendererSettings[weaponStateInInt,5];
		if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = weaponRendererSettings[weaponStateInInt,6];
		if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = weaponRendererSettings[weaponStateInInt,7];
		if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = weaponRendererSettings[weaponStateInInt,8];
		//}
	}
	

	[RPC]
	public void standUpRPC(bool enable, int s) {
		if(enable) {
			a.enabled = true;
			a.cullingMode = AnimatorCullingMode.BasedOnRenderers;
		}
		a.SetBool("StandUp",enable);
		a.SetInteger("RandomM", s); // 0 or 1 are triggers
	}
	
	public Renderer[,] renderersSet;
	
	[RPC]
	public void doCombatRPC(int doCombatStateInInt) {
		BaseState doCombatState = (BaseState)doCombatStateInInt;
		int r1Idx,r2Idx;
		float[] animTime = new float[4];
		bool stateVar;
		if(doCombatState == BaseState.Base) {
			r1Idx = 0;
			r2Idx = 1;
			animTime[0] = 0.3f;
			animTime[1] = 0.6f;
			animTime[2] = 0.9f;
			animTime[3] = 0.6f;
			stateVar = false;
		}
		else{
			r1Idx = 1;
			r2Idx = 0;
			animTime[0] = 0.3f;
			animTime[1] = 0.3f;
			animTime[2] = 0.3f;
			animTime[3] = 0.2f;
			stateVar = true;
		}
		
		// Coroutine draw motion finished -> switch
		if(weaponState == WeaponState.None)
		{
			return;
		}

		int weaponStateIdx = ((int)weaponState) - 1;
		StartCoroutine(DrawHolster(animTime[weaponStateIdx],renderersSet[weaponStateIdx,r1Idx],renderersSet[weaponStateIdx,r2Idx]));
		a.SetBool(weaponNames[weaponStateIdx], stateVar);
		baseState = doCombatState;
	}


	//=================================================================================================================o
	
	//================================================Base=============================================================o
	
	//=================================================================================================================o
	void _Base ()
	{
		// Next Weapon
		if(doNextWeapon && canNextWeapon)
		{
			//StartCoroutine(NextWeapon());
		}
		
		// Combat Stance / Out
		// if is self then create a rpc to tell everyone
		else if(doCombat && canDrawHolster)
		{
			if(photonView.isMine) {
				//Debug.Log ("Call here");
				photonView.RPC("doCombatRPC", PhotonTargets.All, (int)BaseState.Combat);
			}
		}
		
		// Dance
		else if(doDance1)
		{
			a.SetBool("Dance", true);
			a.SetInteger("RandomM", 1);
		}
		else if(doDance2)
		{
			a.SetBool("Dance", true);
			a.SetInteger("RandomM", 2);
		}
		else if(doDance3)
		{
			a.SetBool("Dance", true);
			a.SetInteger("RandomM", 3);
		}
		
		// Pull Push
		else if(doPullLever)
		{
			a.SetBool("Pull", true);
		}
		else if(doPushButton)
		{
			a.SetBool("Push", true);
		}
		
		// Throw
		else if(doThrow)
		{
			a.SetBool("Throw", true);
		}
		
		// Fly
		else if(doFly)
		{
			if(!canFly)
				return;
			rb.useGravity = false;
			a.SetBool("Jump", true);
			rb.velocity = Vector3.up*5; // Up in fast
			
			// Fly State
			if(animCtrl.fly)
			{
				a.runtimeAnimatorController = animCtrl.fly;
				StartCoroutine (JustHolster());
				baseState = BaseState.Fly;
			}
		}
		
		// Climb
		else if(doClimb)
		{
			if(!canClimb)
				return;
			CheckClimb();
			
			if(climbState != ClimbState.None)
			{
				// Climb State
				if(animCtrl.climb)
				{
					a.runtimeAnimatorController = animCtrl.climb;
					StartCoroutine (JustHolster());
					baseState = BaseState.Climb; // Out
				}
			}
		}
		
		
		// Double Tap - Evade takes tapSpeed & coolDown in seconds
		if(canEvade)
		{
			if(!isDoubleTap) StartCoroutine (DoubleTap (doubleTapSpeed, 1));
		}
		
		
		// Start Radoll modus
		if(canRagdoll)
		{
			// When falling for time
			if(st.IsTag("Fall") && a.enabled)
			{
				float nTime = st.normalizedTime;
				
				if(nTime > startRagTime && !a.IsInTransition(0))
				{
					StartRagdoll();
				}
			}
			else if(startRagdoll) // Manual switch
			{
				StartRagdoll();
			}
		}

		//if(isControllable) {
			// Current state info for layer Base
			st = a.GetCurrentAnimatorStateInfo(0);
		//}
		
		if(grounded)
		{
			if(doJumpDown && canJump && !st.IsTag("Jump") && !st.IsTag("Land"))
			{
				a.SetBool("Jump", true);
				//add extra force to main jump
				if(!st.IsTag("LedgeJump"))
					rb.velocity = hero.up * jumpHeight;
				// Start cooldown until we can jump again
				//StartCoroutine (JumpCoolDown(0.5f));
			}
			
			// Don't slide
			if(!rb.isKinematic)
				rb.velocity = new Vector3(0, rb.velocity.y, 0);
			
			// Extra rotation
			if(canRotate)
			{
				if(!doLShift)
					hero.Rotate(0, mX * rotSpeed/2 * Time.deltaTime, 0);
			}
			
			// Punch, Kick if weapon state is not = None
			if(weaponState != WeaponState.None)
			{
				if(doAtk1Down && !st.IsName("PunchCombo.Punch1"))
				{
					a.SetBool("Attack1", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
					a.SetBool("Sneaking", false); // RESET
				}
				else if(doAtk2Down && !st.IsName("KickCombo.Kick1"))
				{
					a.SetBool("Attack2", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
					a.SetBool("Sneaking", false); // RESET
				}
			}
			
			
			// Walk
			if(doWalk)
			{
				if(!st.IsName("WalkTree.TreeW"))
				{
					a.SetBool("Walking", true);
					a.SetBool("Sneaking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
				}
				else
				{
					a.SetBool("Walking", false); // RESET
				}
			}
			
			// Sprint
			else if(doSprint)
			{
				if(!st.IsName("SprintTree.TreeS"))
				{
					a.SetBool("Sprinting", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sneaking", false); // RESET
				}
				else
				{ 
					a.SetBool("Sprinting", false); // RESET
				}
			}
			
			// Sneak
			else if(doSneak)
			{
				if(!st.IsName("SneakTree.TreeSn"))
				{
					a.SetBool("Sneaking", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
				}
				else
				{
					a.SetBool("Sneaking", false); // RESET
				}
			}
			
			WallGround ();
			
			// Balanceing trigger
			if(groundHit.transform && groundHit.transform.gameObject.layer == 9)
			{
				// Layer 9 should be Climb
				a.SetBool("Balancing", true);
			}
			else
				a.SetBool("Balancing", false); // RESET
			
			
			// -----------AirTime--------- //
			if(!a.GetBool("CanLand")) // Very short air time
			{
				groundTime += Time.deltaTime;
				if(groundTime >= 0.4f)
				{
					a.SetBool("CanLand", true);
				}
			}
			else
				groundTime = 0;
			
			// -----------AirTime--------- //
			
		}
		else // In Air
		{
			// -----------AirTime--------- //
			if(groundTime <= 0.3f)
			{
				groundTime += Time.deltaTime;
				if(groundTime >= 0.2f)
				{
					a.SetBool("CanLand", true);
				}
				else
					a.SetBool("CanLand", false);
			}
			// -----------AirTime--------- //
			
			
			if(canRotate)
				hero.Rotate(0, mX * rotSpeed/2 * Time.deltaTime, 0);
			
			WallRun();
			
			// After jumping off from climb state controller
			if(jumpOffNext)
			{
				a.SetBool("Jump", true);
				jumpOffNext = false;
			}
		}
		
		
		
		
		// Resetting--------------------------------------------------
		if(!a.IsInTransition(0))
		{
			if(st.IsTag("Jump") || st.IsTag("LedgeJump"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Jump", false); // RESET LedgeJump
			}
			
			else if(st.IsTag("Dance"))
			{
				a.SetBool("Dance", false);
			}
			
			else if(st.IsTag("Action"))
			{
				a.SetBool("Pull", false);
				a.SetBool("Push", false);
				a.SetBool("Throw", false);
			}
			
			else if(st.IsTag("StandUp"))
			{
				//photonView.RPC ("standUpRPC",PhotonTargets.All,false,3);
				a.SetInteger("RandomM", 3); // 0 or 1 are triggers
				a.SetBool("StandUp", false); // RESET	
			}
			
			if(st.IsName("PunchCombo.Punch1") && st.normalizedTime > 0.7f && !doAtk1)
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack1", false); // RESET
			}
			else if(st.IsName("PunchCombo.Punch2"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack1", false); // RESET
			}
			
			
			
			if(st.IsName("KickCombo.Kick1") && st.normalizedTime > 0.7f && !doAtk2)
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack2", false); // RESET
			}
			else if(st.IsName("KickCombo.Kick2"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack2", false); // RESET
			}
			
			
			if(st.IsTag("Evade"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Evade_F", false); // RESET
				a.SetBool("Evade_B", false); // RESET
				a.SetBool("Evade_L", false); // RESET
				a.SetBool("Evade_R", false); // RESET
			}

			
			if(st.IsTag("WallRun") && grounded) // No instant reset
			{
				a.SetBool("WallRunL", false); // RESET
				a.SetBool("WallRunR", false); // RESET
				a.SetBool("WallRunUp", false); // RESET
			}
		}
	}
	//=================================================================================================================o
	
	// Facing a low object to jump over or a wall
	void WallGround ()
	{
//		if(v < 0.5f)
//		{
//			a.SetBool("WallLow", false); // RESET
//			a.SetBool("WallHigh", false); // RESET
//			return;
//		}
//		
//		// roughly 2/3 of the characters height
//		bool wallHigh = Physics.Raycast (hero.position + hero.up*1.4f,
//				hero.forward, col.radius*1.6f, wallRunLayers);
//		
//		// roughly the half of the characters height
//		bool wallLow = Physics.Raycast (hero.position + hero.up,
//				hero.forward, col.radius*1.5f, wallRunLayers)
//			&& !wallHigh;
//		
//		
//		
//		if(doJumpDown)
//		{
//			a.SetInteger("RandomM", Random.Range(0,3));
//		}
		
		//a.SetBool("WallLow", wallLow);
		//a.SetBool("WallHigh", wallHigh);
	}
	//=================================================================================================================o
	
	// Jump to a wall and run on it (Air)
	void WallRun ()
	{
		if(!canWallRun) return;
		
		if(v < 0.2f) return;
				
		bool leftW = Physics.Raycast (hero.position + hero.up,
				hero.right * -1 + hero.forward/4, col.radius + 0.2f, wallRunLayers);
		bool rightW = Physics.Raycast (hero.position + hero.up,
				hero.right + hero.forward/4, col.radius + 0.2f, wallRunLayers);
		bool frontW = Physics.Raycast (hero.position+ hero.up,
				hero.forward, col.radius + 0.2f, wallRunLayers);
		
		if(!a.IsInTransition(0) && !rb.isKinematic)
		{
			if(leftW)
			{
				
				rb.velocity = hero.forward * Mathf.Abs(v) + hero.up*3;
				if(!st.IsName("WallRun.RunL"))
				{
					a.SetBool("WallRunL", true);
					StartCoroutine( WallRunCoolDown(3f)); // Exclude if not needed
				}
				
				if(doJumpDown)
				{
					a.SetBool("Jump", true);
				}
			}
			
			else if(rightW)
			{
				rb.velocity = hero.forward * Mathf.Abs(v) + hero.up*3;
				if(!st.IsName("WallRun.RunR"))
				{
					a.SetBool("WallRunR", true);
					StartCoroutine( WallRunCoolDown(3f)); // Exclude if not needed
				}
				
				if(doJumpDown)
				{
					a.SetBool("Jump", true);
				}
			}
			
			else if(frontW)
			{
				rb.velocity = hero.forward/2;
				if(!st.IsName("WallRun.RunUp"))
				{
					a.SetBool("WallRunUp", true);
					StartCoroutine( WallRunCoolDown(3f)); // Exclude if not needed
				}
				
				if(doJumpDown)
				{
					a.SetBool("Jump", true);
				}
			}
			
			
			a.SetBool("WallRunL", leftW);
		
			a.SetBool("WallRunR", rightW);
			
			a.SetBool("WallRunUp", frontW);
		}
	}
	//=================================================================================================================o
	
	// Jump cool-down
	IEnumerator JumpCoolDown (float sec)
	{
		canJump = true;
		yield return new WaitForSeconds (sec);
		canJump = false;
		yield return new WaitForSeconds (sec);
		canJump = true;
	}
	//=================================================================================================================o
	
	// Wall run cool-down
	IEnumerator WallRunCoolDown (float sec)
	{
		canWallRun = true;
		yield return new WaitForSeconds (sec/4);
		canWallRun = false;
		a.SetBool("WallRunL", false); // RESET
		a.SetBool("WallRunR", false); // RESET
		a.SetBool("WallRunUp", false); // RESET
		yield return new WaitForSeconds (sec);
		canWallRun = true;
	}
	//=================================================================================================================o
	IEnumerator JustHolster ()
	{
		// Holster now
		if(weaponState == WeaponState.None)
		{
			yield break;
		}
		/*else if(weaponState == WeaponState.Unarmed)
		{
			yield break;
		}*/
		else if(weaponState == WeaponState.Sword)
		{
			yield return StartCoroutine(DrawHolster(0.3f,weapons.sword_Holster.renderer, weapons.sword_Hand.renderer));
			a.SetBool("Sword", false);
			yield break;
		}
		else if(weaponState == WeaponState.Bow)
		{
			yield return StartCoroutine(DrawHolster(0.6f,weapons.bow_Holster.renderer, weapons.bow_Hand.renderer));
			a.SetBool("Bow", false);
			yield break;
		}
		else if(weaponState == WeaponState.Rifle)
		{
			yield return StartCoroutine(DrawHolster(0.9f,weapons.rifle_Holster.renderer, weapons.rifle_Hand.renderer));
			a.SetBool("Rifle", false);
			yield break;
		}
		else if(weaponState == WeaponState.Pistol)
		{
			yield return StartCoroutine(DrawHolster(0.6f,weapons.pistol_Holster.renderer, weapons.pistol_Hand.renderer));
			a.SetBool("Pistol", false);
			yield break;
		}
	}
	//=================================================================================================================o
	IEnumerator DrawHolster (float sec, Renderer on, Renderer off)
	{
		canDrawHolster = false;
		yield return new WaitForSeconds (sec);
		on.enabled = true;
		off.enabled = false;
		yield return new WaitForSeconds (2); // Cool down
		canDrawHolster = true;
	}
	//=================================================================================================================o
	

	// Next Weapon + cool-down
	IEnumerator NextWeapon ()
	{
		if(baseState == BaseState.Base)
		{
			canNextWeapon = false;
			
		    weaponState += 1; // Next
			
			if(weaponState == WeaponState.Sword) // Skip if no weapon is assigned
			{
				if(weapons.sword_Holster == null) // No weapon - next
				{
					 weaponState += 1; 
				}
				else
				{
					if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = false;
					if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = true; // On
					if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = false;
					if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = false;
					if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = false;
					if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = false;
					if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = false;
					if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = false;
					if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = false;
				}
			}
			
			if(weaponState == WeaponState.Bow) // Skip if no weapon is assigned
			{
				if(weapons.bow_Holster == null) // No weapon - next
				{
					 weaponState += 1; 
				}
				else
				{
					if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = false;
					if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = false;
					if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = false;
					if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = true; // On
					if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = true; // On
					if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = false;
					if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = false;
					if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = false;
					if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = false;
				}
			}
			
			if(weaponState == WeaponState.Rifle) // Skip if no weapon is assigned
			{
				if(weapons.rifle_Holster == null) // No weapon - next
				{
					 weaponState += 1; 
				}
				else
				{
					if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = false;
					if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = false;
					if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = false;
					if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = false;
					if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = false;
					if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = false;
					if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = true; // On
					if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = false;
					if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = false;
				}
			}
			
			if(weaponState == WeaponState.Pistol) // Skip if no weapon is assigned
			{
				if(weapons.pistol_Holster == null) // No weapon - next
				{
					 weaponState += 1; 
				}
				else
				{
					if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = false;
					if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = false;
					if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = false;
					if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = false;
					if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = false;
					if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = false;
					if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = false;
					if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = false;
					if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = true; // On
				}
			}
			
			// No weapon
			if(weaponState == WeaponState.None /*|| weaponState == WeaponState.Unarmed*/)
			{
				if(weapons.sword_Hand) weapons.sword_Hand.renderer.enabled = false;
				if(weapons.sword_Holster) weapons.sword_Holster.renderer.enabled = false;
				if(weapons.bow_Hand) weapons.bow_Hand.renderer.enabled = false;
				if(weapons.bow_Holster) weapons.bow_Holster.renderer.enabled = false;
				if(weapons.bow_Quiver) weapons.bow_Quiver.renderer.enabled = false;
				if(weapons.rifle_Hand) weapons.rifle_Hand.renderer.enabled = false;
				if(weapons.rifle_Holster) weapons.rifle_Holster.renderer.enabled = false;
				if(weapons.pistol_Hand) weapons.pistol_Hand.renderer.enabled = false;
				if(weapons.pistol_Holster) weapons.pistol_Holster.renderer.enabled = false;
			}
			
			// Last in the enum
			if (weaponState > (WeaponState)System.Enum.GetValues(typeof(WeaponState)).Length-1) 
				weaponState = WeaponState.None; // Start at the first again
			
			yield return new WaitForSeconds(0.3f); // Cool-down
			canNextWeapon = true;
		}
	}
	//=================================================================================================================o
	IEnumerator DoubleForward (float dTapSpeed, float coolDown)
	{
		is2Fwd = true;
		float t = 0;
		yield return new WaitForSeconds(0.1f);
		while (t < dTapSpeed) 
		{
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.01f); // jitter stabilizing
			if(doFwd)
			{
				a.SetBool("Evade_F", true);
				yield return new WaitForSeconds(coolDown);
				is2Fwd = false;
				yield break;
			}
		}
		is2Fwd = false;
	}
	IEnumerator DoubleBack (float dTapSpeed, float coolDown)
	{
		is2Back = true;
		float t = 0;
		yield return new WaitForSeconds(0.1f);
		while (t < dTapSpeed)
		{
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.01f); // jitter stabilizing
			if(doBack)
			{
				a.SetBool("Evade_B", true);
				yield return new WaitForSeconds(coolDown);
				is2Back = false;
				yield break;
			}
		}
		is2Back = false;
	}
	IEnumerator DoubleLeft (float dTapSpeed, float coolDown)
	{
		is2Left = true;
		float t = 0;
		yield return new WaitForSeconds(0.1f);
		while (t < dTapSpeed)
		{
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.01f); // jitter stabilizing
			if(doLeft)
			{
				a.SetBool("Evade_L", true);
				yield return new WaitForSeconds(coolDown);
				is2Left = false;
				yield break;
			}
		}
		is2Left = false;
	}
	IEnumerator DoubleRight (float dTapSpeed, float coolDown)
	{
		is2Right = true;
		float t = 0;
		yield return new WaitForSeconds(0.1f);
		while (t < dTapSpeed)
		{
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.01f); // jitter stabilizing
			if(doRight)
			{
				a.SetBool("Evade_R", true);
				yield return new WaitForSeconds(coolDown);
				is2Right = false;
				yield break;
			}
		}
		is2Right = false;
	}
	//=================================================================================================================o
	IEnumerator DoubleTap (float tapSpeed, float coolDown)
	{
		isDoubleTap = true;
		if(doFwd && !is2Fwd) 
			yield return StartCoroutine (DoubleForward (tapSpeed, coolDown));
		else if(doBack && !is2Back) 
			yield return StartCoroutine (DoubleBack (tapSpeed, coolDown));
		else if(doLeft && !is2Left) 
			yield return StartCoroutine (DoubleLeft (tapSpeed, coolDown));
		else if(doRight && !is2Right) 
			yield return StartCoroutine (DoubleRight (tapSpeed, coolDown));
		isDoubleTap = false;
	}
	
	//=================================================================================================================o
	
	//==================================================Climb==========================================================o
	
	//=================================================================================================================o
	void _Climb ()
	{
		//if(isControllable) {
			// Current state info for layer Base
			st = a.GetCurrentAnimatorStateInfo(0);
		//}
		// Switch climb states
		switch (climbState)
		{
		case ClimbState.None:
			if (!reset)
			{
				ExitClimb ();
			}
			CheckClimb();
			break;
			
		case ClimbState.Climb:
			ClimbTo();
			reset = false;
			break;
			
		case ClimbState.Top:
			isTop = false;
			PullUp();
			reset = false;
			break;
			
		case ClimbState.Wall:
			WallClimb();
			reset = false;
			break;
			
		case ClimbState.Area:
			ClimbTo();
			reset = false;
			break;
			
		case ClimbState.Edge:
			EdgeClimb();
			reset = false;
			break;
	
		case ClimbState.Corner:
			CornerLerp(climbSpeed);
			reset = false;
			break;
		}
			
			
		if(doJumpDown)
		{
			if(v != 0)
				jumpOffNext = true;
			else
				jumpOffNext = false;
			
			climbState = ClimbState.None;
		}
		
		if(!a.IsInTransition(0))
		{
			// Resetting--------------------------------------------------
			if(st.IsTag("Jump"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Jump", false); // RESET
			}
			if(st.IsTag("Oh"))
			{
				a.SetBool("Overhang", false); // RESET
				isOverhang = false;
			}
		}
	}
	//=================================================================================================================o
	
	// Look for objects to climb
	void CheckClimb ()
	{
		if(!canClimb)
			return;
		if(doClimb && v > 0.2) // Up
		{
			// Reduce check distance/height while falling 
			if (!grounded && baseState == BaseState.Base)
			{
				climbCheckDistance = 0.4f;
			}
			else 
				climbCheckDistance = cacheDist;
			
			Vector3 p1 = hero.position - (hero.up * -heightOffsetToEdge) + hero.forward;
			Vector3 p2 = hero.position - (hero.up * -heightOffsetToEdge) - hero.forward;
			RaycastHit hit;
			
			// Hit nothing and not at edge -> Out
			if(!Physics.CapsuleCast (p1, p2, climbCheckRadius, hero.up, out hit, climbCheckDistance, climbLayers))
				return;
			// If not almost facing the edge cancel/Out
			if (Vector3.Angle (hero.right, hit.transform.right) >= 60.0f)
				return;
			
			if(isTop)
				return;
			
			if(curT != hit.transform)
			{
				curT = hit.transform;
				nextPoint = hit.point;
			}
			
			
			hero.rotation = Quaternion.Euler(curT.eulerAngles.x, curT.eulerAngles.y, 0);
			
			a.SetInteger("RandomM", Random.Range(0,3));
			
			if(curT.name == "climbObject")
			{
				rb.isKinematic = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectOh")
			{
				rb.isKinematic = true;
				isOverhang = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectTop")
			{
				rb.isKinematic = true;
				isTop = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectTopOh")
			{
				rb.isKinematic = true;
				isTop = true;
				isOverhang = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbWall")
			{
				rb.isKinematic = true;
				isTop = true;
				if(grounded)
					climbState = ClimbState.Wall;
				else
					climbState = ClimbState.Area;
			}
			else if(curT.name == "climbArea")
			{
				rb.isKinematic = true;
				climbState = ClimbState.Area;
			}
			else
			{
				climbState = ClimbState.None;
			}
			
			isClimb = true;
			// UP
			climbUp = true;
			// Long or Short ?
			climbLong = hit.distance > 1 ? true : false;
		}
		else if(doClimb && v < -0.2) // Down
		{
			// Reduce check distance/height while falling 
			if (!grounded && baseState == BaseState.Base)
			{
				climbCheckDistance = 0.4f;
			}
			else 
				climbCheckDistance = cacheDist;
			
			Vector3 p1 = hero.position - (hero.up * -heightOffsetToEdge) + hero.forward;
			Vector3 p2 = hero.position - (hero.up * -heightOffsetToEdge) - hero.forward;
			RaycastHit hit;
			
			// Hit nothing and not at edge -> Out
			if(!Physics.CapsuleCast (p1, p2, climbCheckRadius, -hero.up, out hit, climbCheckDistance, climbLayers))
				return;
			// If not almost facing the edge cancel/Out
			if (Vector3.Angle (hero.right, hit.transform.right) >= 60.0f)
				return;
			
			if(curT != hit.transform)
			{
				curT = hit.transform;
				nextPoint = hit.point;
			}
			
			hero.rotation = Quaternion.Euler(0,curT.eulerAngles.y,0);
			isTop = false; // RESET
			a.SetBool("Top", false); // RESET
			a.SetInteger("RandomM", Random.Range(0,3));
			
			if(curT.name == "climbObject")
			{
				rb.isKinematic = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectOh")
			{
				rb.isKinematic = true;
				isOverhang = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectTop")
			{
				rb.isKinematic = true;
				isTop = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbObjectTopOh")
			{
				rb.isKinematic = true;
				isTop = true;
				isOverhang = true;
				climbState = ClimbState.Climb;
			}
			else if(curT.name == "climbWall")
			{
				climbState = ClimbState.None;
			}
			else if(curT.name == "climbArea")
			{
				rb.isKinematic = true;
				climbState = ClimbState.Area;
			}
			else
			{
				climbState = ClimbState.None;
			}
			
			isClimb = true;
			// Down
			climbUp = false;
			// Long or Short ?
			climbLong = hit.distance > 1 ? true : false;
			//print ("Down");
		}
	}
	//=================================================================================================================o
	
	// Reached the top, PullUp
	void PullUp ()
	{
		if(st.IsTag("Top") && !a.IsInTransition(0))
		{
			float nT = st.normalizedTime;
			if (nT <= 1.0f)
			{
				if(nT <= 0.4f) // Step up
				{
					hero.Translate(Vector3.up * Time.deltaTime * climbSpeed *8.0f);
				}
				else // Step forward
				{
					if(nT <= 0.6f)
						hero.Translate(Vector3.forward * Time.deltaTime * climbSpeed *2.5f);
					
					else if(nT >= 0.6f && rb.isKinematic) // fall early
						rb.isKinematic = false;
					if(!rb.isKinematic)
						rb.velocity = new Vector3(0, rb.velocity.y, 0);
				}
			}
			else // Animation is finished 
				climbState = ClimbState.None; // Out
		}
	}
	//=================================================================================================================o
	
	void EdgeClimb ()
	{
		Vector3 relPoint = curT.InverseTransformPoint (hero.position);
		MatchP(Mathf.Abs(relPoint.z), climbOffsetToWall, Vector3.forward, Vector3.back);
		MatchP(Mathf.Abs(relPoint.y), heightOffsetToEdge, Vector3.up, Vector3.down);
		
		if(isOverhang)
		{
			a.SetBool("Overhang", true);
		}
		
		if(!st.IsTag("Oh") && !a.IsInTransition(0)) // not if overhanging
		{
			// Horizontal climbing
			if(h != 0.0f) 
			{
				Vector3 origin = hero.position - (hero.forward * -climbOffsetToWall) - (hero.up * -heightOffsetToEdge);
				if(h > 0.0f) // Check right
				{
					RaycastHit hit;
					if(Physics.SphereCast (origin, 0.2f, hero.right, out hit, 0.4f, climbLayers))
					{
						nextPoint = hit.point;
						curT = hit.transform;
						a.SetBool("CornerR", true);
						cornerReach = 0.4f;
						climbState = ClimbState.Corner; // Out
					}	
					// Edge speed interval, min - max speed.
					float edgeSpeed = st.normalizedTime % 1 < 0.5f ? 0.9f : 0.3f;
					// Apply
					hero.Translate( 1 * Time.deltaTime * edgeSpeed, 0, 0, curT );
				}
				else // Check left
				{
					RaycastHit hit;
					if(Physics.SphereCast (origin, 0.2f, -hero.right, out hit, 0.4f, climbLayers))
					{
						nextPoint = hit.point;
						curT = hit.transform;
						a.SetBool("CornerL", true);
						cornerReach = -0.4f;
						climbState = ClimbState.Corner; // Out
					}
					// Edge speed interval, min - max speed.
					float edgeSpeed = st.normalizedTime % 1 < 0.5f ? 0.3f : 0.9f;
					// Apply
					hero.Translate( -1 * Time.deltaTime * edgeSpeed, 0, 0, curT );
				}
			} 
			
			// Top
			if(isTop && doClimb)
			{
				a.SetBool("Top", true);
				climbState = ClimbState.Top; // Out
			}
		}
		
		// Right End
		if (relPoint.x >= 0.44f) 
		{
			a.SetBool("HangR", true);
			
			if (relPoint.x >= 0.5f)
				climbState = ClimbState.None; // Out
		}
		// Left End
		else if (relPoint.x <= -0.44f) 
		{
			a.SetBool("HangL", true);
			
			if (relPoint.x <= -0.5f)
				climbState = ClimbState.None; // Out
		}
		else
		{
			a.SetBool("HangL", false); // RESET
			a.SetBool("HangR", false); // RESET
			
			// While not hanging, preparing or climbing
			if((!st.IsTag("Pre") || !st.IsTag("Climb"))  && !a.IsInTransition(0))
				CheckClimb(); // Find next
		}
	}
	//=================================================================================================================o
	
	void WallClimb ()
	{
		if(grounded && v < 0) // On ground and going down
		{
			climbState = ClimbState.None;
		}
		
		Vector3 relV = curT.InverseTransformPoint(hero.position);
		Vector3 inputVec = hero.position;
		float distY = Mathf.Abs(relV.y);
		float distZ = Mathf.Abs(relV.z);
		
		a.SetBool("WallClimb", true);
		
		// Limit is almost the edge
		if (distY >= heightOffsetToEdge * 1.1f)
		{
			MatchP (distZ, climbOffsetToWall, Vector3.forward, Vector3.back);
			
			// Input
			if (v != 0.0f)
			{
				if (v > 0.0f)
				{
					inputVec = hero.TransformPoint( Vector3.up );
				} 
				else 
				{
					inputVec = hero.TransformPoint( Vector3.down );
				
					if (!Physics.Raycast(hero.position, hero.forward, climbOffsetToWall, climbLayers) || grounded)
					{
						climbState = ClimbState.None;
					}
				}
				
				// Apply movement
				hero.position = Vector3.Lerp( hero.position, inputVec, Time.deltaTime * climbSpeed * 1.3f);
			}
			else // Stop if there's no input
			{
				inputVec = hero.TransformPoint( Vector3.zero );
			}
		}
		else // Reached the edge
		{
			a.SetBool("WallClimb", false); // RESET
			climbState = ClimbState.Edge; // Out
		}
		
		
	}
	//=================================================================================================================o
	
	void ClimbTo ()
	{
		// When entering Climb state
		if(isClimb)
		{
			if(climbUp) 
			{
				if(climbLong)
					a.SetBool("Climb", true); 
				else
					a.SetBool("ClimbShort", true);
				isClimb = false; 
			} 
			else 
			{
				if(climbLong)
					a.SetBool("ClimbDown", true); 
				else
					a.SetBool("ClimbDownShort", true);
				isClimb = false;
			}
		}
		
		if(grounded && v < 0) // On ground and going down
		{
			climbState = ClimbState.None;
		}
		
		if(!st.IsTag("Pre"))
		{
			Vector3 edgePos = nextPoint + ((hero.up * -heightOffsetToEdge) - (hero.forward * climbOffsetToWall));
			// Apply lift up
			hero.position = Vector3.Lerp (hero.position, edgePos, Time.deltaTime * climbSpeed * 11);
			
			if(Vector3.Distance (hero.position, edgePos) <=  0.1f)
			{
				a.SetBool("Climb", false); // RESET
				a.SetBool("ClimbShort", false); // RESET
				a.SetBool("ClimbDown", false); // RESET
				a.SetBool("ClimbDownShort", false); // RESET
				
				if(isOverhang) 
					a.SetBool("Overhang", true);
				
				if(curT.name == "climbArea" || curT.name == "climbWall")
				{
					climbState = ClimbState.Wall; // Out
				}
				else
					climbState = ClimbState.Edge; // Out
			}
		}
	}
	//=================================================================================================================o
	
	// At corner change climb handle
	void CornerLerp (float speed)
	{
		Vector3 tVec = nextPoint - 
			(hero.right * -cornerReach) - (hero.forward * climbOffsetToWall) - (hero.up * heightOffsetToEdge);
		
		float angleTo = Vector3.Angle (hero.right, curT.transform.right);
		
		// Faster if angle is rel small
		speed = angleTo <= 15.0f ?  speed *= 15 : speed *= 4;
		
		// Avoid hanging
		a.SetBool("HangL", false); // RESET
		a.SetBool("HangR", false); // RESET
		
		// Apply
		hero.position = Vector3.Lerp (hero.position, tVec, Time.deltaTime * speed);
		hero.rotation = Quaternion.Lerp (hero.rotation, curT.rotation, Time.deltaTime * speed);
		
		// Shift inward a bit 
		if (angleTo <= 0.01f)
		{
			Vector3 relPoint = curT.InverseTransformPoint (hero.position);
			
			MatchP (Mathf.Abs(relPoint.z), climbOffsetToWall, Vector3.forward, Vector3.back);
			
			a.SetBool("CornerL", false); // RESET
			a.SetBool("CornerR", false); // RESET
			climbState = ClimbState.Edge; // Out
		
		}
	}
	//=================================================================================================================o
	
	void MatchP (float dist, float desiredDist, Vector3 positive, Vector3 negative)
	{		
		if (dist.ToString("f2") != desiredDist.ToString("f2")) 
		{
			float s = climbSpeed * Time.deltaTime;
			
			if (dist > desiredDist) // forward
			{
				if (dist > (desiredDist + 0.1f)) 
					s *= smoothDistanceSpeed * 2; // Far, move faster
				else
					s /= smoothDistanceSpeed /1.5f; // Near, move slower
				hero.Translate( positive * s );
			}
			else if (dist < desiredDist) // backward
			{
				if (dist < (desiredDist + 0.1f))
					s *= smoothDistanceSpeed * 2; // Far, move faster
				else
					s /= smoothDistanceSpeed /1.5f; // Near, move slower
				hero.Translate( negative * s );
			}
		}
	}
	//=================================================================================================================o
	
	void ExitClimb ()
	{
		// Base State
		if(animCtrl.climb)
		{
			rb.isKinematic = false;
			
			isTop = false; // RESET
			isOverhang = false; // RESET
			hero.rotation = Quaternion.Euler(0, curT.eulerAngles.y, 0);
			curT = null;
			climbState = ClimbState.None;
			a.runtimeAnimatorController = animCtrl.baseC;
			baseState = BaseState.Base;
			reset = true; // Out
		}
	}
	
	//=================================================================================================================o
	
	//================================================Swim=============================================================o
	
	//=================================================================================================================o
	
	// Movement & Rotation
	void _Swim ()
	{
		float dTS = distY - hero.position.y;
		Floating( dTS, liftVector);
		
		a.SetFloat("WaterLevel", dTS);
		
		rb.drag = waterDrag;
		
		// Rotation, if mouse movement
		if(mX != 0.0f)
		{
			hero.Rotate (0, mX * Time.fixedDeltaTime * diveRotSpeed * 55, 0, Space.Self);
		}
		
		// If input
		if(h != 0.0f || v != 0.0f) 
		{
			float speed = swimSpeed; 

			// Movement Vector
			if(dTS >= offsetToSurf) // Under surface
			{
				Vector3 swimInput = hero.right * h + cam.transform.forward * v;	
			
				rb.velocity = swimInput.normalized * speed;
			}
			else if(grounded && dTS < offsetToSurf) // Walking in/out of the water
			{
				Vector3 swimInput = hero.right * h + hero.forward * v;	
				
				rb.velocity = swimInput.normalized * speed;
			}
			else // Above the surface
			{
				Vector3 swimInput = hero.right * h + hero.forward * v + liftVector;
				Floating(dTS, swimInput);
			}
		}
	}
	//=================================================================================================================o
	void DiveRotation (float speed) // Procedural animation for LateUpdate
	{
		float targetAngle = Mathf.Abs (Vector3.Angle (cam.transform.forward, hero.up));
		// stay upwards till targetAngle is in scope
		if(targetAngle < 150.0f)
		{
			targetAngle -= 90;
		}
		else if(targetAngle >= 150.0f)
		{
			targetAngle = 140.0f;
		}
		if(grounded)
		{
			targetAngle = 0.0f;
		}
		
		curAngle = Mathf.Lerp (curAngle, targetAngle, Time.deltaTime * speed);
		// Update our current rotation
		if(curAngle > 5.0f) 
		{
			// Apply rotation, in this case the local Y axis of the rootBone
			rootBone.Rotate (0, curAngle, 0, Space.Self);
		}
	}
	//=================================================================================================================o
	
	// Water behaviour
	void Floating (float distToSurf, Vector3 liftVec)
	{
		if(distToSurf > offsetToSurf) // Under water
		{
			rb.velocity = Vector3.Lerp(rb.velocity, liftVec, Time.deltaTime *2);
		}
		else
		{
			rb.velocity = Vector3.Lerp(rb.velocity, -liftVec, Time.deltaTime *2);
		}
	}
	//=================================================================================================================o
	
	// Trigger of the water system
	void OnTriggerEnter (Collider c)
	{
		if(c.name == "WaterTrigger")
		{
			if(!canSwim || baseState == BaseState.PhysX)
				return;
			
			distY = c.bounds.max.y;
			
			rb.useGravity = false;
			rb.velocity = Vector3.down*5; // Dive in fast
			// Swim State
			if(animCtrl.swim)
			{
				a.runtimeAnimatorController = animCtrl.swim;
				StartCoroutine (JustHolster());
				baseState = BaseState.Swim; // Out
			}
		}
	}
	void OnTriggerExit (Collider c)
	{
		if(c.name == "WaterTrigger")
		{
			if(!canSwim || baseState == BaseState.PhysX)
				return;
			distY = 0.0f;
			
			rb.useGravity = true;
			
			// Base State
			if(animCtrl.baseC)
			{
				rb.drag = baseDrag;
				a.runtimeAnimatorController = animCtrl.baseC;
				baseState = BaseState.Base; // Out
			}
		}
	}
	
	//=================================================================================================================o
	
	//================================================Fly==============================================================o
	
	//=================================================================================================================o
	void _Fly ()
	{	
		rb.drag = flyDrag;
		
		// Rotation, if mouse movement
		if(mX != 0.0f)
		{
			hero.Rotate (0, mX * Time.fixedDeltaTime * diveRotSpeed * 55, 0, Space.Self);
		}
		// Leave Flying
		if(doFly)
		{
			rb.useGravity = true;
			
			// Base State
			if(animCtrl.baseC)
			{
				rb.drag = baseDrag;
				a.runtimeAnimatorController = animCtrl.baseC;
				baseState = BaseState.Base; // Out
			}
		}
		
		// If input
		if(h != 0.0f || v != 0.0f) 
		{
			// Movement Vector
			Vector3 flyVec = hero.right * h 
				+ cam.transform.forward * v ;
			
			
			// Lift if grounded
			if(grounded)
			{
				groundTime += Time.deltaTime;
				
				if(groundTime > 0.6f)
				{
					rb.velocity = Vector3.Lerp(Vector3.zero, Vector3.up * 22, groundTime);
					groundTime = 0;
				}
			}
			else
			{
				rb.velocity = flyVec.normalized * flySpeed;
			}
		}
	}
	
	//=================================================================================================================o
	
	//===============================================Combat============================================================o
	
	//=================================================================================================================o
	
	void _Combat ()
	{
		// Combat Stance / Out
		if(doCombat && canDrawHolster)
		{
			//if is self then create a rpc to tell everyone
			if(photonView.isMine) {
				//Debug.Log("Call");
				photonView.RPC("doCombatRPC", PhotonTargets.All, (int)BaseState.Base);
			}
		}
		
		// Double Tap - Evade takes tapSpeed & coolDown in seconds
		if(canEvade)
		{
			if(!isDoubleTap) StartCoroutine (DoubleTap (doubleTapSpeed, 1));
		}
		//if(isControllable) {
			// Current state info for layer Base
			st = a.GetCurrentAnimatorStateInfo(0);
		//}
		if(grounded)
		{
			if(doJumpDown /*&& canJump */&& !st.IsTag("Jump") && !st.IsTag("Land"))
			{
				a.SetBool("Jump", true);
				//add extra force to main jump
				rb.velocity = hero.up * jumpHeight;
				// Start cooldown until we can jump again
				//StartCoroutine (JumpCoolDown(0.5f));
			}
			
			// Don't slide
			if(!rb.isKinematic)
				rb.velocity = new Vector3(0,rb.velocity.y,0);
			
			// Extra rotation
			if(canRotate)
			{
				if(!doLShift)
					hero.Rotate(0,mX * rotSpeed/2 * Time.deltaTime,0);
			}
			
			
			// Punch, Kick
			if(doAtk1Down && !st.IsTag("Attack1"))
			{
				a.SetBool("Attack1", true);
				a.SetBool("Walking", false); // RESET
				a.SetBool("Sprinting", false); // RESET
				a.SetBool("Sneaking", false); // RESET
			}
			else if(doAtk2Down && !st.IsTag("Attack2"))
			{
				
				a.SetBool("Attack2", true);
				a.SetBool("Walking", false); // RESET
				a.SetBool("Sprinting", false); // RESET
				a.SetBool("Sneaking", false); // RESET
			}
			
			
			// Walk
			if(doWalk)
			{
				if(!st.IsTag("WalkTree"))//IsName("WalkTree.TreeW")) 
				{
					a.SetBool("Walking", true);
					a.SetBool("Sneaking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
				}
				else
				{
					a.SetBool("Walking", false); // RESET
				}
			}
			
			// Sprint
			else if(doSprint)
			{
				if(!st.IsTag("SprintTree"))
				{
					a.SetBool("Sprinting", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sneaking", false); // RESET
				}
				else
				{ 
					a.SetBool("Sprinting", false); // RESET
				}
			}
			
			// Sneak
			else if(doSneak)
			{
				if(!st.IsTag("SneakTree"))
				{
					a.SetBool("Sneaking", true);
					a.SetBool("Walking", false); // RESET
					a.SetBool("Sprinting", false); // RESET
				}
				else
				{
					a.SetBool("Sneaking", false); // RESET
				}
			}
			
			WallGround ();
			
				// -----------AirTime--------- //
			if(!a.GetBool("CanLand")) // Very short air time
			{
				groundTime += Time.deltaTime;
				if(groundTime >= 0.4f)
				{
					a.SetBool("CanLand", true);
				}
			}
			else
				groundTime = 0;
			
			// -----------AirTime--------- //
			
		}
		else // In Air
		{
			// -----------AirTime--------- //
			if(groundTime <= 0.3f)
			{
				groundTime += Time.deltaTime;
				if(groundTime >= 0.2f)
				{
					a.SetBool("CanLand", true);
				}
				else
					a.SetBool("CanLand", false);
			}
			// -----------AirTime--------- //
			
			if(canRotate)
				hero.Rotate(0,mX * rotSpeed/2 * Time.deltaTime,0);
			
			WallRun();
		}
		
		
		
		// Resetting--------------------------------------------------
		if(!a.IsInTransition(0))
		{
			if(st.IsTag("Jump") || st.IsTag("LedgeJump"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Jump", false); // RESET
			}
			
			if(st.IsTag("Shoot") && !doAtk1)
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack1", false); // RESET
			}
			
			if((st.IsName("SwordCombo_L.slash1") 
				|| st.IsName("SwordCombo_L.slash2")) && st.normalizedTime > 0.4f && !doAtk1)
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack1", false); // RESET
			}
			else if(st.IsName("SwordCombo_L.slash2.5"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack1", false); // RESET
			}
			
			
			if((st.IsName("SwordCombo_R.slash3") 
				|| st.IsName("SwordCombo_R.slash4")) && st.normalizedTime > 0.4f && !doAtk2)
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack2", false); // RESET
			}
			else if(st.IsName("SwordCombo_R.slash5"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Attack2", false); // RESET
			}
			
			
			
			if(st.IsTag("Evade"))
			{
				// Reset our parameter to avoid looping
            	a.SetBool("Evade_F", false); // RESET
				a.SetBool("Evade_B", false); // RESET
				a.SetBool("Evade_L", false); // RESET
				a.SetBool("Evade_R", false); // RESET
			}
			
			
			
			
			if(st.IsTag("WallRun") && grounded) // No instant reset
			{
				a.SetBool("WallRunL", false); // RESET
				a.SetBool("WallRunR", false); // RESET
				a.SetBool("WallRunUp", false); // RESET
			}
		}
	}
	
	//=================================================================================================================o
	
	//================================================PhysX============================================================o
	
	//=================================================================================================================o
	void _PhysX ()
	{
		// End - back to Base state
		if(endRagdoll)
		{
			EndRagdoll(1); // Manual front
		}
		
		// Stay at Ragdoll position
		Vector3 pos = rootBone.position;
		hero.position = Vector3.Lerp (hero.position, pos, Time.deltaTime * 100.0f);
		rootBone.position = pos;
	
		float velM = rootBone.rigidbody.velocity.magnitude;
		
		// Stop condition
		if(velM <= endRagForce && grounded)
		{
			Vector3 targetDir = rootBone.forward - hero.up;
			float targetAngle = Mathf.Abs(Vector3.Angle (targetDir, rootBone.forward));
			
			velM = 0;
			// Pick animation based on angle
			if (targetAngle >= 40.0f) // Back
			{
				a.SetInteger("RandomM", 0);
				MainRotationRagDoll (2);
				EndRagdoll(0); // Out
			}
			else if (targetAngle <= 40.0f) // Front
			{
				rootBone.rigidbody.velocity = Vector3.zero;
				a.SetInteger("RandomM", 1);
				MainRotationRagDoll (-2);
				EndRagdoll(1); // Out
			}
		}
		else if(doClimb) // End ragdoll manually
		{
			MainRotationRagDoll (-2);
			EndRagdoll(1); // Out
		}
	}
	//=================================================================================================================o
	
	// Start ragdoll modus
	void StartRagdoll ()
	{
		startRagdoll = true;
		a.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		a.enabled = false;
		// Velocity with H and V axis
		Vector3 vec = rb.velocity + hero.forward *v*2 + hero.right *h*2;
		vec.y -= 3; // Extra down force
		
		rb.isKinematic = true;
		Physics.IgnoreLayerCollision(8,8); // No self collision
		
		int i;
		for (i=0; i<bones.Length; i++)
		{
			if (bones[i].rigidbody && bones[i] != hero.root)
			{
				bones[i].rigidbody.isKinematic = false;
				bones[i].collider.isTrigger = false;
				bones[i].rigidbody.drag = 0.0f;
				bones[i].rigidbody.velocity = vec;
			}
		}
		startRagdoll = false;
		StartCoroutine (JustHolster());
		baseState = BaseState.PhysX;
	}
	//=================================================================================================================o
	
	// End ragdoll modus
	void EndRagdoll (int s)
	{
		rb.isKinematic = false;
		int i;
		for (i=0; i<bones.Length; i++)
		{
			if (bones[i] != hero.root && bones[i].rigidbody)
			{
				bones[i].rigidbody.isKinematic = true;
				bones[i].collider.isTrigger = true;
			}
		}
		a.enabled = true;
		a.cullingMode = AnimatorCullingMode.BasedOnRenderers;
		a.SetBool("StandUp", true);
		a.SetInteger("RandomM", s);
		//photonView.RPC ("standUpRPC",PhotonTargets.All,true,s);

		endRagdoll = false;
		baseState = BaseState.Base;
	}
	//=================================================================================================================o
	
	// Recover rotation before STandUp()
	void MainRotationRagDoll (float f)
	{
		Vector3 v = rootBone.position + (rootBone.right * f); //root local up axis
		v.y = hero.position.y;
		hero.LookAt(v);
	}
	
	//=================================================================================================================o
	
	//============================================Feet Placement=======================================================o
	
	//=================================================================================================================o
	void FeetPlacementIK () // Unity Pro only!
	{
		if(!useIdleFeetPlacement)
		{
			if(a.GetLayerWeight(1) != 0)
				a.SetLayerWeight(1, 0); // Set Leglayer weight to 0 
			return;
		}
			
		if(!grounded)
			return;
		// Cancel if out of MoveTree State 
		if (!st.IsTag("MoveTree"))
			return;
		
		float LegLayerW = a.GetLayerWeight(1);
		
		// Axis movement
		if(v != 0 || h != 0)
		{
			if(LegLayerW > 0)
				a.SetLayerWeight(1, LegLayerW - (Time.deltaTime + 0.2f));
		}
		else // Idle
		{
			if(LegLayerW < 1)
				a.SetLayerWeight(1, LegLayerW + (Time.deltaTime + 0.1f));
		}
	
		if(LegLayerW > 0.1f)
		{
			RaycastHit rL;
			RaycastHit rR;
			Vector3 leftP = (hero.position + hero.up) - hero.right/4.5f;
			Vector3 rightP = (hero.position + hero.up) + hero.right/4.5f;
			
			if(!Physics.Raycast(leftP, Vector3.down, out rL, 2, groundLayers))
				return;
			if(!Physics.Raycast(rightP, Vector3.down, out rR, 2, groundLayers))
				return;
			
			float dif = Mathf.Abs(rL.distance - rR.distance);
			if(dif < 0.05f) // If groundheight difference is bigger than...
				return;
			
			a.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LegLayerW);
			a.SetIKPositionWeight(AvatarIKGoal.RightFoot, LegLayerW);
			
			Vector3 rheight = Vector3.Lerp(Vector3.zero, new Vector3(0, dif, 0), LegLayerW);
			a.bodyPosition -= rheight;
			
			Vector3 feetHeight = new Vector3(0, 0.09f, 0);
			Vector3 leftFootPos = rL.point + feetHeight;
			Vector3 rightFootPos = rR.point + feetHeight;
			
			a.SetIKPosition(AvatarIKGoal.LeftFoot,leftFootPos);
			a.SetIKPosition(AvatarIKGoal.RightFoot,rightFootPos);
		}
	}
	
	//=================================================================================================================o
	
	//================================================Aiming===========================================================o
	
	//=================================================================================================================o
	void AimLookIK () // Unity Pro only!
	{
		if(st.IsTag("Evade_F"))
			return;
		if(baseState == BaseState.Combat)
		{
			// Camera forward(Z) is aim direction
			Vector3 targetDir = cam.transform.position + cam.transform.forward * 22;
			
			a.SetLookAtPosition (targetDir);
	        a.SetLookAtWeight(1, 0.5f, 0.7f, 0.0f, 0.5f);
		}
	}
	//=================================================================================================================o
	
	void OnAnimatorIK (int layerIndex) // Unity Pro only!
	{	
		if(layerIndex == 1)
		{
			FeetPlacementIK ();
		}
		if(layerIndex == 2)
		{
			AimLookIK ();
		}
	}
}
