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

    private GameObject staticZone;
    private GameObject provZone;

	[SerializeField] private Material provZoneMat;
	[SerializeField] private Material staticZoneMat;

	private List<GameObject> dropList = new List<GameObject>();

	[HideInInspector] public GameObject dropPos;
	[HideInInspector] public GameObject lastDropTapped;


	Vector3 lasPositionTapped = new Vector3(0, 0, 0);
    Vector3 zoneCenter = new Vector3(0, 0, 0);

    public GameObject[] playersViewsList;

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
        playersViewsList = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ZoneManager.Instance.isEditingZone && PhotonNetwork.CurrentRoom != null) //Si no se ha creado la zona todavía y estoy dentro de una sala,puedo crearla
        {
            CreateZone();
        }


        if (creatingDrop) //Si el botón de los drops está holdeado, se puede crear un drop
        {
            CreateDrop();
        }

    } 

	#region Zone
    public void CreateZone()
    {
        if (Input.touchCount > 0)
        {
			Touch touch = Input.GetTouch(0);

			Vector2 posNormalized = touch.position / Screen.width;
			if (posNormalized.x > 0.66f) return;
            
            float cameraDistanceToGround = getDistanceFromCameraToGround();
            Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    if (provZone == null)
                    {
                        lasPositionTapped = touch.position;          
						provZone = Instantiate(zonePrefab, touchInWorldCoord, Quaternion.identity);
						provZone.GetComponentInChildren<MeshRenderer>().material = provZoneMat;
                    }
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if (provZone != null)
                    {
                        float distanceFromCenterToTap = (zoneCenter - touchInWorldCoord).magnitude;
						provZone.transform.localScale = new Vector3(distanceFromCenterToTap * 2, 5f, distanceFromCenterToTap * 2);
                    }

                    lasPositionTapped = touch.position;

                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    if (provZone != null)
                    {
						provZone.GetComponent<Zone>().creatingObject = false;
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
        if (provZone != null) //Solo se envía una zona si se ha creado previamente en local. Si no se ha editado nada, no se puede mandar nada
        {
            if (staticZone == null) //Si no hay ninguna zona todavia, se guarda la zona editada en actualZone
            {
                staticZone = PhotonNetwork.Instantiate(zonePrefab.name, provZone.transform.position, Quaternion.identity);
                staticZone.transform.localScale = provZone.transform.localScale;
            }
            else //Si ya habia una zona en el mapa, se guarda la zona editada en nextZone
            {
                //Vector3 geoPosition = editableZone.GetComponent<ZonePositionWithLatLon>().GetGeoPosition();
                provZone = PhotonNetwork.Instantiate(zonePrefab.name, provZone.transform.position, Quaternion.identity);
                provZone.transform.localScale = provZone.transform.localScale;
            }
            Destroy(provZone);
        }

    }

    public void enableCreateNewRadious()
    {
        if (provZone != null)
        {
            if (provZone.GetComponent<Zone>().creatingNewRadious)
            {
                provZone.GetComponent<Zone>().creatingNewRadious = false;
            }
            else
            {
                provZone.GetComponent<Zone>().creatingNewRadious = true;
            }
        }

    }

    public void StartClosingZone()
    {
        StartCoroutine(CloseActualZone());
    }

    public IEnumerator CloseActualZone()
    {
        float timeToClose = 1.0f;
        float t = 0;
        Vector3 InitialScale = staticZone.transform.localScale;
        Vector3 FinalScale = provZone.transform.localScale;
        Vector3 Initialpos = staticZone.transform.position;
        Vector3 Finalpos = provZone.transform.position;

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
        PhotonNetwork.Destroy(provZone);

    }

    public void DeleteZone()
    {
        if(provZone != null)
        {
            Destroy(provZone);           
        }
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
