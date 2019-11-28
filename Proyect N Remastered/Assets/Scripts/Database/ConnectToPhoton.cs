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

    public void Start()
    {
        if (PhotonNetwork.IsConnected) //Si ya estoy conectado,mantengo mi nombre de usuario
        {
            usernametext.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    public void Connect()
    {
        if(usernametext.text != "")
        {
            feedbackText.text = "Trying to connect...";
            if (!PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.ConnectUsingSettings())
                {

                }
                else
                {
                    feedbackText.text = "Connection to server failed, try again";
                }
            }
            else //Si ya estás conectado a Photon
            {
                SceneManager.LoadScene("RoomScreen");
            }
        }
        else
        {
            feedbackText.text = "Select an username before trying to connect";
        }
    }

    public override void OnConnectedToMaster()
    {
        feedbackText.text = "You have been connected";

        PhotonNetwork.LocalPlayer.NickName = usernametext.text;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(null);
        SceneManager.LoadScene("RoomScreen");
    }
}
