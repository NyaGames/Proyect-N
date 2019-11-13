
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
    public Button joinRandomButton;
    public Toggle gameMasterToggle;
    public Text Log;
    public Text playerCount;
    public int playersCount;

    public GameObject playerPrefab;
    public List<GameObject> playersList = new List<GameObject>();
    public byte maxPlayersPerRoom = 4;

    

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
        Log.text += "\nConnected to server";
        connectButton.interactable = false;
        joinRandomButton.interactable = true;
    }

    public void JoinRandom()
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

    }
    public void JoinExistingRoom()
    {
        string roomName = GameObject.Find("JoinSpecificTextInput").GetComponent<TMP_InputField>().text;

        if (roomName != "")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false; //Hace la sala privada
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
        }
    }
    public override void OnJoinedRoom()
    {
        Log.text += "\nJoined";
        joinRandomButton.interactable = false;
       PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        
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
