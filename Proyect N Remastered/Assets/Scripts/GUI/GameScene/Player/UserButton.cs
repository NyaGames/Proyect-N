using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserButton : MonoBehaviour
{
    public int actorNumber;
    private string userNickname;

	public void SelectUser()
	{
        userNickname = GetComponentInChildren<Text>().text;
		UsersPanelGUIController.Instance.SelectUser(userNickname);
	}
}
