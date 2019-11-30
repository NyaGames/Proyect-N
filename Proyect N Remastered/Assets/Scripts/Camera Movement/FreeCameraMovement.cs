using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraMovement : CameraMovement
{
	[Header("Parameters")]
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private Vector2 zoomClamping;
    [SerializeField] private float rotationSpeed = 10f;

    private bool isFollowingPlayer = false;
    private bool inputDisabled = false;

    private void OnEnable()
    {
		Initialize();        
    }

	private Transform playerToFollow;
	[HideInInspector] public Transform gameMaster;

	private void Update()
	{
		if (!inputDisabled)
		{
			base.Update();
		}
		else
		{
			FollowPlayer();
		}
	}

	protected override void HandleInput()
    {    
        switch (Input.touchCount)
        {
            case 1:
                AdjustPosition(Input.GetTouch(0));
                break;
            case 2:
                AdjustZoom(Input.GetTouch(0), Input.GetTouch(1));
                AdjustRotation(Input.GetTouch(0), Input.GetTouch(1));
                break;
            default:
                break;            
        }
    }   

    private void AdjustPosition(Touch touch)
    {
        Vector2 touchDeltaPosition = touch.deltaPosition;

        Vector3 direction = new Vector3(-touchDeltaPosition.x, -touchDeltaPosition.y, 0f).normalized;

        float speed = Mathf.Lerp(moveSpeed.x, moveSpeed.y, zoom);

        float distance = speed * 1000 * Time.deltaTime;
  
        stickRB.AddRelativeForce(direction * distance);
    }

    private void AdjustZoom(Touch firstTouch, Touch secondTouch)
    {
        if (Input.touchCount != 2) return;

        Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

        float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
        float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

        float delta = -(currentMagnitude - prevMagnitude);

        zoom = Mathf.Clamp01(zoom + delta * zoomSpeed * Time.deltaTime);

        float size = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);
        m_camera.orthographicSize = size;

        stick.transform.position = new Vector3(stick.transform.position.x, size, stick.transform.position.z);
    }

    private void AdjustRotation (Touch firstTouch, Touch secondTouch)
    {
        if (Input.touchCount != 2) return;

        Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

        Vector2 middlePoint = new Vector2((firstTouch.position.x + secondTouch.position.x) * 0.5f,
                                          (firstTouch.position.y + secondTouch.position.y) * 0.5f);

        Vector3 worldRotationPivot = m_camera.ScreenToWorldPoint(new Vector3(middlePoint.x, middlePoint.y, 0));

        Vector3 stickPosition = stick.position;     

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

    public void StartFollowingPlayer(Transform playerToFollow)
    {
		inputDisabled = true;
		this.playerToFollow = playerToFollow;
		StartCoroutine(DampToPosition(playerToFollow.position, SetFollowingPlayer));
    }

    public void StopFollowingPlayer()
    {
        isFollowingPlayer = false;
		inputDisabled = false;

	}

	public override void Initialize()
	{
		m_camera.orthographic = true;

		float halfFrustumHeight = (m_camera.transform.position.y + 15) * Mathf.Tan(m_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		m_camera.orthographicSize = halfFrustumHeight;
	

		swivel.localRotation = Quaternion.Euler(startingSwivel);
		stick.localPosition = startingStick;

		zoom = Mathf.InverseLerp(zoomClamping.x, zoomClamping.y, m_camera.orthographicSize);
		stick.transform.position = new Vector3(stick.transform.position.x, m_camera.orthographicSize, stick.transform.position.z);
	}

	public override IEnumerator DampCamera(Action onCoroutineFinished)
	{
		Vector3 _targetPosition = new Vector3(0, 0, -100);
		Vector3 _targetRotation = new Vector3(90, 0, 0);

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

	private void FollowPlayer()
	{
		transform.position = Vector3.Slerp(transform.position, playerToFollow.position, 0.5f);
	}

	public void CenterCamera()
	{
		StartCoroutine(DampToPosition(gameMaster.position));
	}

	private IEnumerator DampToPosition(Vector3 target, Action OnCorotuineFinished = null)
	{
		Vector3 _velocity = Vector3.zero;

		while ((transform.position - target).magnitude > 1f)
		{
			transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, smoothTime);		

			yield return new WaitForEndOfFrame();
		}	

		if(OnCorotuineFinished != null)
		{
			OnCorotuineFinished();
		}
	}

	private void SetFollowingPlayer()
	{
		isFollowingPlayer = true;
	}
}
