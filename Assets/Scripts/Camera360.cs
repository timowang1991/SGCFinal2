using UnityEngine;
using System.Collections;

public class Camera360 : MonoBehaviour {

	private Transform originalPoint;
	public Transform middlePoint;
	public int rotateSpeed = 20;
	public int moveSpeed = 50;
	// Use this for initialization
	void Start () {
		originalPoint = middlePoint;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tmpPoint = Vector3.Lerp (originalPoint.position, middlePoint.position, Time.deltaTime);

		this.transform.LookAt (tmpPoint);
		transform.RotateAround(tmpPoint, Vector3.down, rotateSpeed * Time.deltaTime);

		if (Input.GetKey(KeyCode.UpArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
			}
			else
			{
				this.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed,Space.World);
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				this.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
			}
			else
			{
				this.transform.Translate(Vector3.down * Time.deltaTime* moveSpeed,Space.World);
			}

		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.transform.Translate(Vector3.right * Time.deltaTime* moveSpeed,Space.World);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.transform.Translate(Vector3.left * Time.deltaTime* moveSpeed,Space.World);
		}
	}
}
