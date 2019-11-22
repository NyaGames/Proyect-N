using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemasterManager : MonoBehaviour
{
    public static GamemasterManager Instance { get; private set; }

    private GameObject editableZone;
    private GameObject actualZone;
    private GameObject nextZone;
    public GameObject zonePrefab;
    private List<GameObject> dropList = new List<GameObject>();
    public GameObject dropPrefab;

    Vector3 lasPositionTapped = new Vector3(0, 0, 0);
    Vector3 zoneCenter = new Vector3(0, 0, 0);

    public bool creatingDrop = false;
    [HideInInspector]public GameObject dropPos;
    public int numDrops = 3;
    public bool zoneCreated;
    [HideInInspector] public GameObject lastDropTapped;

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

    public void Start()
    {
     
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!zoneCreated && PhotonNetwork.CurrentRoom != null) //Si no se ha creado la zona todavía y estoy dentro de una sala,puedo crearla
        {
            CreateZone();
        }


        if (creatingDrop) //Si el botón de los drops está holdeado, se puede crear un drop
        {
            CreateDrop();
        }

    }

    
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
    //Zone methods
    public void CanCreateNewZone()
    {
        zoneCreated = false; //Podemos crear una nueva zona
        Debug.Log("Puedes crear una nueva zona");
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
                    if (editableZone == null)
                    {
                        lasPositionTapped = touch.position;
                        zoneCenter = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
                        editableZone = Instantiate(zonePrefab, zoneCenter,Quaternion.identity);
                    }
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if (editableZone != null)
                    {
                        float distanceFromCenterToTap = (zoneCenter - touchInWorldCoord).magnitude;
                        editableZone.transform.localScale = new Vector3(distanceFromCenterToTap * 2, distanceFromCenterToTap * 2, distanceFromCenterToTap * 2);
                    }

                    lasPositionTapped = touch.position;

                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    if (editableZone != null)
                    {
                        zoneCreated = true;
                        editableZone.GetComponent<Zone>().creatingObject = false;
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
        if (editableZone != null) //Solo se envía una zona si se ha creado previamente en local. Si no se ha editado nada, no se puede mandar nada
        {
            if (actualZone == null) //Si no hay ninguna zona todavia, se guarda la zona editada en actualZone
            {
                actualZone = PhotonNetwork.Instantiate(zonePrefab.name, editableZone.transform.position, Quaternion.identity);
                actualZone.transform.localScale = editableZone.transform.localScale;
            }
            else //Si ya habia una zona en el mapa, se guarda la zona editada en nextZone
            {
                //Vector3 geoPosition = editableZone.GetComponent<ZonePositionWithLatLon>().GetGeoPosition();
                nextZone = PhotonNetwork.Instantiate(zonePrefab.name, editableZone.transform.position, Quaternion.identity);
                nextZone.transform.localScale = editableZone.transform.localScale;
            }
            Destroy(editableZone);
        }

    }
    public void enableCreateNewRadious()
    {
        if (editableZone != null)
        {
            if (editableZone.GetComponent<Zone>().creatingNewRadious)
            {
                editableZone.GetComponent<Zone>().creatingNewRadious = false;
            }
            else
            {
                editableZone.GetComponent<Zone>().creatingNewRadious = true;
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
        Vector3 InitialScale = actualZone.transform.localScale;
        Vector3 FinalScale = nextZone.transform.localScale;
        Vector3 Initialpos = actualZone.transform.position;
        Vector3 Finalpos = nextZone.transform.position;

        while (t <= 1)
        {
            actualZone.transform.localScale = Vector3.Lerp(InitialScale, FinalScale, t);
            actualZone.transform.position = Vector3.Lerp(Initialpos, Finalpos, t);
            t += Time.deltaTime / timeToClose;
            yield return null;
        }
        actualZone.transform.localScale = FinalScale;
        actualZone.transform.position = Finalpos;
        PhotonNetwork.Destroy(actualZone);
        actualZone = PhotonNetwork.Instantiate(zonePrefab.name, Finalpos, Quaternion.identity); //NextZone pasa a ser nuestra actualZone y borramos nextZone
        actualZone.transform.localScale = FinalScale;
        PhotonNetwork.Destroy(nextZone);

    }
    public void DeleteZone()
    {
        if(editableZone != null)
        {
            Destroy(editableZone);
            zoneCreated = false;
        }
    }
    //Drop methods
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



}
