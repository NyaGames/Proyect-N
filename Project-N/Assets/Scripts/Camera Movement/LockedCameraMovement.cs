using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCameraMovement : CameraMovement
{
    [Header("Parameters")]
    [Range(0.01f, 1.0f), SerializeField] private float smoothFactor = 0.5f;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform lockObjective;



    private void Start()
    {
        cameraOffset = stick.position - lockObjective.position;
        InitalizeCamera();
    }

    private void FixedUpdate()
    {
        Vector3 newPos = lockObjective.position + cameraOffset; 
   
        stick.position = Vector3.Slerp(stick.position, newPos, smoothFactor);
    }


    protected override void HandleInput()
    {
        switch (Input.touchCount)
        {
            case 1:
                RotateAroundPlayer(Input.GetTouch(0));
                break;
            case 2:               
                break;
            default:
                break;
        }
    }
    
    private void RotateAroundPlayer(Touch touch)
    {
        Vector2 touchDeltaPosition = touch.deltaPosition;     

        float delta;

        if (touch.phase == TouchPhase.Moved)
        {
            if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y))
            {

                delta = touchDeltaPosition.x * rotationSpeed;

                if (touch.position.y / Screen.height > 0.5f)
                {
                    stick.RotateAround(lockObjective.position, Vector3.up,  -delta);
                }
                else
                {
                    stick.RotateAround(lockObjective.position, Vector3.up, delta);
                }

            }
            else
            {
                delta = touchDeltaPosition.y * rotationSpeed;

                if (touch.position.x / Screen.width > 0.5f)
                {
                    stick.RotateAround(lockObjective.position, Vector3.up, delta);
                }
                else
                {
                    stick.RotateAround(lockObjective.position, Vector3.up, -delta);
                }
            }

          
        }
  
    }

    private void InitalizeCamera()
    {
        
    }


}
