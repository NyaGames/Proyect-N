using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCounter : MonoBehaviour
{
	private TextMeshProUGUI counter;

	private void Awake()
	{
		counter = GetComponent<TextMeshProUGUI>();

		InvokeRepeating("UpdateText", 0f, 1f);

	}

	private void UpdateText()
	{
		counter.text = (Photon.Pun.PhotonNetwork.CurrentRoom.PlayerCount).ToString();
	}
}
