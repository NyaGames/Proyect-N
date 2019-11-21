using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSceneGUIController : MonoBehaviour
{
    public TMPro.TMP_InputField roomName;
    public Button loginRoomButton;
    public Button createRoomButton;
    public GameObject maxPlayers;

    private void Awake()
    {
        if (PersistentData.account.isGameMaster)
        {
            loginRoomButton.interactable = false;
            createRoomButton.interactable = true;
        }
        else
        {
            loginRoomButton.interactable = true;
            createRoomButton.interactable = false;

            maxPlayers.SetActive(false);
        }
    }
}
