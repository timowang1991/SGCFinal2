using UnityEngine;
using System.Collections;

public class ArrowSelfScript : MonoBehaviour {

	public enum ArrowState
	{
		holding,
		shooted,
		touched
	}
	public ArrowState state;
	public GameObject BloodFX;
	// Use this for initialization
	void Start () {
		state = ArrowState.holding;
		this.gameObject.transform.localScale = new Vector3(1f,1f,1f);
		//Destroy (this.gameObject, 10);
	}
	
	/// <summary>
	/// Scale up when the state is shooted (let small guys and Giant to see Arrow clearly.)
	/// </summary>
	void Update () {
		if (state == ArrowState.shooted) {
			if(transform.localScale.x < 30)
			{
				transform.localScale += new Vector3 (1F,1F,1F);
			}
		}
	}
	/// <summary>
	/// Same as Stone, Hit different tag object to hurt different value.
	/// </summary>
	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Weak")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().damage(10);
			AttachToCollisionPoint(other);
		}
		else if(other.gameObject.tag == "Weaker")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().damage(20);
			AttachToCollisionPoint(other);
		}
		else if(other.gameObject.tag == "Weakest")
		{
			Debug.Log ("Arrow: " + other.gameObject.tag);
			GameObject.FindGameObjectWithTag("GiantPlayer").GetComponent<GiantHealth>().damage(30);
			AttachToCollisionPoint(other);
		}
	}
	/// <summary>
	/// Stay at the point, and should be a child of the giant's body. This time just stay at the global position. (BUG)
	/// </summary>
	void AttachToCollisionPoint(Collision collision)
	{
		//gameObject.transform.position = Point.point;
		gameObject.rigidbody.isKinematic = true;
		state = ArrowSelfScript.ArrowState.touched;

		GameObject obj = (GameObject)Instantiate (BloodFX, collision.contacts[0].point, Quaternion.identity);
		obj.transform.parent = collision.gameObject.transform;
		Destroy(obj,10);
	}
}
