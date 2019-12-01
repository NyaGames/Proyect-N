using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoButton : MonoBehaviour
{
	[HideInInspector] public int actorNumber;

    public void ShowUser()
	{	
		GameSceneGUIController.Instance.playersInfoPanel.GetComponent<PlayersInfoGUIController>().ShowInfo(actorNumber);
	}
}
