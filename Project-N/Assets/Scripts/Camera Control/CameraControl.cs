using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraControl : MonoBehaviour
{
    [Header("Panning")]
    [SerializeField]
    private float panSpeed;

    [Header("Zooming")]
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private Vector2 zoomClamping;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed;

    private Rigidbody rb;
    private MovementType movementType = MovementType.Still;

    private float zoomAmount = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                ZoomCamera(touchZero, touchOne);
                break;



        }
    }

    private void PanCamera(Touch touch)
    {    
        Vector2 touchDeltaPosition = touch.deltaPosition;
        rb.AddForce(-touchDeltaPosition.x * panSpeed, 0f, -touchDeltaPosition.y * panSpeed);
    }

    private void ZoomCamera(Touch touchZero, Touch touchOne)
    {
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchOne.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
        Debug.Log(deltaMagnitudeDiff);

        rb.AddRelativeForce(0f, 0f, deltaMagnitudeDiff * zoomSpeed);
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
