using UnityEngine;
using System.Collections;

public class MobileController : MonoBehaviour {

	public CNAbstractController RotateJoystick;
	public float RotationSpeed = 10f;
	[HideInInspector]
	public GameObject Catapult = null;
//	[HideInInspector]
	private float viewAngleUpperBound = 360 - 0.05f;
	private float viewAngleLowerBound = 320;
	private Transform _transformCache;
	private Transform _parentTransformCache;
	private Transform _catapultTransformCache;

	private Animator animator = null;
	private string state_shot = "Shoot_Stone";
	private bool isShooting = false;
	private float threshold = 0.3f;
	private CatapultsController cataCtrl = null;
	private Platform platform;
	[HideInInspector]
	public bool isControllable = false;

	// Use this for initialization
	void Start () {
		//if(isControllable) {
		Input.gyro.enabled = true;
		RotateJoystick = GameObject.Find ("CNJoystick").GetComponent<CNJoystick>();
		//}
		_transformCache = GetComponent<Transform>();
	}

	//this script should only be called in current player
	public void setCatapult(GameObject pCatapult) {
		Catapult = pCatapult;
		animator = Catapult.GetComponent<Animator>();
		_catapultTransformCache = Catapult.GetComponent<Transform>();
		cataCtrl = Catapult.GetComponent<CatapultsController>();
		cataCtrl.isControllable = true;
		//cataCtrl.TargetPoint = GameObject.Find ("TargetPoint").transform; //set directly in inspector
		_parentTransformCache = _transformCache.parent;
		_transformCache.Rotate (-10, 0, 0); //turn view angle upward 10 degrees
		Vector3 giantWaistPos = GameObject.Find ("GiantWaist").transform.position;
		giantWaistPos.y = _catapultTransformCache.transform.position.y;
		_catapultTransformCache.LookAt (giantWaistPos);
	}

	private const float angleOffset = 0.05f;

	// Update is called once per frame
	void Update () {
		if(isControllable) {
			if(RotateJoystick != null && Catapult != null){
				float rotationX = RotateJoystick.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
				float rotationY = RotateJoystick.GetAxis("Vertical") * RotationSpeed * Time.deltaTime;
				_parentTransformCache.Rotate(0f, rotationX, 0f, Space.World);
				_catapultTransformCache.Rotate(0f, rotationX, 0f, Space.World);
				float curViewAngle = _transformCache.eulerAngles.x;
				//Debug.Log (curViewAngle);
				if(viewAngleUpperBound - curViewAngle - angleOffset > -rotationY && curViewAngle- viewAngleLowerBound - angleOffset > rotationY) {
					_transformCache.Rotate(-rotationY, 0f, 0f);
				}
			}
			if(isShooting == false){	
				StartShooting();
			}
		}
	}
	private void StartShooting(){
		if((Input.GetKeyDown(KeyCode.Space) || PhoneShooting()) && animator != null && cataCtrl.Stone_clone != null){
			//photonView.RPC ("startShootingRPC",PhotonTargets.All,null);
			startShootingRPC(); //called locally
		}
	}

	[RPC]
	public void startShootingRPC() {
		Debug.Log ("shot");
		animator.SetBool(state_shot, true);
		isShooting = true;
		Invoke("StopShooting", 0.7f);
	}

	private void StopShooting(){
		if (animator != null) {
			animator.SetBool (state_shot, false);
			isShooting = false;
		}
	}
	private bool PhoneShooting(){
		if(Input.gyro.rotationRateUnbiased.z > threshold)
			return true;
		return false;
	}
}
