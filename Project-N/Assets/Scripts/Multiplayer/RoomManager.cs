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
    public RoomSceneGUIController screenSceneGUIController;
    private List<RoomInfo> roomsAvaiable = new List<RoomInfo>();

    /*public override void OnRoomListUpdate(List<RoomInfo> roomList)
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

    }*/

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
        string roomName = "23";//GenerateUniqueRoomID();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)screenSceneGUIController.maxPlayers.GetComponentInChildren<Slider>().value;
        roomOptions.IsVisible = false; //FALSE = Hace la sala privada
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        Debug.Log("Sala creada: " + roomName);
    }

    public override void OnCreatedRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void JoinRoom()
    {
        string roomPassword = "23";//roompasswordInputText.text;

        if (roomPassword != "")
        {
            PhotonNetwork.JoinRoom(roomPassword);
        }
        else
        {
            Debug.Log("Introduce el código de la sala a la que quieres unirte");
        }

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Te has unido a la sala " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("LobbyScene");
        //myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }

}