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

    //Photon.Realtime.Player player;
    GameObject playerGO;

	public void ShowUserInfo(int actorNumber)
	{
		gameObject.SetActive(true);

        //Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;	

        for (int i = 0; i < GamemasterManager.Instance.playersViewsList.Length; i++)
		{
			if(GamemasterManager.Instance.playersViewsList[i].GetPhotonView().CreatorActorNr == actorNumber)
			{
				playerGO = GamemasterManager.Instance.playersViewsList[i];
				username.text = playerGO.GetComponent<Player>().nickName;

                ammo.text = playerGO.GetComponent<AmmoInfo>().currentAmmo + "/" + playerGO.GetComponent<AmmoInfo>().maxAmmo;

            }
		}		
	}

	public void AskForKillConfirmation()
	{
		killConfirmationPanel.AskForConfirmation(playerGO);
	}

	private void OnDisable()
	{
		killConfirmationPanel.gameObject.SetActive(false);
	}
}
