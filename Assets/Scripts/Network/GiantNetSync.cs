using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ExitGames.Client.Photon;

//dont forget to add to observed component in the photonView
using System;


public class GiantNetSync : Photon.MonoBehaviour //enable it to access GameObject's PhotonView
{
	private string[] bodyPartNames = new string[]{"Clavicle","Arm","Forearm"};
	private const int numNonDuplicateBodyParts = 0; //prefix not left or right
	private int totalNumBodyParts;
	//public BigLittleGameLogic gameLogic;//Need Identify

	private Platform platform;
	
	[HideInInspector]
	public Transform[] bodyPartTransforms = null;
	[HideInInspector]
	public Vector3[] correctBodyPartPositions = null;
	[HideInInspector]
	public Quaternion[] correctBodyPartRotations = null;

	//bool startToUpdate;
	/// <summary>
	/// Calculate how many parts to sync and 
	/// </summary>
	void Awake() {
		//startToUpdate = false;
		int numBodyPartNames = bodyPartNames.Length;
		totalNumBodyParts = (numBodyPartNames - numNonDuplicateBodyParts) * 2 + numNonDuplicateBodyParts;
		bodyPartTransforms = new Transform[totalNumBodyParts];
		correctBodyPartPositions = new Vector3[totalNumBodyParts];
		correctBodyPartRotations = new Quaternion[totalNumBodyParts];
		int numCurrentlyCollectedBodyParts = 0;
		//get all children
		Transform[] allDescendants = gameObject.GetComponentsInChildren<Transform>();
		foreach(Transform childTransform in allDescendants) {
			//Debug.Log(childTransform.name);
			for(int i = 0;i < numBodyPartNames;i++) {
				//if it contain in the bodyPartNames, then add it to array.
				if(childTransform.name.Contains(bodyPartNames[i])) {
					Debug.Log ( i + " - getBodyParts In GiantNetSync:" + childTransform.name);
					bodyPartTransforms[numCurrentlyCollectedBodyParts] = childTransform;
					correctBodyPartPositions[numCurrentlyCollectedBodyParts] = childTransform.position;
					correctBodyPartRotations[numCurrentlyCollectedBodyParts] = childTransform.rotation;
					numCurrentlyCollectedBodyParts++;
					if(numCurrentlyCollectedBodyParts == totalNumBodyParts) {
						return;
					}
					break;
				}
			}
		}

	}
	/// <summary>
	/// platform
	/// </summary>
	void Start() {
		platform = GameObject.Find("PlatformManager").GetComponent<PlatformIndicator>().platform;
	}

	
	// Update is called once per frame
	/// <summary>
	/// if player is not Giant, then get the data from received array and lerp it to correct position.
	/// </summary>
	void Update()
	{
		if (platform != Platform.PC_Giant)//Need Identify
		{
			//Debug.Log ("Giant is not mine");
			for(int i = 0;i < totalNumBodyParts;i++) {
				bodyPartTransforms[i].position = Vector3.Lerp (bodyPartTransforms[i].position,correctBodyPartPositions[i],Time.deltaTime * 5);
				bodyPartTransforms[i].rotation = Quaternion.Lerp (bodyPartTransforms[i].rotation,correctBodyPartRotations[i],Time.deltaTime * 5);
			}
		}
	}
	
	//Called every 'network-update' when observed by PhotonView.
	/// <summary>
	/// Using isWriting to determine is sending data or receiving data.
	/// </summary>
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			for(int i = 0;i < totalNumBodyParts;i++) {
				Debug.Log("bodypart:"+bodyPartTransforms[i].position);
				stream.SendNext(bodyPartTransforms[i].position);
				stream.SendNext(bodyPartTransforms[i].rotation);
			}
		}
		else
		{
			Debug.Log("reading");
			for(int i = 0;i < totalNumBodyParts;i++) {
				correctBodyPartPositions[i] = (Vector3)stream.ReceiveNext();
				correctBodyPartRotations[i] = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
