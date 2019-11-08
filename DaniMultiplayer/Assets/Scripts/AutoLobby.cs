
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

    private GameObject actualZone;
    public GameObject zonePrefab;
    private List<GameObject> dropList = new List<GameObject>();
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
       PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        
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



    /*[PunRPC]
    public void NewObjectReceived(Vector3[] objectArray, Vector3 scale, ObjectsToSpawn objectToSpawn)
    {
        switch (objectToSpawn)
        {
            case ObjectsToSpawn.Zone: //Ha llegado una zona nueva
                if (actualZone != null)
                {
                    Destroy(actualZone);
                }
                actualZone = Instantiate(zonePrefab, objectArray[0], Quaternion.identity);
                actualZone.transform.localScale = scale;
                break;

            case ObjectsToSpawn.Drop: //Han llegado nuevos drops
                if (dropList != null)
                {
                    foreach (GameObject i in dropList)
                    {
                        Destroy(i);
                    }
                }
                foreach(Vector3 v in objectArray)
                {
                    GameObject newDrop = Instantiate(dropPrefab, v, Quaternion.identity);
                    newDrop.transform.localScale = scale;
                    dropList.Add(newDrop);
                }
               
                break;
        }

    }*/
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
                    actualZone = Instantiate(zonePrefab, zoneCenter, Quaternion.identity);
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
                    if (actualZone != null)
                    {
                        zoneCreated = true;
                        actualZone.GetComponent<Zone>().creatingObject = false;
                    }
                    break;
            }
        }
        else //Si no se detecta bien el input del dedo
        {
            
        }
        
    }
    public void SendZoneToOtherPlayers()
    {
        /*Vector3[] zoneArray = new Vector3[] { actualZone.transform.position};
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("NewObjectReceived", RpcTarget.Others, zoneArray, actualZone.transform.localScale, ObjectsToSpawn.Zone);*/
        GameObject newZone = PhotonNetwork.Instantiate(dropPrefab.name, actualZone.transform.position, Quaternion.identity);
        newZone.transform.localScale = actualZone.transform.localScale;

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
                    GameObject newDrop = Instantiate(dropPrefab, worldPosition,Quaternion.identity);
                    newDrop.GetComponent<Drop>().creatingObject = false;
                    dropList.Add(newDrop);
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
    public void SendDropsToOtherPlayers()
    {
        foreach (GameObject g in dropList)
        {
            GameObject n = PhotonNetwork.Instantiate(dropPrefab.name,g.transform.position,Quaternion.identity);
            n.transform.localScale = g.transform.localScale;
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
    public float getDistanceFromCameraToGround()
    {
        return (GameObject.Find("Plane").transform.position - GameObject.Find("Main Camera").transform.position).magnitude;
    }

}
