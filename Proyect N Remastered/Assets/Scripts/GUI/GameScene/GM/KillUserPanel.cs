using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class KillUserPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI username;

	private GameObject playerPrefab;

	public void AskForConfirmation(GameObject user, string usernameString)
	{
		gameObject.SetActive(true);
		username.text = usernameString;
        playerPrefab = user;
	}

    public void KillPlayer()
	{
        string s = "GM killed you";
        playerPrefab.GetPhotonView().RPC("KillYourself", RpcTarget.All, new byte[0], s,WaysToKillAPlayer.GMChoice);
        gameObject.SetActive(false);
		Debug.Log("Pum, " + username.text + " matao!");
	}	
}
