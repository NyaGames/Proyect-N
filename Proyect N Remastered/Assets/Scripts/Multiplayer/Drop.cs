using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(DropAnimation))]
public class Drop : MonoBehaviour
{
    public bool movingObject;
    public bool creatingObject;

	[SerializeField] private float pickUpRange = 5f;
	[SerializeField] private GameObject rangeScaler;

	[SerializeField] private Transform player;

    private PhotonView photonView;
    [HideInInspector]public int dropAmmo;

	DropAnimation dropAnimation;
	bool pickable = false;

    [HideInInspector] public bool playerInside = false;

	private void Awake()
	{
        photonView = GetComponent<PhotonView>();
        dropAmmo = Random.Range(3,6);
        dropAnimation = GetComponent<DropAnimation>();

		if (!GameManager.Instance.myPlayer.GetComponent<Player>().isGameMaster)
		{
			pickable = true;
			player = GameManager.Instance.myPlayer.transform;
		}
	}

	private void OnValidate()
	{
		rangeScaler.transform.localScale = Vector3.one * (pickUpRange * 0.25f);
	}

	private void Start()
    {
        movingObject = false;

		rangeScaler.transform.localScale = Vector3.one * (pickUpRange * 0.25f);
	}

	private void Update()
    {

        if (!movingObject ) //Si no se mueve y no se esta creando, lo detectamos
        {
            detectObjectTapped();
        }
        else if (movingObject)  //Si puede moverse y no se esta crando, lo movemos
        {
            moveObject();
        }

		//Animaciones
		if (!pickable)
		{
			//GM
			if (dropAnimation.activated)
			{
				bool someoneInRange = false;
                GameObject[] list = GameObject.FindGameObjectsWithTag("Player"); 
                for (int i = 0; i < list.Length; i++)
				{
                    GameObject player = list[i];

                    if (!player.GetPhotonView().Owner.IsMasterClient)
                    {
                        GameManager.Instance.gmCountdown.SetActive(true);
                        GameManager.Instance.gmCountdown.GetComponentInChildren<TextMeshProUGUI>().text = "Drop: " + transform.position + " / Player LP: " + player.transform.localPosition + " y Player WP: " + player.transform.position;

                    }
                    if (Vector3.Distance(player.transform.position, transform.position) <= pickUpRange && !player.GetPhotonView().Owner.IsMasterClient)
					{
						someoneInRange = true;
					}
				}

				if (!someoneInRange)
				{
                    photonView.RPC("DeActivateToEveryone", RpcTarget.All);
                }
			}
		}
		else
		{
            GameManager.Instance.playerCountDown.SetActive(true);
            GameManager.Instance.playerCountDown.GetComponentInChildren<TextMeshProUGUI>().text = "Activado: "+ dropAnimation.activated + "Distance to drop: " + Vector3.Distance(transform.position, player.transform.position);

            //Player
            if (!dropAnimation.activated)
			{

                if (Vector3.Distance(transform.position, player.transform.position) <= pickUpRange)
				{
                    photonView.RPC("ActivateToEveryone", RpcTarget.All);
                }
			}
		}
    }

    public void detectObjectTapped()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            { 
               
                if (raycastHit.collider.gameObject == dropAnimation.model.gameObject)
                {
                    if (!GameManager.Instance.myPlayer.GetPhotonView().Owner.IsMasterClient && dropAnimation.activated)
                    {
                        PickUpDrop();
                    }
                   // movingObject = true;
                    Debug.Log("Drop tapped");
                }

            }
        }
    }
    public void moveObject()
    {
        if (Input.touchCount > 0)
        {           
            Touch touch = Input.GetTouch(0);
            float cameraDistanceToGround = GamemasterManager.Instance.getDistanceFromCameraToGround();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            worldPosition.y = 0;
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    this.gameObject.transform.position = worldPosition;
                    break;

                case TouchPhase.Ended:
                    movingObject = false;
                    break;
            }
        }
    }

	//Se está cerca de un drop y se ha pulsado, 
	public void PickUpDrop()
	{
        GameManager.Instance.myPlayer.GetComponent<AmmoInfo>().currentAmmo += dropAmmo;
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer); //Le damos el control del drop al jugador que lo pilla
        photonView.RPC("TryDestroyAnimation",RpcTarget.All);

    }	

    [PunRPC]
    public void ReceiveDropNotification()
    {
		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			GameSceneGUIController.Instance.playerMessages.AddMessage(new PlayerMessage("New Drops!", 3, 5f));
		}
		else
		{
			GameSceneGUIController.Instance.playerMessages.AddMessage(new PlayerMessage("¡Nuevos Drops!", 3, 5f));
		}
		
    }

}
