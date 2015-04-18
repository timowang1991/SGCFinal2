#pragma strict


var leftjoystick: Joystick;
var rightjoystick: Joystick;
var main_Rotor_GameObject: GameObject;
var tail_Rotor_GameObject: GameObject;
var Wind: GameObject;
var dust: GameObject;
var leave: GameObject;
var max_tail_Rotor_Force: float = 15000.0;
var max_Tail_Rotor_Velocity: float = 2200.0;
var Hover_Const: float = 2f;
var gui: GUIStyle; 
var labelPosition: Rect;
var layer: LayerMask = 0;
var altitude: float;
var refr: GameObject;

private var max_Rotor_Force: float = 30000;
private var max_Rotor_Velocity: float = 10000;
private var StablisingConstant: float = 4f;
static var rotor_Velocity: float = 0.0;
private var rotor_Rotation: float = 0.0;
public var mainrotor_axis: int = 1;
public var tailrotor_axis: int = 1;
private var tail_rotor_Velocity: float = 0.0;
private var tail_rotor_Rotation: float = 0.0;
private var forward_Rotor_Torque_Multiplier: float = 3;
private var sideways_Rotor_Torque_Multiplier: float = 2.5f;
private var turnAroundSpeedMultiplier: float = 2.5f;
private var horVelAccMultiplier: int = 50; //accelerate helicopter's horizontal moving velocity
private var RotorSpeedIncreaseConstant: float = 30;
private var RestoringTorqueMultiplier: float = 4;
static var main_Rotor_Active: boolean = true;
static var tail_Rotor_Active: boolean = true;
private var healthy: helispark;
private var maxHeight: float = 1000f;
private var timer: float = 0f;
private var cannotRestore: boolean = false;

function Awake() {
    healthy = gameObject.GetComponent(helispark);
}

function FixedUpdate() { //Main physics forces section
    if (healthy.state == true) {}
    var torqueValue: Vector3;
    var controlTorque: Vector3;
    var value1: float;
    var value2: float;
    
    #if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
    controlTorque = Vector3(leftjoystick.position.y * forward_Rotor_Torque_Multiplier, 1.0, rightjoystick.position.x * sideways_Rotor_Torque_Multiplier);
    #else
    /*
    if (Input.GetAxis("Vertical") != 0) {
        value1 = Input.GetAxis("Vertical");
    } else {
        value1 = Input.GetAxis("Mouse Y");
    }
    if (Input.GetAxis("Horizontal2") != 0) {
        value2 = Input.GetAxis("Horizontal2");
    } else {
        value2 = -Input.GetAxis("Mouse X");
    }
    */
    value1 = Input.GetAxis("Vertical");
    value2 = Input.GetAxis("Horizontal2");
    controlTorque = Vector3(-value1 * forward_Rotor_Torque_Multiplier, 1, -value2 * sideways_Rotor_Torque_Multiplier);
    #endif
    
    //make horizontal moving much faster
    rigidbody.velocity += ((Vector3.ProjectOnPlane(transform.right,Vector3.up).normalized * value2 + Vector3.ProjectOnPlane(transform.forward,Vector3.up).normalized * -value1) * horVelAccMultiplier * Time.deltaTime);
    if (main_Rotor_Active == true) {
        torqueValue += (controlTorque * max_Rotor_Force * rotor_Velocity);
        if (altitude < maxHeight) {
            rigidbody.AddForce(Vector3.up * max_Rotor_Force * rotor_Velocity);
        }
    }
    /*
    if (rigidbody.velocity.y < 0 && rotor_Velocity < 0.3) {
        rigidbody.AddForce(Vector3.down * 30000);
    }
    */
    if (Vector3.Angle(Vector3.up, transform.up) < 80) {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 360), Time.deltaTime * rotor_Velocity * StablisingConstant);
    }


    if (tail_Rotor_Active == true) {
        torqueValue -= (Vector3.up * max_tail_Rotor_Force * tail_rotor_Velocity);
    }
    //Fix v4.0 Helicopter not restoring z position when bent forward
    if (Input.GetAxis("Vertical") != 0) {
        if (Input.GetAxis("Horizontal2") == 0) {
            if (transform.localRotation.eulerAngles.z > 5 && transform.localRotation.eulerAngles.z < 80) {
                rigidbody.AddRelativeTorque(0, 0, -rotor_Velocity * 1000 * transform.localRotation.eulerAngles.z * RestoringTorqueMultiplier);
            }
            if (transform.localRotation.eulerAngles.z > 190 && transform.localRotation.eulerAngles.z < 355) {
                rigidbody.AddRelativeTorque(0, 0, rotor_Velocity * 1000 * (360 - transform.localRotation.eulerAngles.z) * RestoringTorqueMultiplier);
            }
        }
    }

    rigidbody.AddRelativeTorque(torqueValue);
}

function Start() {

}

function dead() {

    Destroy(Wind);
    Destroy(GetComponent(helifin));
}

function Update() {

    if (healthy.state == true) {
        dead();
    }
    if (main_Rotor_Active == true) {
        switch (mainrotor_axis) {
            case 1:
                main_Rotor_GameObject.transform.Rotate(rotor_Velocity * 40, 0, 0);
                break;
            case 2:
                main_Rotor_GameObject.transform.Rotate(0, rotor_Velocity * 40, 0);
                break;
            case 3:
                main_Rotor_GameObject.transform.Rotate(0, 0, rotor_Velocity * 40);
                break;

        }

    }
    if (tail_Rotor_Active == true) {
    	#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
        if (leftjoystick.position.x > 0) {
            switch (tailrotor_axis) {
                case 1:
                    tail_Rotor_GameObject.transform.Rotate(tail_rotor_Velocity * 20, 0, 0);
                    break;
                case 2:
                    tail_Rotor_GameObject.transform.Rotate(0, tail_rotor_Velocity * 20, 0);
                    break;
                case 3:
                    tail_Rotor_GameObject.transform.Rotate(0, 0, tail_rotor_Velocity * 20);
                    break;

            }

        }
        #endif
        if (Input.GetAxis("Horizontal") == 1) {
            switch (tailrotor_axis) {
                case 1:
                    tail_Rotor_GameObject.transform.Rotate(tail_rotor_Velocity * 20, 0, 0);
                    break;
                case 2:
                    tail_Rotor_GameObject.transform.Rotate(0, tail_rotor_Velocity * 20, 0);
                    break;
                case 3:
                    tail_Rotor_GameObject.transform.Rotate(0, 0, tail_rotor_Velocity * 20);
                    break;

            }

        } else {
            switch (tailrotor_axis) {
                case 1:
                    tail_Rotor_GameObject.transform.Rotate(tail_rotor_Velocity * 20, 0, 0);
                    break;
                case 2:
                    tail_Rotor_GameObject.transform.Rotate(0, tail_rotor_Velocity * 20, 0);
                    break;
                case 3:
                    tail_Rotor_GameObject.transform.Rotate(0, 0, tail_rotor_Velocity * 20);
                    break;

            }
        }
    }
    rotor_Rotation += max_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
    tail_rotor_Rotation += max_Tail_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
    var hover_Rotor_Velocity = (rigidbody.mass * Mathf.Abs(Physics.gravity.y) / max_Rotor_Force);
    var hover_Tail_Rotor_Velocity = (max_Rotor_Force * rotor_Velocity) / max_tail_Rotor_Force;
    if (Input.GetAxis("Vertical2") != 0.0) {
        rotor_Velocity += Input.GetAxis("Vertical2") * RotorSpeedIncreaseConstant * 0.001;
    }
    else {
        rotor_Velocity = Mathf.Lerp(rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 5 * Hover_Const);
    }
    
    //decrease height;
    if(Input.GetAxis("Vertical2") < 0) {
    	if(rotor_Velocity < 0.6) {
    		rotor_Velocity = 0.6;
    		rigidbody.velocity.y -= (20 * Time.deltaTime);
    		//Debug.Log(altitude);
    		if(altitude < 3) {
    			rotor_Velocity = hover_Rotor_Velocity;
    		}
    	}
    	
    		
    }
    
    #if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
    if (rightjoystick.position.y != 0.0) {
        rotor_Velocity += rightjoystick.position.y * RotorSpeedIncreaseConstant * 0.001;
    } else {
        rotor_Velocity = Mathf.Lerp(rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime);
    }
    #endif
    
    tail_rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal") * turnAroundSpeedMultiplier;
    
    /*
    if (Input.GetAxis("Horizontal") != 0) {
        tail_rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal");
    }
    else {
        tail_rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Mouse X");
    }
    */
    #if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
    tail_rotor_Velocity = hover_Tail_Rotor_Velocity - leftjoystick.position.x;
    #endif
    
    if (rotor_Velocity > 1.0) {
        rotor_Velocity = 1.0;
    } else if (rotor_Velocity < 0.0) {
        rotor_Velocity = 0.0;
    }
    audio.pitch = rotor_Velocity;
    var groundHit: RaycastHit;
    Physics.Raycast(refr.transform.position, -Vector3.up, groundHit, 10000, layer);
    altitude = groundHit.distance;

    extrafx();
}

function extrafx() {
    if (rotor_Velocity > 0.4) {
        Wind.SetActive(true);

    } else {
        Wind.SetActive(false);
    }


    if (healthy.state == false && altitude < 15) {
        dust.particleEmitter.emit = true;
        dust.particleEmitter.minEnergy = 2.2;
        dust.particleEmitter.maxEnergy = (rotor_Velocity / 1) * 6;
        leave.particleEmitter.emit = true;
        leave.particleEmitter.minEnergy = 2.2;
        leave.particleEmitter.maxEnergy = (rotor_Velocity / 1) * 6;
    } else {
        dust.particleEmitter.emit = false;
        leave.particleEmitter.emit = false;
    }
}

//
//function OnGUI()
//{
//	GUI.Label(labelPosition, "\nVel left - " + rigidbody.velocity.x + "m/s", gui);
//}