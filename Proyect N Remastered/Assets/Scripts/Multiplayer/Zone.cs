using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Zone : MonoBehaviour
{

    public bool creatingNewRadious;

    public bool movingObject;
    public bool creatingObject;
    public bool editRadius;

    private void Start()
    {
        movingObject = false;
        creatingObject = true;
        editRadius = false;
        creatingNewRadious = false;
    }
    private void Update()
{
        if (!creatingNewRadious)
        {
            if (!movingObject && !creatingObject) //Si no se mueve y no se esta creando, lo detectamos
            {
                bool tapped = detectObjectTapped();
                if (tapped)
                {
                    movingObject = true;
                }
            }
            else if (movingObject)  //Si puede moverse y no se esta crando, lo movemos
            {
                moveObject();
            }
        }
        else //Si se pulsa el botón de modificar el radio, vemos si estamos pulsando sobra la zona
        {
            if (!editRadius)
            {
                bool tapped = detectObjectTapped();
                Debug.Log("Creando nuevo radio");
                if (tapped)
                {
                    editRadius = true;
                }
            }
            else
            {
                setNewRadius();
            }
        }
       
    }

    public void setNewRadius()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float cameraDistanceToGround = GamemasterManager.Instance.getDistanceFromCameraToGround();
            Vector3 touchInWorldCoord = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraDistanceToGround));
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    float distanceFromCenterToTap = (gameObject.transform.position - touchInWorldCoord).magnitude;
                    gameObject.transform.localScale = new Vector3(distanceFromCenterToTap * 2, distanceFromCenterToTap * 2, distanceFromCenterToTap * 2);

                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    editRadius = false;
                    break;
            }
        }

    }
    public bool detectObjectTapped()
    {
        bool tapped = false;
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.gameObject == this.gameObject.transform.GetChild(0).gameObject)
                {
                    //Destroy(this.gameObject);
                    tapped = true;
                }

            }
        }
        return tapped;
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

	[PunRPC]
	public void NewZoneReceived()
	{
		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			GameSceneGUIController.Instance.playerMessages.AddMessage(new PlayerMessage("A new zone was created!", 2, 5f));
		}
		else
		{
			GameSceneGUIController.Instance.playerMessages.AddMessage(new PlayerMessage("¡Se ha creado una nueva zona!", 2, 5f));
		}
		
	}

   

}
