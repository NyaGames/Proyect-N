using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovement : MonoBehaviour
{
    protected Transform swivel, stick;
    protected Camera m_camera;
    protected Rigidbody stickRB;

    protected float zoom;

    private void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
        m_camera = stick.GetChild(0).GetComponent<Camera>();

        stickRB = stick.GetComponent<Rigidbody>();
         
    }

    private void Update()
    {
        //if (ZoneManager.Instance.isEditingZone) return;

        if(Input.touchCount > 0)
        {
            if (PersistentData.isGM)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if(Input.GetTouch(i).position.x / Screen.width > 0.66)
                    {
                        return;
                    }
                }
            }

            HandleInput();
        }
    }

    protected abstract void HandleInput();
}
