using UnityEngine;
using System.Collections;

public class Camera360 : MonoBehaviour {

	public GameObject middlePoint;
	public int speed = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (middlePoint.transform.position);
		transform.RotateAround(middlePoint.transform.position, Vector3.down, speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.UpArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}
			else
			{
				this.transform.Translate(Vector3.up * Time.deltaTime * speed,Space.World);
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				this.transform.Translate(Vector3.back * Time.deltaTime * speed);
			}
			else
			{
				this.transform.Translate(Vector3.down * Time.deltaTime* speed,Space.World);
			}

		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.transform.Translate(Vector3.right * Time.deltaTime* speed,Space.World);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.transform.Translate(Vector3.left * Time.deltaTime* speed,Space.World);
		}
	}
}
