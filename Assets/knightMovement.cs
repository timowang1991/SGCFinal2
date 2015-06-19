using UnityEngine;
using System.Collections;

public class knightMovement : Photon.MonoBehaviour {
	
	//public Animator knightAnim;
	public CNAbstractController leftJoystick;
	public CNAbstractController rightJoystick;
	Vector3 movement;
	Rigidbody playerRigidbody;
	public float speed; 
	Animator anim; 
	
	
	// Use this for initialization
	void Start () {
		// Set up references.
		anim = GetComponent<Animator> ();
		leftJoystick = GameObject.FindGameObjectWithTag ("Left_Joystick").GetComponent<CNJoystick> ();
		rightJoystick = GameObject.FindGameObjectWithTag ("Right_Joystick").GetComponent<CNJoystick> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		
	}
	
	void Awake ()
	{
		// Create a layer mask for the floor layer.
		//floorMask = LayerMask.GetMask ("Floor");
		
		
	}
	void Update()
	{
		float h = leftJoystick.GetAxis ("Horizontal");
		float v = leftJoystick.GetAxis ("Vertical");
		float rh = rightJoystick.GetAxis ("Horizontal");
		float rv = rightJoystick.GetAxis ("Vertical");
		//UpdateRotateMovement ();
		UpdateMovement (h,v);
		UpdateAnimation (h, v, rh, rv);
		
	}
	
	void UpdateMovement (float h, float v) {
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		speed = h;
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed *10* Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}
	void UpdateAnimation (float h, float v, float rh, float rv)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;
		
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("isWalking", walking);
		Debug.Log ("h"+h);
		Debug.Log ("v"+v);
		anim.SetFloat ("speed", speed);
		Debug.Log ("speed"+speed);
		anim.SetFloat ("turningSpeed", 0);
		if (rh > 0)
		{
			anim.SetTrigger("attack");
		}
		
		//anim.SetTrigger ("jump", jump);
		
	}
}
