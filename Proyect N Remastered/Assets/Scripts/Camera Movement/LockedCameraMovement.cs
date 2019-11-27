using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCameraMovement : CameraMovement
{
	[Header("Parameters")]
    [Range(0.01f, 1.0f), SerializeField] private float smoothFactor = 0.5f;    
    [SerializeField] private float rotationSpeed = 10f;
	[SerializeField] private float zoomSpeed = 10f;
	[SerializeField] private Vector2 swivelZoom;
	[SerializeField] private Vector2 stickZoom;

    [Header("References")]
    [SerializeField] private Transform lockObjective;

     private void FixedUpdate()
     {
        Vector3 newPos = lockObjective.position;    
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
     }

    protected override void HandleInput()
    {
        switch (Input.touchCount)
        {
            case 1:
                RotateAroundPlayer(Input.GetTouch(0));
                break;
            case 2:
				AdjustZoom(Input.GetTouch(0), Input.GetTouch(1));
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
                    transform.Rotate(Vector3.up,  -delta);
                }
                else
                {
					transform.Rotate(Vector3.up, delta);
				}

            }
            else
            {
                delta = touchDeltaPosition.y * rotationSpeed;

                if (touch.position.x / Screen.width > 0.5f)
                {
					transform.Rotate(Vector3.up, delta);
                }
                else
                {
					transform.Rotate(Vector3.up, -delta);
				}
            }
          
        }
  
    }

	private void AdjustZoom(Touch firstTouch, Touch secondTouch)
	{
		if (Input.touchCount != 2) return;	

		Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
		Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

		float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
		float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

		float delta = -(currentMagnitude - prevMagnitude) * zoomSpeed;

		zoom = Mathf.Clamp01(zoom + delta);

		float angle = Mathf.Lerp(swivelZoom.x, swivelZoom.y, zoom);
		swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);

		float distance = Mathf.Lerp(stickZoom.x, stickZoom.y, zoom);
		stick.localPosition = new Vector3(0f, stick.localPosition.y, distance);
	}

    public override void Initialize()
    {
		m_camera.orthographic = false;
		m_camera.fieldOfView = 60;		
    }

	public void AssignObjective(Transform obj)
	{
		lockObjective = obj;

		stick.localPosition = startingStick;
		swivel.localRotation = Quaternion.Euler(startingSwivel);
	}

	public override IEnumerator DampCamera(Action onCoroutineFinished)
	{	

		Vector3 _targetPosition = startingStick;
		Vector3 _targetRotation = startingSwivel;

		Vector3 _movementVelocity = Vector3.zero;
		Vector3 _rotationVelocity = Vector3.zero;

		while ((stick.localPosition - _targetPosition).magnitude > 1f)
		{
			stick.localPosition = Vector3.SmoothDamp(stick.localPosition, _targetPosition, ref _movementVelocity, smoothTime);
			swivel.localRotation = Quaternion.Euler(Vector3.SmoothDamp(swivel.localRotation.eulerAngles, _targetRotation, ref _rotationVelocity, smoothTime));

			yield return new WaitForEndOfFrame();
		}

		onCoroutineFinished();
	}
}
