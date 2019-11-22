using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UsersPanelGUIController : MonoBehaviour
{
	public static UsersPanelGUIController Instance { get; private set; }

	[SerializeField] private float separation = 10f;
	[SerializeField] private GameObject userButtonPrefab;
	[SerializeField] private GameObject scrollRectObj;

	private Rect scrollRect;
	private List<GameObject> users = new List<GameObject>();

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

	private void Start()
	{
		scrollRect = scrollRectObj.GetComponent<RectTransform>().rect;
	}

	private void OnEnable()
	{
		if(users.Count > 0)
		{
			RemoveExistingButtons();
		}

		ShowUsers();
	}

	public void ShowUsers()
	{
		Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

		for (int i = 0; i < players.Length; i++)
		{
			if (!players[i].IsMasterClient && players[i] != PhotonNetwork.LocalPlayer)
			{
				scrollRect.height += userButtonPrefab.GetComponent<RectTransform>().rect.height + separation;
				GameObject newButton = Instantiate(userButtonPrefab);
				newButton.transform.SetParent(scrollRectObj.transform, false);
				newButton.GetComponentInChildren<Text>().text = players[i].ActorNumber.ToString();
				users.Add(newButton);
			}
		}
	}

	public void SelectUser(int actorSelected)
	{
		PhotosPanelGUIController.Instance.CancelPhotoToSend();
		gameObject.SetActive(false);		
		GameManager.Instance.myPlayer.GetComponent<ImageManager>().SendImageToMaster(GameSceneGUIController.Instance.sourceImage, actorSelected);
		
	}

	private void RemoveExistingButtons()
	{
		scrollRect.height = 0;

		for (int i = 0; i < users.Count; i++)
		{
			users[i].Destroy();
		}

		users.Clear();
	}
}
