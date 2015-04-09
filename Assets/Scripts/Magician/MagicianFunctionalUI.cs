using UnityEngine;
using System.Collections;

public class MagicianFunctionalUI : MonoBehaviour {

	public enum MagicianFunctionalType
	{
		Tornado,
		Fireball,
		HealthOthers,
	}

	public int MainplayerID;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = PhotonView.Find (MainplayerID).gameObject;
	}

	public void ReceivedMessageFromUI(string name)
	{
		switch(name)
		{
		case "Tornado":
			BtnPressed(MagicianFunctionalType.Tornado);
			break;
		case "FireBall":
			BtnPressed(MagicianFunctionalType.Fireball);
			break;
		case "HealthOthers":
			BtnPressed(MagicianFunctionalType.HealthOthers);
			break;
		}
	}

	void BtnPressed(MagicianFunctionalType type)
	{
		switch(type)
		{
		case MagicianFunctionalType.Tornado:
			player.GetComponent<MagicianThrowTornado>().CastSpell();
			break;
		case MagicianFunctionalType.HealthOthers:
			player.GetComponent<MagicianRecoverHP>().CastSpell();
			break;
		case MagicianFunctionalType.Fireball:
			player.GetComponent<MagicianThrowFireBall>().CastSpell();
			break;
		}
	}
}
