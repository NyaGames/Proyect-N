﻿using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isGameMaster;
    public int id { get; set; }
    public PhotonView photonViewTransform;

    private BoxCollider playerCollider;

    private bool newZoneCreated;

    void Start()
    {

        playerCollider = GetComponent<BoxCollider>();
        //Variables
        if (!photonViewTransform.IsMine) //Si no soy yo, cojo los datos del servidor
        {
            Photon.Realtime.Player playerReference = getPlayerReference(photonViewTransform.OwnerActorNr); 
            id = (int)playerReference.CustomProperties["id"];
            isGameMaster = (bool)playerReference.CustomProperties["isGameMaster"];

            if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["isGameMaster"]) //Si el cliente NO es game master, desactiva a los demás
            {
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }

        }
        else //Si soy yo,cojo los datos de mi escena
        {
            isGameMaster = PersistentData.isGM;
            id = PhotonNetwork.LocalPlayer.ActorNumber;
            if (!isGameMaster) //Si no soy gameMaster,desactivo los controles para crear la zona
            {
                
            }
        }

        //Visual
        Color randomColor = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        this.GetComponent<Renderer>().material.color = randomColor;
        
    }

    public Photon.Realtime.Player getPlayerReference(int actorNumber){
        for(int i = 0;i  < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].ActorNumber == actorNumber)
            {
                return PhotonNetwork.PlayerList[i];
            }
        }
        return null;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Drop") && !isGameMaster) //Si un jugador normla choca con un drop
        {
            PickDrop(collision.gameObject);
            
        }
    }

    public void PickDrop(GameObject drop)
    {
        Destroy(drop);
        Debug.Log("Cogiste un drop!");
    }

    void Update()
    {
       /* if (!isGameMaster)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 position = this.transform.position;
                position.x--;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 position = this.transform.position;
                position.x++;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 position = this.transform.position;
                position.z++;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 position = this.transform.position;
                position.z--;
                this.transform.position = position;
            }
        }
        else
        {

            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                this.transform.position = new Vector3( hit.point.x,0,hit.point.z);
        
            }
        }*/

    }

    public void EnterGameMasterMode(){
        isGameMaster = GameObject.Find("GameMasterToggle").GetComponent<Toggle>().isOn;
        Debug.Log("Gamemaster = " + isGameMaster);
    }

}
