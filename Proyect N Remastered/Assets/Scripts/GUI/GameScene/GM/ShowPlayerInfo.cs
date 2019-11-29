using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ShowPlayerInfo : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI username;
	[SerializeField] private TextMeshProUGUI playersKilled;
	[SerializeField] private TextMeshProUGUI ammo;
	[SerializeField] private TextMeshProUGUI ozt;

	[SerializeField] KillUserPanel killConfirmationPanel;

	Photon.Realtime.Player player;

	public void ShowUserInfo(int actorNumber)
	{
		gameObject.SetActive(true);

		Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;	

		for (int i = 0; i < players.Length; i++)
		{
			if(players[i].ActorNumber == actorNumber)
			{
				player = players[i];
				username.text = player.NickName;
			}
		}		
	}

	public void AskForKillConfirmation()
	{
		killConfirmationPanel.AskForConfirmation(player);
	}

	private void OnDisable()
	{
		killConfirmationPanel.gameObject.SetActive(false);
	}
}
