using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public RoomSceneGMGUIController roomSceneGMGUIController;
    public RoomScenePlayerGUIController roomScenePlayerGUIController;
    private List<RoomInfo> roomsAvaiable = new List<RoomInfo>();

	private void Awake()
	{
		if (PersistentData.isGM)
		{			
			roomSceneGMGUIController.gameObject.SetActive(true);
			roomScenePlayerGUIController.gameObject.SetActive(false);
		}
		else
		{
			roomScenePlayerGUIController.gameObject.SetActive(true);
			roomSceneGMGUIController.gameObject.SetActive(false);
		}
	}

	public string GenerateUniqueRoomID()
    {
        string s = "";
        string time = System.DateTime.Now.ToString();

        string[] timeSplit = time.Split(char.Parse("/"), char.Parse(" "), char.Parse(":"));

        for (int i = 0; i < 3; i++)
        {
            s += timeSplit[i] + timeSplit[i + 3];
        }
        return s;
    }
    public void CreateRoom()
    {
        roomSceneGMGUIController.feedbackText.text = "Creating room...";

        string roomName = "29";//GenerateUniqueRoomID();
        RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 20;//(byte)roomSceneGMGUIController.maxPlayers.GetComponentInChildren<Slider>().value;
        roomOptions.IsVisible = false; //FALSE = Hace la sala privada
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        Debug.Log("Sala creada: " + roomName);
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

		PhotonNetwork.NickName = "GM";//roomSceneGMGUIController.usernameInput.text;
    }

    public override void OnCreatedRoom()
    {
		roomSceneGMGUIController.feedbackText.text = "Room created!";
        SceneManager.LoadScene("FinalLobbyScene");
    }

    public void JoinRoom()
    {
		roomScenePlayerGUIController.feedbackText.text = "Joining room...";

        string roomPassword = "29";//roompasswordInputText.text;

        if (roomPassword != "")
        {
            PhotonNetwork.JoinRoom(roomPassword);
        }
        else
        {
            Debug.Log("Introduce el código de la sala a la que quieres unirte");
        }

        PhotonNetwork.NickName = roomScenePlayerGUIController.usernameInput.text;
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Unido a la sala");
        //Antes de pasar al lobby, comprobamos que nuestro username es único
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerListOthers;
        foreach (Photon.Realtime.Player p in playerList)
        {
            if (p.NickName.Equals(PhotonNetwork.LocalPlayer.NickName))
            {
                PhotonNetwork.LeaveRoom();
				roomScenePlayerGUIController.feedbackText.text = "Username already in use, please use other one";
                return;
            }
        }

		roomScenePlayerGUIController.feedbackText.text = "You have joined Room: " + PhotonNetwork.CurrentRoom.Name;
        SceneManager.LoadScene("FinalLobbyScene");

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
		roomScenePlayerGUIController.feedbackText.text = "Room doesnt exists or wrong room code";
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("FinalMainMenu");
    }
}