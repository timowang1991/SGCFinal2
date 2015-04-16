#pragma strict
public var missileInitSpeed : float;
public var rocketPrefab : Rigidbody;
public var positionr : Transform;
public var targetPos : Transform;
var launchsound:AudioClip;
private var timer:float=0;
private var Helispark:helispark;
var gunparticle:GameObject;
var gunsound:AudioClip;
var muzzle:GameObject;
var gunhit:GameObject;
private var shootingDir : Vector3;
private var shootingDirRotation : Quaternion;
public var poolAPIWrapper : PoolAPIWrapper;

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
    if(Input.GetKey(KeyCode.Mouse0)&&Helispark.state==false) {
    	FireWithMachineGun();
    }
    else {
    	muzzle.SetActive(false);
    } 
    #endif
}

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY || UNITY_TIZEN
function OnGUI() {

	if( GUI.Button(Rect(Screen.width-(Screen.width/6),Screen.height/2,60,30),"Fire")){
		launchMissile();
	}

	if( GUI.Button(Rect(Screen.width-(Screen.width/6),Screen.height/2-40,60,30),"Gun")){
		FireWithMachineGun();
	}else {
	    muzzle.SetActive(false);
	} 

}

#endif

function getShootingDirAndRotation() {
	shootingDir = (targetPos.position - positionr.position).normalized;
	shootingDirRotation = Quaternion.LookRotation(shootingDir, Vector3.up);
}

function launchMissile() {
	timer=0;
	var rocketInstance : Rigidbody;
	getShootingDirAndRotation();
	rocketInstance = Instantiate(rocketPrefab, positionr.position, shootingDirRotation);
	rocketInstance.velocity = (shootingDir * missileInitSpeed);
	audio.PlayOneShot(launchsound);
}

function FireWithMachineGun() {
	var bulletTrans : Transform = poolAPIWrapper.spawn("CopterRelated",gunparticle);
	if(bulletTrans == null) {
		return;
	}
	bulletTrans.position = positionr.position;
	bulletTrans.LookAt(targetPos.position);
	bulletTrans.rigidbody.velocity = bulletTrans.forward * 100;
	muzzle.SetActive(true);
    audio.PlayOneShot(gunsound);
  	  
    /*
    if(Physics.Raycast(positionr.position, shootingDir, hit, 50))
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
    */
}