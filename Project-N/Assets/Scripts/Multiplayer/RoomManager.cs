using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button joinRoomButton;
    public Button createRoomButton;

    public TMP_InputField roomnameInputText;
    public TMP_InputField roompasswordInputText;
    public TMP_InputField roomMaxPlayers;

    private List<RoomInfo> roomsAvaiable = new List<RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Has entrado en el lobby, estas son las salas que hay disponibles");
        UpdateCachedRoomList(roomList);
    }

    void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo r in roomList)
        {
            if (!roomsAvaiable.Contains(r)) //Si no teniamos esa sala guardada,la guardamos
            {
                ExitGames.Client.Photon.Hashtable table = r.CustomProperties;
                roomsAvaiable.Add(r);
            }
            else //Si la sala etaba guardada, la borramos
            {
                roomsAvaiable.Remove(r);
            }
        }

    }

    public void CreateRoom()
    {
        string roomName = roomnameInputText.text;
        string roomPassword = roompasswordInputText.text;

        bool usernameAlreadyUsed = false;
        foreach (RoomInfo r in roomsAvaiable)
        {
            if (r.Name == roomName)
            {
                usernameAlreadyUsed = true;
                break;
            }
        }

        if (roomName != "" && roomPassword != "" && !usernameAlreadyUsed)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = byte.Parse(roomMaxPlayers.text);
            roomOptions.IsVisible = true; //FALSE = Hace la sala privada
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
            table.Add("password", roomPassword);
            roomOptions.CustomRoomProperties = table;
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
            Debug.Log("Sala creada: " + roomName + " / " + roomPassword);
        }
        else
        {
            Debug.Log("Nombre de sala no disponible o contraseña no introducida");
        }
    }

    public void JoinRoom()
    {
        string roomName = roomnameInputText.text;
        string roomPassword = roompasswordInputText.text;

        bool passwordCorrect = false;
        //Miramos en las salas creadas la sala a la que queremos unirnos, y comprobamos si la contraseña es correcta
        foreach (RoomInfo r in roomsAvaiable)
        {
            if (r.Name == roomName)
            {
                ExitGames.Client.Photon.Hashtable table = r.CustomProperties;
                if (table.ContainsValue(roomPassword))
                {
                    passwordCorrect = true;
                    break;
                }
            }
        }
        if (roomName != "" && roomPassword != "" && passwordCorrect)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            Debug.Log("Nombre de sala o contraseña incorrectas");
        }

    }
    public override void OnJoinedRoom()
    {
        roomnameInputText.interactable = false;
        roompasswordInputText.interactable = false;
        createRoomButton.interactable = false;
        //myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }
    private void FixedUpdate()
    {

        /*playerCount.text = GameObject.Find("JoinSpecificTextInput").GetComponent<TMP_InputField>().text;
        if (PhotonNetwork.CurrentRoom != null) //Si estamos en una sala
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            playerCount.text = playersCount + "/" + maxPlayersPerRoom;
            gameMasterToggle.interactable = false;
        }
        else
        {
            gameMasterToggle.interactable = true;
            playerCount.text = "You are not in a room";
        }*/

    }
}
