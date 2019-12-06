using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbySceneGUIController : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerCount;
    public Button startGameButton;

    public void Start()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (!PersistentData.isGM)
        {
            startGameButton.interactable = false;
        }
    }

    public void Update()
    {
        if (PhotonNetwork.CurrentRoom != null) 
        {
            playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    public void StartNewGame()
    {
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("GameStarted",true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(table);

        PhotonNetwork.LoadLevel("FinalGameScene");
    }

}
