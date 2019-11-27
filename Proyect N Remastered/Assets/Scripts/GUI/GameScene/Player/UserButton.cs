using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserButton : MonoBehaviour
{
	private int actorNumber;

	public void SelectUser()
	{
		actorNumber = int.Parse(GetComponentInChildren<Text>().text);
		UsersPanelGUIController.Instance.SelectUser(actorNumber);
	}
}
