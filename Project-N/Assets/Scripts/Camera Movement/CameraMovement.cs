using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FreeCameraMovement), typeof(LockedCameraMovement))]
public abstract class CameraMovement : MonoBehaviour
{
	[Header("Camera Initialization")]
	[SerializeField] protected Vector3 startingStick;
	[SerializeField] protected Vector3 startingSwivel;

	protected Transform swivel, stick;
    protected Camera m_camera;
    protected Rigidbody stickRB;

    protected float zoom;
	private bool cameraLocked = true;
	private float smoothTime = 0.5f;

	private LockedCameraMovement lockedCameraMovement;
	private FreeCameraMovement freeCameraMovement;

	private void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
        m_camera = stick.GetChild(0).GetComponent<Camera>();

        stickRB = stick.GetComponent<Rigidbody>();

		lockedCameraMovement = GetComponent<LockedCameraMovement>();
		freeCameraMovement = GetComponent<FreeCameraMovement>();

        /*if (PersistentData.isGM)
		{
			lockedCameraMovement.enabled = false;
			freeCameraMovement.enabled = true;
		}
		else
		{
			lockedCameraMovement.enabled = true;
			freeCameraMovement.enabled = false;
		}*/

        lockedCameraMovement.enabled = false;
        freeCameraMovement.enabled = true;

    }

    private void Update()
    {
        if (ZoneManager.Instance.isEditingZone) return;

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
    protected abstract void Initialize();

	public void TriggerLockCamera()
	{

		if (cameraLocked)
		{
			Vector3 _targetPosition = new Vector3(0, 0, -100);
			Vector3 _targetRotation = new Vector3(90, 0, 0);

			StartCoroutine(DampCamera(_targetPosition, _targetRotation));
		}
		else
		{
			Vector3 _targetPosition = new Vector3(0, 7, -35);
			Vector3 _targetRotation = new Vector3(35, 0, 0);

			StartCoroutine(DampCamera(_targetPosition, _targetRotation));
		}

		cameraLocked = !cameraLocked;
	}	

	private IEnumerator DampCamera(Vector3 _targetPosition, Vector3 _targetRotation)
	{
		Vector3 _movementVelocity = Vector3.zero;
		Vector3 _rotationVelocity = Vector3.zero;

		while ((stick.localPosition - _targetPosition).magnitude > 1f)
		{
			stick.localPosition = Vector3.SmoothDamp(stick.localPosition, _targetPosition, ref _movementVelocity, smoothTime);
			swivel.localRotation = Quaternion.Euler(Vector3.SmoothDamp(swivel.localRotation.eulerAngles, _targetRotation, ref _rotationVelocity, smoothTime));

			yield return new WaitForEndOfFrame();
		}
	}

}
