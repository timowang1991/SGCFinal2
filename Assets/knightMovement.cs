using UnityEngine;
using System.Collections;
using System;

public class knightMovement : Photon.MonoBehaviour {
	
	//public Animator knightAnim;
	public CNAbstractController leftJoystick;
	public CNAbstractController rightJoystick;
	Rigidbody playerRigidbody;
	Animator anim; 
	Vector3 movement;
	public float speed; 
	public float turnSmoothing = 15f;
	public float rotSpeed = 90.0f;
	public float shakeSpeed = 90.0f;

	// Use this for initialization
	void Start () {
		// Set up references.
		anim = GetComponent<Animator> ();
		leftJoystick = GameObject.FindGameObjectWithTag ("Left_Joystick").GetComponent<CNJoystick> ();
		rightJoystick = GameObject.FindGameObjectWithTag ("Right_Joystick").GetComponent<CNJoystick> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}
	

	void Update()
	{
		if (anim.GetBool("Die") == true) {
			this.enabled = false;
		}

		float h = leftJoystick.GetAxis ("Horizontal");
		float v = leftJoystick.GetAxis ("Vertical");
		float rh = rightJoystick.GetAxis ("Horizontal");
		float rv = rightJoystick.GetAxis ("Vertical");

		UpdateMovement (h,v);
		UpdateRotateMovement (h,v);
		UpdateGyro ();
		UpdateAnimation (h, v, rh, rv);
		
	}
	
	void UpdateMovement (float h, float v) {

		if ( v > 0.3f) {
			movement = transform.forward;
			//movement.x = 0f;
			//movement.y = 0f;
			speed = movement.normalized.magnitude;
			movement = movement.normalized * speed * 80 * Time.deltaTime;
			//print ("movement: "+movement);
			// Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);
		} 
		else if(v < -0.3f){
			//movement = -1*transform.localPosition.forward;
			movement = -transform.forward;//-transform.localPosition.z;
			//movement.x = 0f;
			//movement.y = 0f;
			speed = movement.normalized.magnitude;
			movement = movement.normalized * speed * 80 * Time.deltaTime;
			//print ("movement: "+movement);
			// Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);
		}

	}
	void UpdateRotateMovement (float h,float v)
	{	
		// Create a new vector of the horizontal and vertical inputs.
		/*Vector3 t = new Vector3 (h, 0, v);
		t = t.normalized;
		Vector3 targetDirection = new Vector3(50f * t.x, 0f, 50f * t.z);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation , targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);*/

		if (h > 0.3f) {
			//print("H value:"+h);
			//transform.Rotate (Vector3.up * Time.deltaTime, Space.World);
			transform.Rotate (Vector3.up * Time.deltaTime * 100f);
		} 
		else if(h < -0.3f){
			transform.Rotate (-Vector3.up * Time.deltaTime * 100f);
		}
	}
	void UpdateGyro (){
		if (Input.gyro.rotationRate.y > shakeSpeed) {
			Vector3 forward = transform.forward;
			forward.y = 0f;
			movement = forward.normalized*10f;
			playerRigidbody.MovePosition (transform.position + movement);
		}

	}

	void UpdateAnimation (float h, float v, float rh, float rv)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = v != 0f || h != 0f;
		//bool turning = h > 0.5f;
		bool direction = v > 0.3f;
		bool running = v != 0f || h != 0f;//v > 0.8f || v < -0.8f;
		bool shield = rv < 0f;
		bool attack = rv > 0f;
	
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("isWalking", walking);
		//anim.SetBool ("isTurning", turning);
		anim.SetBool ("direction", direction);
		anim.SetBool ("isRunning", running);
		anim.SetBool ("isShield", shield);
		anim.SetFloat ("turningSpeed", 0f);
		anim.SetBool ("isattack", attack);
		/*if (rv > 0) {
			anim.SetBool ("attack", true);
		}*//* else if (rv < 0) {
				anim.SetTrigger ("shield");
		}*/
		
		
		//anim.SetTrigger ("jump", jump);
		
	}
}
