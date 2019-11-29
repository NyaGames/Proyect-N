using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class KillUserPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI username;

	private int actorNumber;

	public void AskForConfirmation(GameObject user)
	{
		gameObject.SetActive(true);
		username.text = user.GetComponent<Player>().nickName;
		actorNumber = user.gameObject.GetPhotonView().CreatorActorNr;
	}

    public void KillPlayer()
	{
		gameObject.SetActive(false);
		Debug.Log("Pum, " + username.text + " matao!");
	}	
}
