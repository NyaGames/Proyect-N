using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserButton : MonoBehaviour
{
    public int actorNumber;
    private string userNickname;

	public void SelectUser()
	{
		userNickname = GetComponentInChildren<TextMeshProUGUI>().text; 
		GameSceneGUIController.Instance.userPanel.GetComponent<UsersPanelGUIController>().SelectUser(userNickname);
	}
}
