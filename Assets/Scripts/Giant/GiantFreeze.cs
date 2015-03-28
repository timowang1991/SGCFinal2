using UnityEngine;
using System.Collections;

public class GiantFreeze : MonoBehaviour {

	Platform platform;

	// Use this for initialization
	void Start () {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
	}
	
	[RPC]
	void FreezeGiant(int HowLongItLast)
	{
		if(platform == Platform.PC_Giant)
		{
			this.GetComponent<AvatarController>().stopMove = true;
		}
		Invoke ("ChangeFreezeBack", HowLongItLast);
	}
	
	void ChangeFreezeBack()
	{
		if(platform == Platform.PC_Giant)
		{
			this.GetComponent<AvatarController>().stopMove = false;
		}
	}
}
