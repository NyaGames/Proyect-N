using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isGameMaster;
    public int id { get; set; }
    public PhotonView photonViewTransform;
    public PhotonView photonViewInfo;

    public Button createZoneButton;
    public Slider zoneRadiusSlider;
    public Dropdown zonePosDropdown;

    private BoxCollider playerCollider;

    private bool newZoneCreated;

    void Start()
    {
        newZoneCreated = false;

        playerCollider = GetComponent<BoxCollider>();

        createZoneButton = GameObject.Find("CreateZoneButton").GetComponent<Button>();
        zoneRadiusSlider = GameObject.Find("RadiusSelector").GetComponent<Slider>();
        zonePosDropdown = GameObject.Find("PosSelector").GetComponent<Dropdown>();

        //photonViewTransform = GetComponent<PhotonView>();
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
            isGameMaster = GameObject.Find("GameMasterToggle").GetComponent<Toggle>().isOn;
            id = PhotonNetwork.LocalPlayer.ActorNumber;
            if (!isGameMaster) //Si no soy gameMaster,desactivo los controles para crear la zona
            {
                createZoneButton.interactable = false;
                zoneRadiusSlider.interactable = false;
                zonePosDropdown.interactable = false;
            }
        }

        //Visual
        Color randomColor = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        this.GetComponent<Renderer>().material.color = randomColor;
        this.GetComponentInChildren<TextMesh>().text = id.ToString() + "/" + isGameMaster.ToString();
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

    public void PickDrop()
    {

    }

    void Update()
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

    public void EnterGameMasterMode(){
        isGameMaster = GameObject.Find("GameMasterToggle").GetComponent<Toggle>().isOn;
        Debug.Log("Gamemaster = " + isGameMaster);
    }

}
