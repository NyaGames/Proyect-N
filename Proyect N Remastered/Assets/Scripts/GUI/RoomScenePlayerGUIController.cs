using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomScenePlayerGUIController : MonoBehaviour
{
	public TMP_InputField roomName;
	public Button loginRoomButton;
	public TMP_InputField usernameInput;
	public TextMeshProUGUI feedbackText;

	private void Awake()
	{
		loginRoomButton.interactable = true;	
	}

	public void Update()
	{
		if (usernameInput.text != "")
		{
			
			loginRoomButton.interactable = true;
		}
		else
		{
			loginRoomButton.interactable = false;			
		}
	}
}
