using UnityEngine;
using System.Collections;

public class TransferOwnership : Photon.MonoBehaviour {

	public void OnOwnershipRequest(object[] viewAndPlayer)
	{
		PhotonView view = viewAndPlayer[0] as PhotonView;
		PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
		
		Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");

		view.TransferOwnership(requestingPlayer.ID);

	}
}
