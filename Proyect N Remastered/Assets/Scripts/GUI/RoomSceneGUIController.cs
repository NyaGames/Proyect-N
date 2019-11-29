using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSceneGUIController : MonoBehaviour
{
    public TMPro.TMP_InputField roomName;
    public Button loginRoomButton;
    public Button createRoomButton;
    public GameObject maxPlayers;
    public TMP_InputField usernameInput;

    private void Awake()
    {
        if (PersistentData.isGM)
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

    public void Update()
    {
        if(usernameInput.text != "")
        {
            if (PersistentData.isGM)
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
        else
        {
            loginRoomButton.interactable = false;
            createRoomButton.interactable = false;
        }
    }

}
