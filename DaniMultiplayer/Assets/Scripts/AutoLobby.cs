
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tutoriales.multiplayer{ }

public class AutoLobby : MonoBehaviourPunCallbacks
{

    public static AutoLobby Instance { get; private set; }
    public Button connectButton;
    public Button joinRandomButton;
    public Toggle gameMasterToggle;
    public Text Log;
    public Text playerCount;
    public int playersCount;


    public Slider zoneRadiusSlider;
    public Dropdown zonePosDropdown;

    public GameObject playerPrefab;
    public List<GameObject> playersList = new List<GameObject>();
    public byte maxPlayersPerRoom = 4;

    private GameObject actualZone;
    public GameObject zonePrefab;
    private List<GameObject> dropList;
    public GameObject dropPrefab;

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
        playersList.Add(PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity)); 
    }

    public Vector3 stringToVector3(string s)
    {
        // Remove the parentheses
        if (s.StartsWith("(") && s.EndsWith(")"))
        {
            s = s.Substring(1, s.Length - 2);
        }

        // split the items
        string[] sArray = s.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
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

    public void CreateZone()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 startPosition = new Vector3(0,0,0);
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                    startPosition = worldPosition;
                    actualZone = PhotonNetwork.Instantiate(zonePrefab.name, worldPosition, Quaternion.identity);
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    float resta = Mathf.Abs(startPosition.x - touch.position.x);
                    if (touch.position.x < startPosition.x)
                    {
                        actualZone.transform.localScale -= new Vector3(resta * 0.5f, resta * 0.5f, resta*0.5f);
                    }else if (touch.position.x > startPosition.x)
                    {
                        actualZone.transform.localScale -= new Vector3(resta * 0.5f, resta * 0.5f, resta * 0.5f);
                    }
                        
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    //directionChosen = true;
                    break;
            }
        }
        
    }

    public void CreateDrop()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 startPosition = new Vector3(0, 0, 0);
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                    startPosition = worldPosition;
                    dropList.Add(PhotonNetwork.Instantiate(dropPrefab.name, worldPosition, Quaternion.identity));
                    break;
            }
        }
    }

}
