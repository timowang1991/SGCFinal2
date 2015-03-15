using UnityEngine;
using System.Collections;

public class OculusCamRefScript : MonoBehaviour {

	GameObject oculusCam;
	Platform platform;

	// Use this for initialization
	void Start () {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
		if (platform == Platform.PC_Giant) {
			oculusCam = transform.parent.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (platform == Platform.PC_Giant){
			transform.position = oculusCam.transform.position;
			transform.rotation = oculusCam.transform.rotation;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			// Network player, receive data
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
