using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class KillUserPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI username;

	private int actorNumber;

	public void AskForConfirmation(Photon.Realtime.Player user)
	{
		gameObject.SetActive(true);
		username.text = user.NickName;
		actorNumber = user.ActorNumber;
	}

    public void KillPlayer()
	{
		gameObject.SetActive(false);
		Debug.Log("Pum, " + username.text + " matao!");
	}	
}
