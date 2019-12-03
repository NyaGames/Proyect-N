using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayersInfoGUIController : MonoBehaviour
{
	public static PlayersInfoGUIController Instance { get; private set; }

	[SerializeField] private float separation = 10f;
	[SerializeField] private GameObject userInfoPrefab;
	[SerializeField] private RectTransform usersRect;

	[SerializeField] private GameObject infoPanel;

	private List<GameObject> users = new List<GameObject>();

	private void OnEnable()
	{
		if (users.Count > 0)
		{
			RemoveExistingButtons();
		}

		ShowUsers();
	}

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public void ShowUsers()
	{
		Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

		for (int i = 0; i < players.Length; i++)
		{
			if (!players[i].IsMasterClient && players[i] != PhotonNetwork.LocalPlayer)
			{
				usersRect.sizeDelta = new Vector2(usersRect.sizeDelta.x, usersRect.sizeDelta.y + userInfoPrefab.GetComponent<RectTransform>().rect.height + separation);
				GameObject newButton = Instantiate(userInfoPrefab);
				newButton.transform.SetParent(usersRect.transform, false);
				newButton.GetComponentInChildren<TextMeshProUGUI>().text = players[i].NickName;
				newButton.GetComponent<UserInfoButton>().actorNumber = players[i].ActorNumber;
				users.Add(newButton);
			}
		}
	}

	private void RemoveExistingButtons()
	{
		usersRect.sizeDelta = new Vector2(usersRect.sizeDelta.x, 0);

		for (int i = 0; i < users.Count; i++)
		{
			users[i].Destroy();
		}

		users.Clear();
	}

	public void ShowInfo(int actorNumber)
	{

		infoPanel.GetComponent<ShowPlayerInfo>().ShowUserInfo(actorNumber);		
	}

	private void OnDisable()
	{
		infoPanel.SetActive(false);
	}
}
