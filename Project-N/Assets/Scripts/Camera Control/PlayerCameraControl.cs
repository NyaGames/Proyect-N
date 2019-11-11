using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : CameraControl
{
	

	[Header("Player Camera Settings")]
	[SerializeField][Range(0.01f, 1f)]
	private float smoothFactor = 0.5f;
	[SerializeField]
	[Range(0, 5)]
	private float rotationAroundPlayerSpeed = 1f;
	[Range(0, 1)]
	private float zoomCameraLockedSpeed = 0.01f;

	[SerializeField]
	private float stickMinZoom, stickMaxZoom;
	[SerializeField]
	private float swivelMinZoom, swivelMaxZoom;

	public enum CameraControlType { Ortho, Perspective }
	//public CameraControlType cameraControlType { get; private set; }
	public CameraControlType cameraControlType;


	private Vector3 _cameraOffset;
	private Vector3 _originalOffset;
	private Vector3 _originalRotation;
	private bool _isFollowingPlayer = true;		

	private void Start()
	{
		cameraControlType = CameraControlType.Perspective;
		_cameraOffset = stick.transform.position - user.transform.position;
		_originalOffset = stick.transform.position - user.transform.position;
		_originalRotation = stick.localRotation.eulerAngles;

		zoom = Mathf.InverseLerp(stickMinZoom, stickMaxZoom, _cameraOffset.y);
	}

	protected override void PanCamera()
	{
		if (cameraControlType == CameraControlType.Perspective)
		{
			RotateAroundPlayer();
		}
		else
		{
			PanFreeCamera();
		}

	}

	protected override void RotateCamera()
	{
		if (cameraControlType == CameraControlType.Ortho)
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

			stick.Rotate(0, 0, -angularDiff * rotationSpeed * Time.deltaTime);
		}
	}

	protected override void ZoomCamera()
	{
		if (Input.touchCount != 2) return;

		if (cameraControlType == CameraControlType.Perspective)
		{
			Touch firstTouch = Input.GetTouch(0);
			Touch secondTouch = Input.GetTouch(1);

			Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
			Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

			float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
			float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

			float delta = (currentMagnitude - prevMagnitude) * zoomCameraLockedSpeed;

			zoom = Mathf.Clamp01(zoom + delta);

			/*float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
			swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);*/

			float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
			_cameraOffset.y = distance;
		}
		else
		{
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

	}

	protected override void LateUpdate()
	{
		base.LateUpdate();				

		if (_isFollowingPlayer || cameraMovement == CameraMovementType.Centering)
		{
			Vector3 newPos = user.position + _cameraOffset;

			stick.position = Vector3.Slerp(stick.position, newPos, smoothFactor);
			if (_isFollowingPlayer)
			{
				stick.LookAt(user);
			}
		}		

	}

	public void ChangeCameraControlType()
	{
		if(cameraControlType == CameraControlType.Ortho)
		{
			m_camera.orthographic = false;
			_cameraOffset.x = stick.position.x;
			_cameraOffset.z = stick.position.z;

			smoothTime = 0.5f;
			cameraControlType = CameraControlType.Perspective;
			StartCoroutine(ZoomInAnimation());
		}
		else
		{
			smoothTime = 0.5f;
			cameraControlType = CameraControlType.Ortho;
			StartCoroutine(ZoomOutAnimation());
		}
	}

	private IEnumerator ZoomOutAnimation()
	{
		Vector3 _target = new Vector3(user.position.x, stickMinZoom, user.position.z);
		Vector3 _velocity = Vector3.zero;

		cameraMovement = CameraMovementType.Centering;

		while ((_cameraOffset - _target).magnitude > 1f)
		{
			_cameraOffset = Vector3.SmoothDamp(_cameraOffset, _target, ref _velocity, smoothTime);
			yield return new WaitForEndOfFrame();
		}

		float halfFrustumHeight = _cameraOffset.y * Mathf.Tan(m_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		m_camera.orthographicSize = halfFrustumHeight;
		m_camera.orthographic = true;

		cameraMovement = CameraMovementType.Still;		
		_isFollowingPlayer = false;
			
	}

	private IEnumerator ZoomInAnimation()
	{
		Vector3 _targetPosition = _originalOffset;
		Vector3 _targetRotation = _originalRotation;
		Vector3 _movementVelocity = Vector3.zero;
		Vector3 _rotationVelocity = Vector3.zero;

		cameraMovement = CameraMovementType.Centering;

		while ((_cameraOffset - _targetPosition).magnitude > 1f)
		{
			_cameraOffset = Vector3.SmoothDamp(_cameraOffset, _targetPosition, ref _movementVelocity, smoothTime);
			stick.localRotation = Quaternion.Euler(Vector3.SmoothDamp(stick.localRotation.eulerAngles, _targetRotation, ref _rotationVelocity, smoothTime));
			
			yield return new WaitForEndOfFrame();
		}

		cameraMovement = CameraMovementType.Still;


		_isFollowingPlayer = true;
	}

	public IEnumerator CenterCameraToPlayer()
	{
		Vector3 velocity = Vector3.zero;
		Vector3 target = new Vector3(user.position.x, _cameraOffset.y, user.position.z);

		cameraMovement = CameraMovementType.Centering;

		while ((_cameraOffset - target).magnitude > 1f)
		{
			target = new Vector3(user.position.x, _cameraOffset.y, user.position.z);

			_cameraOffset = Vector3.SmoothDamp(_cameraOffset, target, ref velocity, smoothTime);

			yield return new WaitForEndOfFrame();
		}

		cameraMovement = CameraMovementType.Still;
	}

	private void RotateAroundPlayer()
	{
		Touch touch = Input.GetTouch(0);
		Vector2 touchDeltaPosition = touch.deltaPosition;

		Quaternion camTurnAngle;

		float delta;

		if (touch.phase == TouchPhase.Moved)
		{
			if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y))
			{

				delta = touchDeltaPosition.x * rotationAroundPlayerSpeed;

				if (touch.position.y / Screen.height > 0.5f)
				{
					camTurnAngle = Quaternion.AngleAxis(-delta, Vector3.up);
				}
				else
				{
					camTurnAngle = Quaternion.AngleAxis(delta, Vector3.up);
				}

			}
			else
			{
				delta = touchDeltaPosition.y * rotationAroundPlayerSpeed;

				if (touch.position.x / Screen.width > 0.5f)
				{
					camTurnAngle = Quaternion.AngleAxis(delta, Vector3.up);
				}
				else
				{
					camTurnAngle = Quaternion.AngleAxis(-delta, Vector3.up);
				}
			}

			_cameraOffset = camTurnAngle * _cameraOffset;
		}
	}

	private void PanFreeCamera()
	{
		Touch touch = Input.GetTouch(0);
		Vector2 touchDeltaPosition = touch.deltaPosition;
		float speed = Mathf.Lerp(panSpeed.x, panSpeed.y, zoom) * Time.deltaTime;

		stickRb.AddRelativeForce(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0f);
	}
}
