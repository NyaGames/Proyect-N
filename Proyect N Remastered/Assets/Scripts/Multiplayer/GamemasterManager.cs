using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GamemasterManager : MonoBehaviourPunCallbacks
{
    public static GamemasterManager Instance { get; private set; }

	public GameObject zonePrefab;
    public GameObject zoneCantGetOutPrefab;
	public GameObject dropPrefab;
    public GameObject mobilePrefab;

	public bool creatingDrop = false;

    [HideInInspector]public GameObject staticZone;
    [HideInInspector]public GameObject newZonePosition;
    [HideInInspector]public GameObject provZone;

	private List<GameObject> provDropList = new List<GameObject>();
	private List<GameObject> dropList = new List<GameObject>();

	Vector3 lasPositionTapped = new Vector3(0, 0, 0);
    Vector3 zoneCenter = new Vector3(0, 0, 0);

    [HideInInspector] public GameObject[] playersViewsList;

    private bool provZoneCreated = false;

    [HideInInspector] public int secsClosingZone;

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

    public void Start()
    {

    }

    public void Update()
    {
        if(PhotonNetwork.CurrentRoom != null && playersViewsList.Length != PhotonNetwork.CurrentRoom.PlayerCount)
        {
            playersViewsList = GameObject.FindGameObjectsWithTag("Player");
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ZoneManager.Instance.isEditingZone) //Si no se ha creado la zona todavía y estoy dentro de una sala,puedo crearla
        {
            HandleZoneInput();
        }


        if (ZoneManager.Instance.isEditingDrops) //Si el botón de los drops está holdeado, se puede crear un drop
        {
            CreateDrop();
        }
    } 

	#region Zone
    public void HandleZoneInput()
    {
        if (Input.touchCount > 0)
        {
			Touch touch = Input.GetTouch(0);

			Vector2 posNormalized = touch.position / Screen.width;
			if (posNormalized.x > 0.66f) return;         
           
            switch (Input.touchCount)
            {
                case 1:
                    if (!provZoneCreated)
                    {
                        CreateProvZone(touch);
                    }
                    else
                    {
                        MoveProvZone(touch);
                    }
                    break;
                case 2:
                    ChangeProvZoneScale(Input.GetTouch(0), Input.GetTouch(1));
                    break;
                default:
                    break;
            }
        
        }    

    }

    private void CreateProvZone(Touch touch)
    {
        float cameraDistanceToGround = getDistanceFromCameraToGround();
        Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround - 0.5f));

        switch (touch.phase)
        {
            // Record initial touch position.
            case TouchPhase.Began:
                if (provZone == null || provZone.activeSelf == false)
                {
                    if (provZone == null)
                    {                      
                        provZone = Instantiate(zonePrefab, touchInWorldCoord, Quaternion.identity);
                    }
                    else
                    {
                        provZone.SetActive(true);
                        provZone.transform.localScale = Vector3.one * 5;
                        provZone.transform.position = touchInWorldCoord;
                    }

					GameSceneGUIController.Instance.gmHelp.SetMessage("Move to scale the zone");
				}
                break;

            // Determine direction by comparing the current touch position with the initial one.
            case TouchPhase.Moved:
                if (provZone.activeSelf == true)
                {
                    float distanceFromCenterToTap = (provZone.transform.position - touchInWorldCoord).magnitude;
                    provZone.transform.localScale = new Vector3(distanceFromCenterToTap * 2, 5f, distanceFromCenterToTap * 2);
                }           

                break;

            // Report that a direction has been chosen when the finger is lifted.
            case TouchPhase.Ended:
                if (provZone.activeSelf == true)
                {
                    provZone.GetComponent<Zone>().creatingObject = false;
                }

				GameSceneGUIController.Instance.gmHelp.SetMessage("Try moving and scaling!");

				provZoneCreated = true;
                break;
        }
    }

    private void MoveProvZone(Touch touch)
    {
        if (touch.phase == TouchPhase.Moved)
        {
            float cameraDistanceToGround = getDistanceFromCameraToGround();
            Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));

            provZone.transform.position = new Vector3(touchInWorldCoord.x, provZone.transform.position.y, touchInWorldCoord.z);
        }
    }

    private void ChangeProvZoneScale(Touch firstTouch, Touch secondTouch)
    {
        Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

        float zoomSpeed = 0.5f;

        if (firstTouch.position.x / Screen.width < 0.66 && secondTouch.position.x / Screen.width < 0.66)
        {
            float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            float scaleDelta = (currentMagnitude - prevMagnitude) * zoomSpeed;

            provZone.transform.localScale = new Vector3(provZone.transform.localScale.x + scaleDelta, provZone.transform.localScale.y, provZone.transform.localScale.z + scaleDelta);

        }

    }

    public void SendZoneToOtherPlayers()
    {
        if (provZone != null || provZone.activeSelf == true) //Solo se envía una zona si se ha creado previamente en local. Si no se ha editado nada, no se puede mandar nada
        {
            if (staticZone == null) //Si no hay ninguna zona todavia, se guarda la zona editada en actualZone
            {
                staticZone = PhotonNetwork.Instantiate(zoneCantGetOutPrefab.name, provZone.transform.position, Quaternion.identity);
                staticZone.transform.localScale = provZone.transform.localScale;
                HideProvZone();
				staticZone.GetPhotonView().RPC("NewZoneReceived", RpcTarget.Others);

              
            }
            else //Si ya habia una zona en el mapa, se guarda la zona editada en nextZone
            {
                //Vector3 geoPosition = editableZone.GetComponent<ZonePositionWithLatLon>().GetGeoPosition();
                newZonePosition = PhotonNetwork.Instantiate(zonePrefab.name, provZone.transform.position, Quaternion.identity);
                newZonePosition.transform.localScale = provZone.transform.localScale;
                provZoneCreated = false;
				staticZone.GetPhotonView().RPC("NewZoneReceived", RpcTarget.Others);
			}      
        }

        foreach (GameObject g in playersViewsList)
        {
            PhotonView p = g.GetComponent<PhotonView>();
            p.RPC("ActivateCountdownText", RpcTarget.All);
            p.RPC("ResetCurrentSecsOutOfZone", RpcTarget.All);
        }

    }

    public void CreateNewPosZone()
    {
        if (newZonePosition == null)
        {
            newZonePosition = Instantiate(zonePrefab);
        }

        newZonePosition.transform.position = provZone.transform.position;
        newZonePosition.transform.localScale = provZone.transform.localScale;

        HideProvZone();
    }

    public void StartClosingZone()
    {   
        StartCoroutine(CloseActualZone());
		GameSceneGUIController.Instance.playerMessages.AddMessage(new PlayerMessage("Zone is closing!", 1, 5f));
	}

    public IEnumerator CloseActualZone()
    {
        float timeToClose = secsClosingZone;
        float t = 0;
        float currentTime = 0;
        Vector3 InitialScale = staticZone.transform.localScale;
        Vector3 FinalScale = newZonePosition.transform.localScale;
        Vector3 Initialpos = staticZone.transform.position;
        Vector3 Finalpos = newZonePosition.transform.position;

        foreach (GameObject g in playersViewsList)
        {
            g.GetComponent<PhotonView>().RPC("ActivateCountdownText", RpcTarget.All);
        }

        while (t <= 1)
        {
            currentTime += Time.deltaTime;
            foreach (GameObject g in playersViewsList)
            {
                g.GetComponent<PhotonView>().RPC("ReceiveZoneClosingCountdown", RpcTarget.All, Mathf.RoundToInt(currentTime),(int)timeToClose);
            }
            staticZone.transform.localScale = Vector3.Lerp(InitialScale, FinalScale, t);
            staticZone.transform.position = Vector3.Lerp(Initialpos, Finalpos, t);
            t += Time.deltaTime / timeToClose;
            yield return null;
        }

        foreach (GameObject g in playersViewsList)
        {
            g.GetComponent<PhotonView>().RPC("DeactivateCountdownText", RpcTarget.All);
        }

        staticZone.transform.localScale = FinalScale;
        staticZone.transform.position = Finalpos;
        PhotonNetwork.Destroy(staticZone);
        staticZone = PhotonNetwork.Instantiate(zoneCantGetOutPrefab.name, Finalpos, Quaternion.identity); //NextZone pasa a ser nuestra actualZone y borramos nextZone
        staticZone.transform.localScale = FinalScale;;

        PhotonNetwork.Destroy(newZonePosition);
        //newZonePosition.SetActive(false);
        Destroy(newZonePosition);
        //PhotonNetwork.Destroy(provZone);

    }

    public void DeleteZone()
    {
        if(provZone != null)
        {
            HideProvZone();       
        }
    }

	public void HideProvZone()
	{
        //provZone.SetActive(false);
        Destroy(provZone);
        provZoneCreated = false;
    }
	#endregion

	#region Drops
	public void CreateDrop()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

			if (touch.position.x / Screen.width > 0.66) return;
            float cameraDistanceToGround = getDistanceFromCameraToGround();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            worldPosition.y = 0;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    creatingDrop = true;
                    break;
                case TouchPhase.Moved:


                    break;

                case TouchPhase.Ended:

                    if (creatingDrop)
                    {
                        GameObject newDrop = Instantiate(dropPrefab, worldPosition, Quaternion.identity);
                        newDrop.GetComponent<Drop>().creatingObject = false;
                        provDropList.Add(newDrop);
                        creatingDrop = false;
                    }
                    
                    break;
            }

			GameSceneGUIController.Instance.gmHelp.SetMessage("To erase drops, exit edition mode");
        }
        else
        {

        }
    }
    public void SendDropsToOtherPlayers()
    {
		dropList = provDropList;

        GameObject n = null;
        foreach (GameObject g in provDropList)
        {
            n = PhotonNetwork.Instantiate(dropPrefab.name, g.transform.position, Quaternion.identity);
            n.transform.localScale = g.transform.localScale;
			n.GetComponentInChildren<MeshRenderer>().material.color = Color.red;          
        }

        n.GetPhotonView().RPC("ReceiveDropNotification", RpcTarget.Others);
        

		DeleteProvDrops();

		ZoneManager.Instance.TriggerDropsMode();

    }
    public void DeleteDrop()
    {
		GameObject lastDropTapped = provDropList[provDropList.Count - 1];
        provDropList.Remove(lastDropTapped);
        Destroy(lastDropTapped);      
    }
	#endregion

	public float getDistanceFromCameraToGround()
	{
		return (GameObject.Find("LocationBasedGame").transform.position.y + GameObject.Find("Main Camera").transform.position.y);
	}

	//Player interaction methods
	public void tapOnPlayer()
	{
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
		{
			Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out raycastHit))
			{
				Debug.Log("Something Hit");
				if (raycastHit.collider.gameObject.CompareTag("Player")) //Si es un jugador
				{
					/*lastTouchedDrop();
                    movingObject = true;
                    Debug.Log("Drop tapped");*/
				}

			}
		}
	}    

	public void DeleteProvDrops()
	{
		foreach (var gameObject in provDropList)
		{
			Destroy(gameObject);
		}

		provDropList.Clear();
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
           

            foreach (GameObject g in playersViewsList)
            {
                g.GetComponent<PhotonView>().RPC("OnKillReceived", RpcTarget.All, otherPlayer.NickName);
            }

            if (PersistentData.isGM)
            {
                PhotosNotificationsManager.Instance.PlayerDeathReceived(otherPlayer);
            }
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount <= 2 && !PhotonNetwork.LocalPlayer.IsMasterClient && !GameManager.Instance.winningPanel.activeSelf) //Si solo quedais tu y el gm, has ganado
        {
            GameManager.Instance.winningPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "You have killed " + GameManager.Instance.myPlayer.GetComponent<KillsInfo>().currentKills;
            GameManager.Instance.winningPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount - 1  + "/" + GameManager.Instance.playerCountAtStart;
            GameManager.Instance.winningPanel.SetActive(true);
           
        }

    }

}
