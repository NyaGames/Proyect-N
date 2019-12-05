using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class UsersPanelGUIController : MonoBehaviour
{
	public static UsersPanelGUIController Instance { get; private set; }

	[SerializeField] private float separation = 10f;
	[SerializeField] private GameObject userButtonPrefab;
	[SerializeField] private RectTransform buttonsRect;

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
    
	private void OnEnable()
	{
		if(users.Count > 0)
		{
			RemoveExistingButtons();
		}

		InvokeRepeating("ShowUsers", 0f, 1f);
	}

	public void ShowUsers()
	{
		Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

		RemoveExistingButtons();
        
        for (int i = 0; i < players.Length; i++)
		{
			if (!players[i].IsMasterClient && players[i] != PhotonNetwork.LocalPlayer)
			{
               
				GameObject newButton = Instantiate(userButtonPrefab);
				newButton.transform.SetParent(buttonsRect.transform, false);        
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = players[i].NickName;
                users.Add(newButton);

				float height = buttonsRect.rect.width / newButton.GetComponent<AspectRatioFitter>().aspectRatio;
				buttonsRect.sizeDelta = new Vector2(buttonsRect.sizeDelta.x, buttonsRect.sizeDelta.y + height + separation);

			}
		}
	}

	public void SelectUser(string nicknameSelected)
	{
		//PhotosPanelGUIController.Instance.CancelPhotoToSend();
		GameSceneGUIController.Instance.photosPanel.GetComponent<PhotosPanelGUIController>().CancelPhotoToSend();
		gameObject.SetActive(false);		
		GameManager.Instance.myPlayer.GetComponent<MessageSender>().SendImageToMaster(GameSceneGUIController.Instance.sourceImage, nicknameSelected);
		
	}

	private void RemoveExistingButtons()
	{
        buttonsRect.sizeDelta = new Vector2(buttonsRect.sizeDelta.x, 0);

		for (int i = 0; i < users.Count; i++)
		{
			users[i].Destroy();
		}

		users.Clear();
	}
}
