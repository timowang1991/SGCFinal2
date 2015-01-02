using UnityEngine;
using System.Collections;

public class MobileController : MonoBehaviour {

	public CNAbstractController RotateJoystick;
	public float RotationSpeed = 10f;
	[HideInInspector]
	public GameObject Catapult = null;
	public float viewAngleLowerBound = 0;
	private Transform _transformCache;
	private Transform _parentTransformCache;
	private Transform _catapultTransformCahce;

	private Animator animator = null;
	private string state_shot = "Shoot_Stone";
	private bool isShooting = false;
	private float threshold = 0.3f;
	private CatapultsController cataCtrl = null;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
		_transformCache = GetComponent<Transform>();
		RotateJoystick = GameObject.Find ("CNJoystick").GetComponent<CNJoystick>();
	}

	public void setCatapult(GameObject pCatapult) {
		Catapult = pCatapult;
		animator = Catapult.GetComponent<Animator>();
		_catapultTransformCahce = Catapult.GetComponent<Transform>();
		cataCtrl = Catapult.GetComponent<CatapultsController>();
		cataCtrl.TargetPoint = GameObject.Find ("TargetPoint").transform;
		_parentTransformCache = _transformCache.parent;
		  Vector3 posToLookAtInLocal = new Vector3 (0, viewAngleLowerBound + 0.5f, 0);
		_transformCache.LookAt(posToLookAtInLocal + pCatapult.transform.position);


	}

	// Update is called once per frame
	void Update () {
		if(RotateJoystick != null && Catapult != null){
			float rotationX = RotateJoystick.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
			float rotationY = RotateJoystick.GetAxis("Vertical") * RotationSpeed * Time.deltaTime;
			_parentTransformCache.Rotate(0f, rotationX, 0f, Space.World);
			_catapultTransformCahce.Rotate(0f, rotationX, 0f, Space.World);
			//if(_transformCache.rotation.x >= viewAngleLowerBound) {
				_transformCache.Rotate(-rotationY, 0f, 0f);
			//}
		}
		if(isShooting == false){	
			StartShooting();
		}

	}
	private void StartShooting(){
		if((Input.GetKeyDown(KeyCode.Space) || PhoneShooting()) && animator != null && cataCtrl.Stone_clone != null){
			Debug.Log ("shot");
			animator.SetBool(state_shot, true);
			isShooting = true;
			Invoke("StopShooting", 0.7f);
		}
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
