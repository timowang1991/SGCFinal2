using UnityEngine;
using System.Collections;

public class knightMovement : Photon.MonoBehaviour {
	
	//public Animator knightAnim;
	public CNAbstractController leftJoystick;
	public CNAbstractController rightJoystick;
	Rigidbody playerRigidbody;
	Animator anim; 
	Vector3 movement;
	public float speed; 
	public float turnSmoothing = 15f;

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
		float h = leftJoystick.GetAxis ("Horizontal");
		float v = leftJoystick.GetAxis ("Vertical");
		float rh = rightJoystick.GetAxis ("Horizontal");
		float rv = rightJoystick.GetAxis ("Vertical");

		UpdateMovement (h,v);
		UpdateRotateMovement (h,v);
		UpdateAnimation (h, v, rh, rv);
		
	}
	
	void UpdateMovement (float h, float v) {
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		speed = movement.normalized.magnitude;
		// Normalise the movement vector and make it proportional to the speed per second.
		if (v < 0) {
			movement = movement.normalized * -speed * 30 * Time.deltaTime;
		} else {
			movement = movement.normalized * speed * 30 * Time.deltaTime;
		}
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}
	void UpdateRotateMovement (float h,float v)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(h, 0f, v);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);

	}
	void UpdateAnimation (float h, float v, float rh, float rv)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = v != 0f || h != 0f;
		//bool turning = h > 0.5f;
		bool direction = v > 0.1f;
		bool running = v != 0f || h != 0f;//v > 0.8f || v < -0.8f;
		bool shield = rv < 0f;
	
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("isWalking", walking);
		//anim.SetBool ("isTurning", turning);
		anim.SetBool ("direction", direction);
		anim.SetBool ("isRunning", running);
		anim.SetBool ("isShield", shield);
		anim.SetFloat ("turningSpeed", 0f);
		if (rv > 0) {
				anim.SetTrigger ("attack");
		}/* else if (rv < 0) {
				anim.SetTrigger ("shield");
		}*/
		
		
		//anim.SetTrigger ("jump", jump);
		
	}
}
