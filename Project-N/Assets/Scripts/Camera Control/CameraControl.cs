using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private bool isLockedOnPlayer;

    [Header("Panning")]
    [SerializeField]
    private float panSpeed;

    [Header("Zooming")]
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private Vector2 zoomClamping;
    [SerializeField]
    private Vector2 swivelZoomClamping;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed;

    private Rigidbody stickRb;
    private MovementType movementType = MovementType.Still;

    private float zoom = 0f;
    private Transform swivel, stick;    


    private void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);

        stickRb = stick.GetComponent<Rigidbody>();
    }

    private void HandleTouch()
    {
        switch (Input.touchCount)
        {
            case 1:
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch);
                }
                break;
            case 2:
                Touch firstTouch = Input.GetTouch(0);
                Touch secondTouch = Input.GetTouch(1);

                ZoomCamera(firstTouch, secondTouch);
                break;



        }
    }

    private void PanCamera(Touch touch)
    {    
        Vector2 touchDeltaPosition = touch.deltaPosition;
        stickRb.AddForce(-touchDeltaPosition.x * panSpeed, 0f, -touchDeltaPosition.y * panSpeed);
    }

    private void ZoomCamera(Touch firstTouch, Touch secondTouch)
    {
        Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

        float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
        float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

        float delta = currentMagnitude - prevMagnitude;

        zoom = Mathf.Clamp01(zoom + delta * zoomSpeed);

        float distance = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);
        stick.localPosition = new Vector3(stick.localPosition.x, stick.localPosition.y, distance);
        
        float angle = Mathf.Lerp(swivelZoomClamping.x, swivelZoomClamping.y, zoom);  
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    private void LateUpdate()
    {
        if(Input.touchCount > 0)
        {
            HandleTouch();
        }
    }

    private enum MovementType
    {
        Still, Panning, Zooming, Rotating
    }
}
