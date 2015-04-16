#pragma strict
public var rocketPrefab : Rigidbody;
public var positionr: Transform;
var launchsound:AudioClip;
private var timer:float=0;
private var Helispark:helispark;
var gunparticle:GameObject;
var gunsound:AudioClip;
var muzzle:GameObject;
var gunhit:GameObject;
function Awake () {
	Helispark=gameObject.GetComponent(helispark);
}
function Update ()
{
	timer+=Time.deltaTime;

    if(Input.GetButtonDown("Fire1")&&timer>2&&Helispark.state==false)
    {
    	launchMissile();
    }
    #if !UNITY_ANDROID 
    if(Input.GetKey(KeyCode.Mouse0)&&Helispark.state==false){
    Fire();
    }
    
    else {
    muzzle.SetActive(false);
    } 
    #endif
}

function OnGUI() {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
if( GUI.Button(Rect(Screen.width-(Screen.width/6),Screen.height/2,60,30),"Fire")){
	launchMissile();
}
if( GUI.Button(Rect(Screen.width-(Screen.width/6),Screen.height/2-40,60,30),"Gun")){
	//machineGun();
}else {
    muzzle.SetActive(false);
} 

#endif
}


function launchMissile() {
	timer=0;
	var rocketInstance : Rigidbody;
	rocketInstance = Instantiate(rocketPrefab, positionr.position, positionr.rotation);
	rocketInstance.AddForce(positionr.forward * 5000);
	audio.PlayOneShot(launchsound);
}

function Fire() {
	Instantiate(gunparticle,positionr.position,positionr.rotation);
    muzzle.SetActive(true);
    audio.PlayOneShot(gunsound);
    var hit:RaycastHit;
    if(Physics.Raycast(positionr.position, positionr.forward, hit, 50))
	{
		if(hit.collider.rigidbody)
		{
			var vnorm = Quaternion(hit.normal.z, hit.normal.y, -hit.normal.x, 1); //this is for creating a Quaternion variable that contains the rotation of the surface i hit to put in the instantiate.

			var instant:GameObject=Instantiate(gunhit, hit.point, vnorm); 

			hit.collider.rigidbody.AddForceAtPosition(transform.forward * 100, hit.point);
		}
		else
		{
			var vnorme = Quaternion(hit.normal.z, hit.normal.y, -hit.normal.x, 1);

			var instant2:GameObject=Instantiate(gunhit, hit.point, vnorme);
		}
    }
}