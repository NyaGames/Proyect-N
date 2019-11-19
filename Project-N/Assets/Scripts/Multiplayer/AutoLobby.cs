
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

    public TMP_InputField roompasswordInputText;

    private string roomPassword;
    public GameObject myPlayer;
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
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
            if (PhotonNetwork.ConnectUsingSettings())
            {
                Log.text = "\nConectado a " + PhotonNetwork.CloudRegion;
            }
            else
            {
                Log.text = "\nFailing connected to server";
            }
        }
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(null);
        Log.text = "\nConnected to server";
        connectButton.interactable = false;
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;      
    }

    //Se llama cada vez que alguien crea o borra una sala del servidor
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Has entrado en el lobby, estas son las salas que hay disponibles");
        if(roomList.Count <= 0)
        {
            roomsAvaiableText.text = "No hay ninguna sala creada";
        }
        else
        {
            UpdateCachedRoomList(roomList);
        }
       
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
        string roomName = "22";//GenerateUniqueRoomID();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayersPerRoom;
        roomOptions.IsVisible = false; //FALSE = Hace la sala privada
        PhotonNetwork.CreateRoom(roomName, roomOptions,null);
        Debug.Log("Sala creada: " + roomName);
    }
    public string GenerateUniqueRoomID()
    {
        string s = "";
        string time = System.DateTime.Now.ToString();

        string[] timeSplit = time.Split(char.Parse("/"), char.Parse(" "),char.Parse(":"));
        
        for(int i = 0;i < 3; i++)
        {
            s += timeSplit[i] + timeSplit[i+3];
        }
        return s;
    }
    public void JoinRoom()
    {
        roomPassword = roompasswordInputText.text;

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
        roomsAvaiableText.text = "Sala actual: " + PhotonNetwork.CurrentRoom.Name;

        roompasswordInputText.interactable = false;
        createRoomButton.interactable = false;
        Log.text += "\nJoined";
        GameObject g = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        if (g.GetComponent<Player>().isGameMaster)
        {
            myPlayer = g;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("La sala no existe o la contraseña es incorrecta");
    }
    private void FixedUpdate()
    {

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
