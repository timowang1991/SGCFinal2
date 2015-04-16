#pragma strict
   
var sparksound:AudioClip;
var FallImpactVel:float=-40;//The negative velocity below which to detect whether player falls from too much height
var health:int =100;
var state: boolean =false;
var rotor :GameObject;
var DamageFactor:float=1;
var refr:GameObject;
var distanceToGround:float;
var particlefire:GameObject;
var explosion:GameObject;
var detonator:GameObject;
var textureburn:Texture;
var normaltexture:Texture;
var mainmaterial:Material;
var scorchMark:GameObject;
var fire:GameObject;
var hitParticles:GameObject;
public var mainrotor_axis : int=1;

@HideInInspector
public var isControllable : boolean = false; 

private var altitude :float=0;
private var Helifin :HelifinNet;
private var deathtimer:float=0;
private var i:int=0;
private var Dieingtimer:float=0;

function Awake() {
	Helifin=gameObject.GetComponent(HelifinNet);
}	

// Use this for initialization
function Start () 
{
	particlefire.SetActive(false);
	mainmaterial.mainTexture=normaltexture;
}

// Update is called once per frame
function Update () 
{
	if(isControllable) {
		if(rigidbody.velocity.y<FallImpactVel){
			if(Helifin.altitude<2){
				health+=1*rigidbody.velocity.y*DamageFactor;
			}
		}

	    if(health<0){
			state=true;
			falling ();
		}
		
		if(health<20){
			fire.SetActive(true);
		}
	}
	
    if(state==true){
		particlefire.SetActive(true);
	}
	
}

function falling() {
	
	deathtimer+=Time.deltaTime;
	if(deathtimer>10){
		explode();
	}
	
	switch(mainrotor_axis)
	{	
		case 1:
			rotor.transform.Rotate(1000*Random.value,0,0);
			break;
		case 2:
			rotor.transform.Rotate(0,1000*Random.value,0);
			break;
		case 3:
			rotor.transform.Rotate(0,0,1000*Random.value);
			break;	
			
	}
	
	audio.pitch=Random.Range(0.1,0.6);
	altitude = Helifin.altitude;

	if(altitude<2){
		explode();	
	}
	
	if(Vector3.Angle(refr.transform.up,Vector3.down)<30){
		explode();
	}

}

function explode() //exploding the helicopter and creating d4ecals
{
    print ("exploded"+distanceToGround);
	audio.Stop();
	rigidbody.AddExplosionForce(427600,transform.position,100);
	Instantiate(detonator,explosion.transform.position,transform.rotation);
    mainmaterial.mainTexture=textureburn;
    scorchMark=Instantiate(scorchMark,Vector3.zero,Quaternion.identity);
	var scorchPosition : Vector3 = refr.collider.ClosestPointOnBounds (transform.position - Vector3.up * 100);
	scorchMark.transform.position = scorchPosition + Vector3.up * 1.1;
	scorchMark.transform.eulerAngles.y = Random.Range (0.0, 90.0);
	Destroy (this);
}
