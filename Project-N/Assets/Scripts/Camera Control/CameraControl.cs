using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraControl : MonoBehaviour
{
	[SerializeField] protected CameraMovementType cameraMovement = CameraMovementType.Still;

	[Header("Panning")]
	[SerializeField]
	protected Vector2 panSpeed;

	[Header("Zooming")]
	[SerializeField]
	protected float zoomSpeed;
	[SerializeField]
	protected Vector2 zoomClamping;
	[SerializeField]
	protected Vector2 swivelZoomClamping;

	[Header("Rotation")]
	[SerializeField]
	protected float rotationSpeed;
	[Range(0, 5)]
	[SerializeField] private float zoomingRotationSensibility = 0.1f;

	[Header("Centering Camera")]
	[SerializeField]
	protected float smoothTime = 0.3f;

	[Header("References")]
	[SerializeField]
	protected Transform user;
		
	protected Transform swivel, stick;
	protected new Camera camera;
	protected Rigidbody stickRb;

	protected float zoom = 0f;


	protected virtual void Awake()
	{
		swivel = transform.GetChild(0);
		stick = swivel.GetChild(0);
		camera = stick.GetChild(0).GetComponent<Camera>();

		stickRb = stick.GetComponent<Rigidbody>();
	}

	protected virtual void LateUpdate()
    {
        if(Input.touchCount > 0)
		{
			if (cameraMovement == CameraMovementType.Still)
			{
				HandleTouch();
			}
		}
		else
		{
			if (cameraMovement != CameraMovementType.Centering)
			{
				cameraMovement = CameraMovementType.Still;
			}
		}

		if (Input.touchCount <= 0) return;
		switch (cameraMovement)
		{
			case CameraMovementType.Still:
				break;
			case CameraMovementType.Panning:
				PanCamera();
				break;
			case CameraMovementType.Centering:
				CenterCamera();
				break;
			case CameraMovementType.Rotating:
				RotateCamera();
				break;
			case CameraMovementType.Zooming:
				ZoomCamera();
				break;
		}
	}

	private void HandleTouch()
	{
		switch (Input.touchCount)
		{
			case 1:			
				if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					cameraMovement = CameraMovementType.Panning;					
				}
				break;
			case 2:
				Touch firstTouch = Input.GetTouch(0);
				Touch secondTouch = Input.GetTouch(1);

				if (firstTouch.phase == TouchPhase.Moved && secondTouch.phase == TouchPhase.Moved)
				{
					Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
					Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;


					float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
					float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

					Debug.Log(Mathf.Abs(currentMagnitude - prevMagnitude));

					if (Mathf.Abs(currentMagnitude - prevMagnitude) <= zoomingRotationSensibility)
					{
						cameraMovement = CameraMovementType.Rotating;
					}
					else
					{
						cameraMovement = CameraMovementType.Zooming;
					}
				}	

				break;
			default:
				cameraMovement = CameraMovementType.Still;
				break;
		}
	}

	protected abstract void PanCamera();
	protected abstract void ZoomCamera();
	protected abstract void RotateCamera();

	private void CenterCamera() {
		cameraMovement = CameraMovementType.Centering;
	}


	protected enum CameraMovementType { Still, Panning, Zooming, Centering, Rotating }
}
