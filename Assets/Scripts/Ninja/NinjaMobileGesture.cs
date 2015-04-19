using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;

public class NinjaMobileGesture : MonoBehaviour {
	private RuntimePlatform platform;
	
	private List<Point> points = new List<Point>();
	private List<Gesture> trainingSet = new List<Gesture>();
	
	private GameObject leftJoystick;
	private GameObject rightJoystick;
	
	private int strokeId = -1;
	
	public Vector3 virtualKeyPosition = Vector2.zero;
	
	
	public bool checkNow = false;
	
	
	public float throwRate = 1.0f;
	private float nextThrow = 1.0f;
	public Rect drawArea;
	
	public GameObject controlledNinja;
	// Use this for initialization
	void Start () {

		RuntimePlatform currplatform = Application.platform;

		if(currplatform == RuntimePlatform.Android || currplatform == RuntimePlatform.IPhonePlayer)
		{
			this.enabled = false;
		}


		platform = Application.platform;
		
		leftJoystick = GameObject.FindGameObjectWithTag ("Left_Joystick");
		rightJoystick = GameObject.FindGameObjectWithTag ("Right_Joystick");
		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");//just X(FireBall) and H(HPadd)
		foreach (TextAsset gestureXml in gesturesXml)
		{
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		}
		
		
		drawArea = new Rect(Screen.width * 1 / 5, Screen.height * 1/ 5, Screen.width - Screen.width* 2/ 5, Screen.height-Screen.height * 2/5);
	}
	
	// Update is called once per frame
	void Update () {
		if(!leftJoystick.GetComponent<CNJoystick>().currentlyTouched && !rightJoystick.GetComponent<CNJoystick>().currentlyTouched)
		{
			
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
				if(Input.GetMouseButtonUp(0))
				{
					
					
				}
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
				if(gestureResult.GestureClass == "X" && gestureResult.Score > 0.5f)
				{
					controlledNinja.GetComponent<HeroCtrl_Net2>().doAtk1Down = true;
				}
				
				strokeId = -1;
				points.Clear();
			}
		}
	}
}
