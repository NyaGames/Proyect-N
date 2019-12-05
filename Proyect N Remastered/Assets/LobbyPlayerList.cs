using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerList : MonoBehaviour
{

	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private RectTransform playersRect;

	private List<GameObject> currentPlayers = new List<GameObject>();
	private float separation = 5f;

	void Start()
    {
		InvokeRepeating("UpdatePlayers", 0f, 1f);
    }

    
    void UpdatePlayers()
    {
		RemovePlayers();

		Photon.Realtime.Player[] players = Photon.Pun.PhotonNetwork.PlayerList;

		for (int i = 0; i < players.Length; i++)
		{
			AddPlayer(players[i].NickName);
		}
	}

	void RemovePlayers()
	{
		playersRect.sizeDelta = new Vector2(playersRect.sizeDelta.x, 0);

		for (int i = 0; i < currentPlayers.Count; i++)
		{
			currentPlayers[i].Destroy();
		}	
	}


	void RemovePlayer(GameObject player)
	{
		player.Destroy();
	}

	void AddPlayer(string username)
	{		
		GameObject newPlayer = Instantiate(playerPrefab);
		newPlayer.name = username;
		newPlayer.transform.SetParent(playersRect.transform, false);
		newPlayer.GetComponentInChildren<TextMeshProUGUI>().text = username;
		currentPlayers.Add(newPlayer);

		float height = playerPrefab.GetComponent<RectTransform>().rect.width / playerPrefab.GetComponent<AspectRatioFitter>().aspectRatio;
		playersRect.sizeDelta = new Vector2(playersRect.sizeDelta.x, playersRect.sizeDelta.y + height + separation);
	}
}
