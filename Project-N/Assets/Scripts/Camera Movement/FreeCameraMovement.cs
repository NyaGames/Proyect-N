using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraMovement : CameraMovement
{
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private Vector2 zoomClamping;
    [SerializeField] private float rotationSpeed = 10f;

    private bool isFollowingPlayer = false;

    private void Start()
    {
        zoom = Mathf.InverseLerp(zoomClamping.y, zoomClamping.x, m_camera.orthographicSize);
        stick.transform.position = new Vector3(stick.transform.position.x, m_camera.orthographicSize, stick.transform.position.z);
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

    private void AdjustRotation(Touch firstTouch, Touch secondTouch)
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
        isFollowingPlayer = true;
    }

    public void StopFollowingPlayer()
    {
        isFollowingPlayer = false;
    }

    public void CenterCamera()
    {

    }
}
