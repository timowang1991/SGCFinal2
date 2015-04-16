#pragma strict

var speed : float = 10;
var lifeTime : float;
var dist : float = 10000;
private var spawnTime : float = 0.0;
private var tr : Transform;
function OnEnable () {
	tr = transform;
	spawnTime = Time.time;
}

function Update () {
	tr.position += tr.forward * speed * Time.deltaTime;
	dist -= speed * Time.deltaTime;
	if (Time.time > spawnTime + lifeTime) {
		Destroy (gameObject);
	}
}
