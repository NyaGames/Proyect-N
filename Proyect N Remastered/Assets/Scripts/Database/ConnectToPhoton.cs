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
		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			feedbackText.text = "Trying to connect...";
		}
		else
		{
			feedbackText.text = "Intenando conectarse...";
		}
		
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {

            }
            else
            {
				if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
				{
					feedbackText.text = "Connection to server failed, try again";
				}
				else
				{
					feedbackText.text = "Ha fallado la conexión, inténtalo otra vez";
				}
				
            }
        }
        else //Si ya estás conectado a Photon
        {
            SceneManager.LoadScene("FinalRoomScene");
        }
    }


    public override void OnConnectedToMaster()
    {
        feedbackText.text = "You have been connected";
        PhotonNetwork.JoinLobby(null);
        SceneManager.LoadScene("FinalRoomScene");
    }
}
