using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSceneGUIController : MonoBehaviour
{
    [SerializeField] private Button loginRoomButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private GameObject maxPlayers;

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
