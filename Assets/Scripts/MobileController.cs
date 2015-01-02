using UnityEngine;
using System.Collections;

public class MobileController : MonoBehaviour {

	public CNAbstractController RotateJoystick;
	public float RotationSpeed = 10f;
	public GameObject Catapult;

	private Transform _transformCahce;
	private Transform _parentTransformCache;
	private Transform _catapultTransformCahce;

	private Animator animator;
	private string state_shot = "Shoot_Stone";
	private bool isShooting = false;
	private float threshold = 0.3f;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
		_transformCahce = GetComponent<Transform>();
		_parentTransformCache = _transformCahce.parent;
		if(Catapult != null){
			animator = Catapult.GetComponent<Animator>();
		}
		_catapultTransformCahce = Catapult.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(RotateJoystick != null){
			float rotationX = RotateJoystick.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
			float rotationY = RotateJoystick.GetAxis("Vertical") * RotationSpeed * Time.deltaTime;
			_parentTransformCache.Rotate(0f, rotationX, 0f, Space.World);
			_catapultTransformCahce.Rotate(0f, rotationX, 0f, Space.World);
			_parentTransformCache.Rotate(-rotationY, 0f, 0f);
		}
		if(isShooting == false){	
			StartShooting();
		}

	}
	private void StartShooting(){
		if((Input.GetKeyDown(KeyCode.Space) || PhoneShooting()) && animator != null && Catapult.GetComponent<CatapultsController>().Stone_clone != null){
			Debug.Log ("shot");
			animator.SetBool(state_shot, true);
			isShooting = true;
			Invoke("StopShooting", 0.7f);
		}
	}
	private void StopShooting(){
		animator.SetBool(state_shot, false);
		isShooting = false;
	}
	private bool PhoneShooting(){

		if(Input.gyro.rotationRateUnbiased.z > threshold)
			return true;
		return false;
	}
}
