using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ExitGames.Client.Photon;

//dont forget to add to observed component in the photonView
using System;


public class GiantNetSync : Photon.MonoBehaviour //enable it to access GameObject's PhotonView
{
	private string[] bodyPartNames = new string[]{"Hips","Shoulder","Arm","ForeArm"};
	private const int numNonDuplicateBodyParts = 1; //prefix not left or right
	private int totalNumBodyParts;
	public BigLittleGameLogic gameLogic;

	[HideInInspector]
	public Transform[] bodyPartTransforms = null;
	[HideInInspector]
	public Vector3[] correctBodyPartPositions = null;
	[HideInInspector]
	public Quaternion[] correctBodyPartRotations = null;

	//bool startToUpdate;

	void Awake() {
		//startToUpdate = false;
		int numBodyPartNames = bodyPartNames.Length;
		totalNumBodyParts = (numBodyPartNames - numNonDuplicateBodyParts) * 2 + numNonDuplicateBodyParts;
		bodyPartTransforms = new Transform[totalNumBodyParts];
		correctBodyPartPositions = new Vector3[totalNumBodyParts];
		correctBodyPartRotations = new Quaternion[totalNumBodyParts];
		int numCurrentlyCollectedBodyParts = 0;
		Transform[] allDescendants = gameObject.GetComponentsInChildren<Transform>();
		foreach(Transform childTransform in allDescendants) {
			//Debug.Log(childTransform.name);
			for(int i = 0;i < numBodyPartNames;i++) {
				if(childTransform.name.Contains(bodyPartNames[i])) {
					Debug.Log ("getBodyParts In GiantNetSync:" + childTransform.name);
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
	
	// Update is called once per frame
	void Update()
	{
		Debug.Log (gameLogic.giantID);
		if (!photonView.isMine && PhotonNetwork.player.ID == gameLogic.giantID)
		{
			Debug.Log ("Giant is not mine");
			for(int i = 0;i < totalNumBodyParts;i++) {
				bodyPartTransforms[i].position = Vector3.Lerp (bodyPartTransforms[i].position,correctBodyPartPositions[i],Time.deltaTime * 5);
				bodyPartTransforms[i].rotation = Quaternion.Lerp (bodyPartTransforms[i].rotation,correctBodyPartRotations[i],Time.deltaTime * 5);
			}
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			for(int i = 0;i < totalNumBodyParts;i++) {
				stream.SendNext(bodyPartTransforms[i].position);
				stream.SendNext(bodyPartTransforms[i].rotation);
			}
		}
		else
		{
			for(int i = 0;i < totalNumBodyParts;i++) {
				correctBodyPartPositions[i] = (Vector3)stream.ReceiveNext();
				correctBodyPartRotations[i] = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
