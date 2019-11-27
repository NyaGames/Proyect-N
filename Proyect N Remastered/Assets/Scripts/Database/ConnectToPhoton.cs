using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToPhoton : MonoBehaviourPunCallbacks
{
    public static ConnectToPhoton Instance { get; private set; }

    public TextMeshProUGUI feedbackText;
    public TMP_InputField usernametext;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

        }
        else
        {
            Destroy(Instance);
        }
    }

    public void Connect()
    {
        feedbackText.text = "Trying to connect...";
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
               
            }
            else {
                feedbackText.text = "Connection to server failed, try again";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        feedbackText.text = "You have been connected";

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(null);
        // SceneManager.LoadScene("MainMenuScreen");
        SceneManager.LoadScene("RoomScreen");
    }
}
