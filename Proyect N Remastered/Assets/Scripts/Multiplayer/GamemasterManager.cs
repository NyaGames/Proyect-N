using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemasterManager : MonoBehaviour
{
    public static GamemasterManager Instance { get; private set; }

	public GameObject zonePrefab;
	public GameObject dropPrefab;

	public bool creatingDrop = false;
	public int numDrops = 3;

    [HideInInspector]public GameObject staticZone;
    [HideInInspector]public GameObject newZonePosition;
    [HideInInspector]public GameObject provZone;

	[SerializeField] private Material provZoneMat;
	[SerializeField] private Material staticZoneMat;
	[SerializeField] private Material newZonePositionMat;

	private List<GameObject> dropList = new List<GameObject>();

	[HideInInspector] public GameObject dropPos;
	[HideInInspector] public GameObject lastDropTapped;


	Vector3 lasPositionTapped = new Vector3(0, 0, 0);
    Vector3 zoneCenter = new Vector3(0, 0, 0);

    [HideInInspector] public GameObject[] playersViewsList;

    private bool provZoneCreated = false;

    private void OnValidate()
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
        if (ZoneManager.Instance.isEditingZone ) //Si no se ha creado la zona todavía y estoy dentro de una sala,puedo crearla
        {
            HandleZoneInput();
        }


        if (creatingDrop) //Si el botón de los drops está holdeado, se puede crear un drop
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
        Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));


        switch (touch.phase)
        {
            // Record initial touch position.
            case TouchPhase.Began:
                if (provZone == null || provZone.activeSelf == false)
                {
                    if (provZone == null)
                    {                      
                        provZone = Instantiate(zonePrefab, touchInWorldCoord, Quaternion.identity);
                        provZone.GetComponentInChildren<MeshRenderer>().material = provZoneMat;
                    }
                    else
                    {
                        provZone.SetActive(true);
                        provZone.transform.localScale = Vector3.one * 5;
                        provZone.transform.position = touchInWorldCoord;
                    }
                }
                break;

            // Determine direction by comparing the current touch position with the initial one.
            case TouchPhase.Moved:
                if (provZone.activeSelf == true)
                {
                    float distanceFromCenterToTap = (Vector3.zero - touchInWorldCoord).magnitude;
                    provZone.transform.localScale = new Vector3(distanceFromCenterToTap * 2, 5f, distanceFromCenterToTap * 2);
                }           

                break;

            // Report that a direction has been chosen when the finger is lifted.
            case TouchPhase.Ended:
                if (provZone.activeSelf == true)
                {
                    provZone.GetComponent<Zone>().creatingObject = false;
                }
             
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

        float zoomSpeed = 1f;

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
                staticZone = PhotonNetwork.Instantiate(zonePrefab.name, provZone.transform.position, Quaternion.identity);
                staticZone.transform.localScale = provZone.transform.localScale;
				staticZone.GetComponentInChildren<MeshRenderer>().material = staticZoneMat;
                HideProvZone();

                staticZone.transform.GetChild(1).gameObject.AddComponent<ArePlayersInsideZone>();
            }
            else //Si ya habia una zona en el mapa, se guarda la zona editada en nextZone
            {
                //Vector3 geoPosition = editableZone.GetComponent<ZonePositionWithLatLon>().GetGeoPosition();
                newZonePosition = PhotonNetwork.Instantiate(zonePrefab.name, provZone.transform.position, Quaternion.identity);
                newZonePosition.transform.localScale = provZone.transform.localScale;
                newZonePosition.GetComponentInChildren<MeshRenderer>().material = newZonePositionMat;
                provZoneCreated = false;

            }      
        }

    }

    public void CreateNewPosZone()
    {
        if (newZonePosition == null)
        {
            newZonePosition = Instantiate(zonePrefab);
            newZonePosition.GetComponentInChildren<MeshRenderer>().material = newZonePositionMat;
        }

        newZonePosition.transform.position = provZone.transform.position;
        newZonePosition.transform.localScale = provZone.transform.localScale;

        HideProvZone();
    }

    public void StartClosingZone()
    {   
        StartCoroutine(CloseActualZone());
    }

    public IEnumerator CloseActualZone()
    {
        float timeToClose = 3.0f;
        float t = 0;
        Vector3 InitialScale = staticZone.transform.localScale;
        Vector3 FinalScale = newZonePosition.transform.localScale;
        Vector3 Initialpos = staticZone.transform.position;
        Vector3 Finalpos = newZonePosition.transform.position;

        while (t <= 1)
        {
            staticZone.transform.localScale = Vector3.Lerp(InitialScale, FinalScale, t);
            staticZone.transform.position = Vector3.Lerp(Initialpos, Finalpos, t);
            t += Time.deltaTime / timeToClose;
            yield return null;
        }
        staticZone.transform.localScale = FinalScale;
        staticZone.transform.position = Finalpos;
        PhotonNetwork.Destroy(staticZone);
        staticZone = PhotonNetwork.Instantiate(zonePrefab.name, Finalpos, Quaternion.identity); //NextZone pasa a ser nuestra actualZone y borramos nextZone
        staticZone.transform.localScale = FinalScale;
        staticZone.GetComponentInChildren<MeshRenderer>().material = staticZoneMat;
        staticZone.transform.GetChild(1).gameObject.AddComponent<ArePlayersInsideZone>();

        //newZonePosition.SetActive(false);
        Destroy(newZonePosition);
        PhotonNetwork.Destroy(provZone);

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
                    GameObject newDrop = Instantiate(dropPrefab, worldPosition, Quaternion.identity);
                    newDrop.GetComponent<Drop>().creatingObject = false;
                    dropList.Add(newDrop);
                    newDrop.GetComponent<Drop>().lastTouchedDrop();
                    break;
            }
        }
        else
        {
            //Si no se detecta bien el input del dedo, se borra la imagen del drop, ese intento de crear el drop no cuenta
            if (dropPos != null)
            {
                Destroy(dropPos);
            }
        }
    }
    public void SendDropsToOtherPlayers()
    {
        foreach (GameObject g in dropList)
        {
            GameObject n = PhotonNetwork.Instantiate(dropPrefab.name, g.transform.position, Quaternion.identity);
            n.transform.localScale = g.transform.localScale;
            Destroy(g);
        }
        dropList.Clear();

    }
    public void DeleteDrop()
    {
        dropList.Remove(lastDropTapped);
        Destroy(lastDropTapped);
        numDrops++;
    }
	#endregion

	public float getDistanceFromCameraToGround()
	{
		return (GameObject.Find("LocationBasedGame").transform.position - GameObject.Find("Main Camera").transform.position).magnitude;
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

}
