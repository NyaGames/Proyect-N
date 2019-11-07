
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

    public GameObject playerPrefab;
    public List<GameObject> playersList = new List<GameObject>();
    public byte maxPlayersPerRoom = 4;

    private GameObject actualZone;
    public GameObject zonePrefab;
    private List<GameObject> dropList;
    public GameObject dropPrefab;

    public Button dropButton;

    Vector3 lasPositionTapped = new Vector3(0, 0, 0);
    Vector3 zoneCenter = new Vector3(0, 0, 0);

    public bool creatingDrop = false;
    public GameObject dropPos;
    public int numDrops = 3;
    public bool zoneCreated;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            zoneCreated = false;
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
        //playersList.Add(PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity));

        PhotonView photonView = PhotonView.Get(this);
        bool isGameMaster = GameObject.Find("GameMasterToggle").GetComponent<Toggle>().isOn;
        photonView.RPC("NewPlayerJoined", RpcTarget.All, isGameMaster); //Le dices a los otros clientes que has entrado en la sala
    }

    [PunRPC]
    public void NewPlayerJoined(bool isNewPlayerAGameMaster, PhotonMessageInfo info)
    {
        if (GameObject.Find("GameMasterToggle").GetComponent<Toggle>().isOn) //Si este cliente es gameMaster, instancia al jugador que ha entrado
        {
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            if (!info.Sender.IsLocal) //Si se instancia a otro jugador, se desactivan sus controles
            {
                newPlayer.GetComponent<Player>().enabled = false;
            }
            newPlayer.GetComponent<PlayerNetwork>().isGameMaster = isNewPlayerAGameMaster;
            newPlayer.GetComponent<PlayerNetwork>().id = info.Sender.ActorNumber;
            playersList.Add(newPlayer);
            
        }
        else // Si este cliente es jugador normal, no instancia a nadie
        {

        }
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
        if (!zoneCreated) //Si no se ha creado la zona todavía y no estoy creando un drop, se puede crear
        {
            CreateZone();
        }
        

        if (creatingDrop) //Si el botón de los drops está holdeado, se puede crear un drop
        { 
            CreateDrop();
        }
        

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

    public float getDistanceFromCameraToGround()
    {
        return (GameObject.Find("Plane").transform.position - GameObject.Find("Main Camera").transform.position).magnitude;
    }

    public void CreateZone()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float cameraDistanceToGround = getDistanceFromCameraToGround();
            Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    lasPositionTapped = touch.position;
                    zoneCenter = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
                    actualZone = PhotonNetwork.Instantiate(zonePrefab.name, zoneCenter, Quaternion.identity);
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if(actualZone != null)
                    {
                        float distanceFromCenterToTap = (zoneCenter - touchInWorldCoord).magnitude;
                        actualZone.transform.localScale = new Vector3(distanceFromCenterToTap * 2, distanceFromCenterToTap * 2, distanceFromCenterToTap * 2);
                    }

                    lasPositionTapped = touch.position;

                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    zoneCreated = true;
                    actualZone.GetComponent<Zone>().creatingObject = false;
                    //directionChosen = true;
                    break;
            }
        }
        
    }
    public void enableCreateNewRadious()
    {
        if (actualZone != null)
        {
            if (actualZone.GetComponent<Zone>().creatingNewRadious)
            {
                actualZone.GetComponent<Zone>().creatingNewRadious = false;
            }
            else
            {
                actualZone.GetComponent<Zone>().creatingNewRadious = true;
            }
        }

    }
    public void CreateDrop()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float cameraDistanceToGround = getDistanceFromCameraToGround();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            worldPosition.y = 0;
            switch (touch.phase)
            {
                case TouchPhase.Moved:

                    dropPos.transform.position = worldPosition;

                    break;

                case TouchPhase.Ended:
                    Destroy(dropPos);
                    numDrops--;
                    creatingDrop = false;
                    GameObject newDrop = PhotonNetwork.Instantiate(dropPrefab.name, worldPosition,Quaternion.identity);
                    newDrop.GetComponent<Drop>().creatingObject = false;
                    break;
            }
        }
        else
        {
            //Si no se detecta bien el input del dedo, se borra la imagen del drop, ese intento de crear el drop no cuenta
            if(dropPos != null)
            {
                Destroy(dropPos);
            }
        }
    }


}
