using UnityEngine;
using System.Collections;

public class MagicianDetectBlowFromUser : MonoBehaviour {


	public float throwRate = 0.5F;
	private float nextThrow = 0.0F;

	// Use this for initialization
	void Start () {
		audio.clip = Microphone.Start ("Built-in Microphone", true, 1, 44100);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (audio.isPlaying) {
				
		}
		else if(audio.clip.isReadyToPlay){
			audio.Play();
		}
		else{
			audio.clip = Microphone.Start ("Built-in Microphone", true, 1, 44100);
		}

		float y = audio.GetSpectrumData (128, 0, FFTWindow.BlackmanHarris) [64] * 1000000;
		//Debug.Log (y);

		if(y>800 && Time.time > nextThrow)
		{
			nextThrow = Time.time + throwRate;
			//sorry bad code here XDD
			this.GetComponent<MagicianFunctionalUI>().ReceivedMessageFromUI("Tornado");
		}
	}
}
