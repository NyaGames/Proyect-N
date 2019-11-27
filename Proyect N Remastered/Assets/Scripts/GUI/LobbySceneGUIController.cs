using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneGUIController : MonoBehaviour
{
    public Text roomName;
    public Text playerCount;
    public Button startGameButton;

    public void Start()
    {
        roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        if (!PersistentData.isGM)
        {
            startGameButton.interactable = false;
        }
    }

    public void Update()
    {
        if (PhotonNetwork.CurrentRoom != null) 
        {
            playerCount.text = "Players connected: " + PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

    public void StartNewGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

}
