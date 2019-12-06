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

    public TextMeshProUGUI gmFeedbackText;
    public TextMeshProUGUI playerFeedbackText;

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
			gmFeedbackText.text = "Trying to connect...";
			playerFeedbackText.text = "Trying to connect...";
		}
		else
		{
			gmFeedbackText.text = "Intenando conectarse...";
			playerFeedbackText.text = "Intenando conectarse...";
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
					gmFeedbackText.text = "Connection to server failed, try again";
				}
				else
				{
					gmFeedbackText.text = "Ha fallado la conexión, inténtalo otra vez";
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
        gmFeedbackText.text = "You have been connected";
        PhotonNetwork.JoinLobby(null);
        SceneManager.LoadScene("FinalRoomScene");
    }
}
