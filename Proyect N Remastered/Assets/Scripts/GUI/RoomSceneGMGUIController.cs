using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomSceneGMGUIController : MonoBehaviour
{
	public TMP_InputField roomName;
	public Button createRoomButton;
	public GameObject maxPlayers;
	public TMP_InputField usernameInput;
	public TextMeshProUGUI feedbackText;

	private void Awake()
	{			
		createRoomButton.interactable = true;
		//maxPlayers.SetActive(true);
	}

	public void Update()
	{
		/*if (usernameInput.text != "")
		{					
			createRoomButton.interactable = true;		
			
		}*/	
	}
}
