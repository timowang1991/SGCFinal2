using UnityEngine;
using System.Collections;

public class MagicianThrowTornado : Photon.MonoBehaviour {

	public int CostMP = 10; 
	public GameObject Tornado_Prefab;
	private Transform camTrans;
	public Transform TornadoPosition;

//	private float[] samples;          // Samples
//	private float[] spectrum;         // Spectrum
//	private int sampleCount = 10;      // Sample Count.
//	private float refValue = 0.1f;    // RMS value for 0 dB.
//	private float threshold = 0.02f;  // Minimum amplitude to extract pitch (recieve anything)
//	private float rmsValue;           // Volume in RMS
//	private float dbValue;            // Volume in DB
//	private float pitchValue;         // Pitch - Hz (is this frequency?)

	void Start()
	{
		if (Tornado_Prefab == null) {
			Debug.LogError("NO MagicianTornado Loaded");
		}
		camTrans = Camera.main.transform;
		Debug.Log ("CamName:" + camTrans.name);

//		if(photonView.isMine)
//		{
//			samples = new float[sampleCount];
//			spectrum = new float[sampleCount];
//			Debug.Log(Microphone.devices);
//			audio.clip = Microphone.Start("Built-in Microphone",false,1,48000);
//			audio.Play();
//		}
	}
	public void CastSpell()
	{
		
		if ( photonView.isMine && this.gameObject.GetComponent<MagicianMPManager> ().UseMP (CostMP)) {
			//this.photonView.RPC("FireTheFireBall",PhotonTargets.All);
			this.photonView.RPC("generateTornado",PhotonTargets.All,this.photonView.viewID);
		}
	}

	[RPC]
	void generateTornado(int viewID)
	{
		GameObject Tornado = (GameObject)Instantiate (Tornado_Prefab, TornadoPosition.position,Quaternion.identity);
		Tornado.GetComponent<TornadoSelfScript>().PlayerPosition = PhotonView.Find(viewID).gameObject.transform;
	}
//	public void Update () {
//		
//		 //Le big cheese doing its thing.
//		if ( photonView.isMine)
//		{
//			AnalyzeSound();
//			Debug.Log("RMS: " + rmsValue.ToString("F2") + " (" + dbValue.ToString("F1") + " dB)\n" + "Pitch: " + pitchValue.ToString("F0") + " Hz");
//		}
//	}
//
//	private void AnalyzeSound() {
//		audio.GetOutputData(samples, 0); // Get all of our samples from the mic.
//		
//		// Sums squared samples
//		float sum = 0;
//		for (int i = 0; i < sampleCount; i++){
//			Debug.Log(samples[i]);
//			sum += Mathf.Pow(samples[i], 2);
//		}
//		
//		rmsValue = Mathf.Sqrt(sum/sampleCount);          // RMS is the square root of the average value of the samples.
//		dbValue = 20*Mathf.Log10(rmsValue/refValue);  // dB
//		
//		// Clamp it to -160dB min
//		if (dbValue < -160) {
//			dbValue = -160;
//		}
////		
////		// Gets the sound spectrum.
//		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
//		float maxV = 0;
//		int maxN = 0;
////		
////		// Find the highest sample.
//		for (int i = 0; i < sampleCount; i++){
//			if (spectrum[i] > maxV && spectrum[i] > threshold){
//				maxV = spectrum[i];
//				maxN = i; // maxN is the index of max
//			}
//		}
//		
//		// Pass the index to a float variable
//		float freqN = maxN;
//		
//		// Interpolate index using neighbours
//		if (maxN > 0 && maxN < sampleCount - 1) {
//			float dL = spectrum[maxN-1] / spectrum[maxN];
//			float dR = spectrum[maxN+1] / spectrum[maxN];
//			freqN += 0.5f * (dR * dR - dL * dL);
//		}
//		
//		// Convert index to frequency
//		pitchValue = freqN * 24000 / sampleCount;
//	}
}
