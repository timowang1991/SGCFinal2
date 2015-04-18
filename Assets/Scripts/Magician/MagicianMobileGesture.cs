using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;

public class MagicianMobileGesture : MonoBehaviour {
	private RuntimePlatform platform;

	private List<Point> points = new List<Point>();
	private List<Gesture> trainingSet = new List<Gesture>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;


	public bool checkNow = false;


	public float throwRate = 0.5f;
	private float nextThrow = 1.0f;
	private Rect drawArea;

	public GameObject controlledMagician;
	// Use this for initialization
	void Start () {
		platform = Application.platform;

		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");//just X(FireBall) and H(HPadd)
		foreach (TextAsset gestureXml in gesturesXml)
		{
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		}


		drawArea = new Rect(Screen.width - Screen.width* 3 / 4, Screen.height-Screen.height* 3 / 4, Screen.width - Screen.width / 4, Screen.height-Screen.height/4);
	}
	
	// Update is called once per frame
	void Update () {
		virtualKeyPosition = new Vector3(0,0,0);
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		}else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}
		if (drawArea.Contains(virtualKeyPosition)) {
			Debug.Log("drawArea");
			nextThrow = Time.time + throwRate;
			if(Input.GetMouseButtonDown(0))
			{
				checkNow = true;
				++strokeId;
			}

			if (Input.GetMouseButton(0)) {
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
			}
		}

		if(Time.time > nextThrow && checkNow == true)
		{
			checkNow = false;
			Gesture candidate = new Gesture(points.ToArray());
			Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
			Debug.Log(gestureResult.GestureClass);
			if(gestureResult.GestureClass == "H" && gestureResult.Score > 0.5f)
			{
				controlledMagician.GetComponent<MagicianRecoverHP>().CastSpell();
			}
			else if(gestureResult.GestureClass == "X" && gestureResult.Score > 0.5f)
			{
				controlledMagician.GetComponent<MagicianThrowFireBall>().CastSpell();
			}

			strokeId = -1;
			points.Clear();
		}
	}
}
