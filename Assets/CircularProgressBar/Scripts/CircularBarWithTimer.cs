using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CircularBarWithTimer : MonoBehaviour {
	
	public Image CircleImage;
	public Color start;
	public Color end;
	
	public Color current;

	public float timeToComplete = 1.5f;
	public float timeToDisappearAfterComplete = 0.5f;
	public float timeToUnloadFromComplete = 1.5f;

	float completion = 0.0f;
	
	void Start(){
		CircleImage.type = Image.Type.Filled;
		CircleImage.fillMethod = Image.FillMethod.Radial360;
		CircleImage.fillOrigin = 0;
	}
	
	void Update(){
		if(Input.GetButtonDown("Fire1")){
			RestartLoading();
		} else if(Input.GetButtonDown("Fire2")){
			PauseLoading();
		} else if(Input.GetButtonDown("Jump")){
			UnLoad(0);
		}
	}

	public void RestartLoading(){
		RestartLoading(this.timeToComplete);
	}

	public void RestartLoading(float timeToComplete){
		RestartLoading(timeToComplete, 0.0f);
	}

	public void RestartLoading(float timeToComplete, float startCompletion){
		completion = startCompletion;
		this.timeToComplete = timeToComplete;
		StopCoroutine("Loading");
		StopCoroutine ("Fading");
		StopCoroutine("Unloading");
		StartCoroutine("Loading");
	}

	public void ContinueLoading(){
		if(completion >= 1) return;
		StopCoroutine("Unloading");
		StopCoroutine("Loading");
		StartCoroutine("Loading");
	}

	public void PauseLoading(){
		StopCoroutine("Unloading");
		StopCoroutine("Loading");
	}

	public void UnLoad(){
		UnLoad(this.timeToUnloadFromComplete);
	}

	public void UnLoad(float timeToUnloadFromComplete){
		UnLoad(this.timeToUnloadFromComplete, 0.0f);
	}

	public void UnLoad(float timeToUnloadFromComplete, float targetCompletion){
		if(completion <= targetCompletion || completion >= 1) return;
		StopCoroutine("Unloading");
		StopCoroutine("Loading");
		StartCoroutine("Unloading", targetCompletion);
	}

	IEnumerator Loading(){
		float rate = 1 / (timeToComplete *(1-completion));
		while (completion <= 1){
			completion += Time.deltaTime * rate;
			CircleImage.fillAmount = Mathf.Max(completion, 0.001f);
			CircleImage.color = Color.Lerp(start, end, completion);
			current = Color.Lerp(start, end, completion);
			yield return null;
		}
		StartCoroutine("Fading");
	}

	IEnumerator Fading(){
		if(timeToDisappearAfterComplete > 0){
			float i = 1;
			float rate = 1/timeToDisappearAfterComplete;
			while (i > 0){
				i -= Time.deltaTime * rate;
				Color c = CircleImage.color;
				c.a = i;
				CircleImage.color = c;
				yield return null;
			}
		} else if(timeToDisappearAfterComplete == 0){
			Color c = CircleImage.color;
			c.a = 0;
			CircleImage.color = c;
		}
	}

	IEnumerator Unloading(float targetCompletion){
		if(timeToUnloadFromComplete > 0){
			float rate = 1 / (timeToUnloadFromComplete *(completion - targetCompletion));
			while (completion > targetCompletion){
				completion -= Time.deltaTime * rate;
				CircleImage.fillAmount = Mathf.Max(completion, 0.001f);
				CircleImage.color = Color.Lerp(start, end, completion);
				current = Color.Lerp(start, end, completion);
				yield return null;
			}
		} else {
			completion = targetCompletion;
			CircleImage.fillAmount = Mathf.Max(completion, 0.001f);
			CircleImage.color = Color.Lerp(start, end, completion);
			current = Color.Lerp(start, end, completion);
		}
	}
}
