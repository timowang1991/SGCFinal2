
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
		#if !UNITY_IOS
		if(platform == Platform.PC_Giant)
		{
			this.GetComponent<AvatarController>().stopMove = true;
		}
		#endif
		Invoke ("ChangeFreezeBack", HowLongItLast);
	}
	
	void ChangeFreezeBack()
	{
		#if !UNITY_IOS
		if(platform == Platform.PC_Giant)
		{
			this.GetComponent<AvatarController>().stopMove = false;
		}
		#endif
	}
}
