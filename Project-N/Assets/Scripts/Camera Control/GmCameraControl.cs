using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GmCameraControl : CameraControl
{	
	private Vector3 velocity = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();

		float size = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);
		m_camera.orthographicSize = size;
	}

	protected override void PanCamera()
	{
		Touch touch = Input.GetTouch(0);
		Vector2 touchDeltaPosition = touch.deltaPosition;
		float speed = Mathf.Lerp(panSpeed.x, panSpeed.y, zoom) * Time.deltaTime;

		stickRb.AddRelativeForce(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0f);
	}

	protected override void ZoomCamera()
	{
		if (Input.touchCount != 2) return;

		Touch firstTouch = Input.GetTouch(0);
		Touch secondTouch = Input.GetTouch(1);

		Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
		Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;
		
		float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
		float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

		float delta = currentMagnitude - prevMagnitude;

		zoom = Mathf.Clamp01(zoom + delta * zoomSpeed * Time.deltaTime);

		float size = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);
		m_camera.orthographicSize = size;

		stick.transform.position = new Vector3(stick.transform.position.x, size, stick.transform.position.z);
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();

		if (cameraMovement == CameraMovementType.Centering)
		{
			float size = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);		

			Vector3 target = new Vector3(user.position.x, size, user.position.z);

			stick.position = Vector3.SmoothDamp(stick.position, target, ref velocity, smoothTime);

			if((stick.position - target).magnitude <= 1f)
			{
				cameraMovement = CameraMovementType.Still;			
			}
		}
	}

	protected override void RotateCamera()
	{
		if (Input.touchCount != 2) return;

		Touch firstTouch = Input.GetTouch(0);
		Touch secondTouch = Input.GetTouch(1);

		Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
		Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

		Vector2 middlePoint = new Vector2((firstTouch.position.x + secondTouch.position.x) * 0.5f,
										  (firstTouch.position.y + secondTouch.position.y) * 0.5f);

		Vector3 worldRotationPivot = m_camera.ScreenToWorldPoint(new Vector3(middlePoint.x, middlePoint.y, 0));
	
		Vector3 stickPosition = stick.position;	

		pivot.localPosition = worldRotationPivot;

		stick.position = stickPosition;


		float actualAngularDifference = Mathf.Atan2(firstTouch.position.y - secondTouch.position.y,
													firstTouch.position.x - secondTouch.position.x) * Mathf.Rad2Deg;

		float prevAngularDifference = Mathf.Atan2(firstTouchPrevPos.y - secondTouchPrevPos.y,
												  firstTouchPrevPos.x - secondTouchPrevPos.x) * Mathf.Rad2Deg;

		if (actualAngularDifference > 90 && prevAngularDifference < -90)
		{
			actualAngularDifference -= 360;
		}

		if (actualAngularDifference < -90 && prevAngularDifference > 90)
		{
			actualAngularDifference += 360;
		}

		float angularDiff = actualAngularDifference - prevAngularDifference;

		swivel.Rotate(0, 0, -angularDiff * rotationSpeed * Time.deltaTime);
	}
}
