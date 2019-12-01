using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isGameMaster;
    public int id { get; set; }
    public string nickName;
    public PhotonView photonViewTransform;

    private BoxCollider playerCollider;

    private void Awake()
    {
        //Debug.Log("AHORA SE CREA EL PLAYER");
    }

    void Start()
    {
        playerCollider = GetComponent<BoxCollider>();
        //Variables
        if (!photonViewTransform.IsMine) //Si no soy yo, cojo los datos del servidor
        {
            Photon.Realtime.Player playerReference = getPlayerReference(photonViewTransform.OwnerActorNr); 
            id = (int)playerReference.CustomProperties["id"];
            isGameMaster = (bool)playerReference.CustomProperties["isGameMaster"];
            nickName = gameObject.GetPhotonView().Controller.NickName;

            if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["isGameMaster"]) //Si el cliente NO es game master, desactiva a los demás
            {
               // if (isGameMaster)
                //{
                 //   gameObject.SetActive(false);
               // }
               // else
               // {
                    Destroy(gameObject);
               // }
                
            }

        }
        else //Si soy yo,cojo los datos de mi escena
        {
            nickName = PhotonNetwork.NickName;
            isGameMaster = PersistentData.isGM;
            id = PhotonNetwork.LocalPlayer.ActorNumber;          
			InitCamera();
		}

        //Visual
        Color playerColor;
        if (isGameMaster)
        {
            playerColor = new Color(0f, 255f, 0f);
        }
        else
        {
            playerColor = new Color(0f, 0f, 255f);
        }
       
        this.GetComponent<Renderer>().material.color = playerColor;			           
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
        //Drop collision
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

	private void InitCamera()
	{
		if (isGameMaster)
		{
			Camera.main.GetComponentInParent<CameraController>().UnlockCamera();
			Camera.main.GetComponentInParent<FreeCameraMovement>().gameMaster = transform;
		}
		else
		{
			Camera.main.GetComponentInParent<CameraController>().LockCameraToPlayer(transform);
		}
	}


}
