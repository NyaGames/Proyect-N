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
    public RoomSceneGUIController screenSceneGUIController;
    private List<RoomInfo> roomsAvaiable = new List<RoomInfo>();

    public TextMeshProUGUI feedbackText;

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
        feedbackText.text = "Creating room...";

        string roomName = "90";//GenerateUniqueRoomID();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)screenSceneGUIController.maxPlayers.GetComponentInChildren<Slider>().value;
        roomOptions.IsVisible = false; //FALSE = Hace la sala privada
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        Debug.Log("Sala creada: " + roomName);
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

        PhotonNetwork.NickName = screenSceneGUIController.usernameInput.text;
    }

    public override void OnCreatedRoom()
    {
        feedbackText.text = "Room created!";
        SceneManager.LoadScene("LobbyScene");
    }

    public void JoinRoom()
    {
        feedbackText.text = "Joining room...";

        string roomPassword = "90";//roompasswordInputText.text;

        if (roomPassword != "")
        {
            PhotonNetwork.JoinRoom(roomPassword);
        }
        else
        {
            Debug.Log("Introduce el código de la sala a la que quieres unirte");
        }

        PhotonNetwork.NickName = screenSceneGUIController.usernameInput.text;
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
                feedbackText.text = "Username already in use, please use other one";
                return;
            }
        }

        feedbackText.text = "You have joined Room: " + PhotonNetwork.CurrentRoom.Name;
        SceneManager.LoadScene("LobbyScene");

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "Room doesnt exists or wrong room code";
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }
}