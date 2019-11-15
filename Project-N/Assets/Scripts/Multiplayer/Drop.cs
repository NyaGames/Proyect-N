using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{

    public bool movingObject;
    public bool creatingObject;

    private void Start()
    {
        movingObject = false;
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
    }

    public void detectObjectTapped()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.gameObject == this.gameObject)
                {
                    lastTouchedDrop();
                    movingObject = true;
                    Debug.Log("Drop tapped");
                }

            }
        }
    }
    public void moveObject()
    {
        if (Input.touchCount > 0)
        {
            lastTouchedDrop();
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

    //Informa al gamemaster manager de que ESTE ha sido el último drop clickado
    public void lastTouchedDrop() 
    {
        if(GamemasterManager.Instance.lastDropTapped != null)
        {
            GamemasterManager.Instance.lastDropTapped.GetComponent<Renderer>().material.color = Color.white;
        }
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        GamemasterManager.Instance.lastDropTapped = this.gameObject;
    }


}
