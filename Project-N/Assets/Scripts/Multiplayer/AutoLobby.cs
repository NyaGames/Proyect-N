
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tutoriales.multiplayer{ }

public enum ObjectsToSpawn
{
    Zone,Drop
}

public class AutoLobby : MonoBehaviourPunCallbacks
{

    public static AutoLobby Instance { get; private set; }
    public Button connectButton;
    public Button createRoomButton;
    public Button joinRoomButton;
    public Toggle gameMasterToggle;
    public Text Log;
    public Text playerCount;
    public int playersCount;

    public GameObject playerPrefab;
    public List<GameObject> playersList = new List<GameObject>();
    public byte maxPlayersPerRoom = 4;

    public TMP_InputField roomnameInputText;
    public TMP_InputField roompasswordInputText;

    private string roomPassword;
    private GameObject myPlayer;
    private List<RoomInfo> roomsAvaiable = new List<RoomInfo>();
    public Text roomsAvaiableText;

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
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                
            }
            else
            {
                Log.text += "\nFailing connected to server";
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(null);
        Log.text += "\nConnected to server";
        connectButton.interactable = false;
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
        
    }

    //Se llama cada vez que alguien crea o borra una sala del servidor
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Has entrado en el lobby, estas son las salas que hay disponibles");
        UpdateCachedRoomList(roomList);
    }
    void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        roomsAvaiableText.text = "";

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
        foreach(RoomInfo r in roomsAvaiable)
        {
            roomsAvaiableText.text += r.Name + "/";
        }
        
    }

    /*public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nJoined room";
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text += "\nNo rooms to join, creating one...";
        if (PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom })) {
            Log.text += "\nRoom created";
        }
        else {
            Log.text += "\nFail creating room";
        }

    }*/
    public void CreateRoom()
    {
        string roomName = roomnameInputText.text;
        roomPassword = roompasswordInputText.text;

        bool usernameAlreadyUsed = false;
        foreach(RoomInfo r in roomsAvaiable)
        {
            if(r.Name == roomName)
            {
                usernameAlreadyUsed = true;
                break;
            }
        }

        if (roomName != "" && roomPassword != "" && !usernameAlreadyUsed)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            roomOptions.IsVisible = true; //FALSE = Hace la sala privada
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
            table.Add("password", roomPassword);
            roomOptions.CustomRoomProperties = table;
            PhotonNetwork.CreateRoom(roomName, roomOptions,null);
            Debug.Log("Sala creada: " + roomName  + " / " + roomPassword);
        }
        else
        {
            Debug.Log("Nombre de sala no disponible o contraseña no introducida");
        }
    }
    public void JoinRoom()
    {
        string roomName = roomnameInputText.text;
        roomPassword = roompasswordInputText.text;

        bool passwordCorrect = false;
        //Miramos en las salas creadas la sala a la que queremos unirnos, y comprobamos si la contraseña es correcta
        foreach (RoomInfo r in roomsAvaiable)
        {
            if(r.Name == roomName)
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
        roomsAvaiableText.text = "Sala actual: " + PhotonNetwork.CurrentRoom.Name;

        roomnameInputText.interactable = false;
        roompasswordInputText.interactable = false;
        createRoomButton.interactable = false;
        Log.text += "\nJoined";
        myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }
    private void FixedUpdate()
    {

        playerCount.text = GameObject.Find("JoinSpecificTextInput").GetComponent<TMP_InputField>().text;
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
        }

    }

  

}
