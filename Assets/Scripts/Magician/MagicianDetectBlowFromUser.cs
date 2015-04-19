using UnityEngine;
using System.Collections;

public class MagicianDetectBlowFromUser : MonoBehaviour {


	public float throwRate = 1.0F;
	private float nextThrow = 0.0F;

	private int sampleCount = 1024;      // Sample Count.
	private float refValue = 0.1f;    // RMS value for 0 dB.
	private float threshold = 0.02f;  // Minimum amplitude to extract pitch (recieve anything)
	private float rmsValue;           // Volume in RMS
	public float dbValue;            // Volume in DB
	private float pitchValue;         // Pitch - Hz (is this frequency?)
	
	private float[] samples;          // Samples
	private float[] spectrum;         // Spectrum



	// Use this for initialization
	void Start () {
		samples = new float[sampleCount];
		spectrum = new float[sampleCount];
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
		audio.GetOutputData(samples, 0); // Get all of our samples from the mic.
		
		// Sums squared samples
		float sum = 0;
		for (int i = 0; i < sampleCount; i++){
			sum += Mathf.Pow(samples[i], 2);
		}
		
		rmsValue = Mathf.Sqrt(sum/sampleCount);          // RMS is the square root of the average value of the samples.
		dbValue = 20*Mathf.Log10(rmsValue/refValue);  // dB
		
		// Clamp it to -160dB min
		if (dbValue < -160) {
			dbValue = -160;
		}
		
		// Gets the sound spectrum.
		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		float maxV = 0;
		int maxN = 0;
		
		// Find the highest sample.
		for (int i = 0; i < sampleCount; i++){
			if (spectrum[i] > maxV &&  spectrum[i] > threshold){
				maxV = spectrum[i];
				maxN = i; // maxN is the index of max
			}
		}
		
		// Pass the index to a float variable
		float freqN = maxN;
		
		// Interpolate index using neighbours
		if (maxN > 0 && maxN < sampleCount - 1) {
			float dL = spectrum[maxN-1] / spectrum[maxN];
			float dR = spectrum[maxN+1] / spectrum[maxN];
			freqN += 0.5f * (dR * dR - dL * dL);
		}
		
		// Convert index to frequency
		pitchValue = freqN * 24000 / sampleCount;


//		float y = audio.GetSpectrumData (128, 0, FFTWindow.BlackmanHarris) [64] * 1000000;
//		//Debug.Log (y);
//
		if(dbValue > 10.0f && Time.time > nextThrow)
		{
			nextThrow = Time.time + throwRate;
			//sorry bad code here XDD
			this.GetComponent<MagicianFunctionalUI>().ReceivedMessageFromUI("Tornado");
		}
//		Debug.Log ("dbvalue:" + dbValue);
	}
}
