using UnityEngine;
using System.Collections;

public class UpdateWithPosition : MonoBehaviour {

	public Transform obj;
	public float speed = 0.6f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(obj != null)
		{
			transform.position = Vector3.Lerp(transform.position,obj.transform.position,Time.deltaTime*speed);
		}
	}
}
