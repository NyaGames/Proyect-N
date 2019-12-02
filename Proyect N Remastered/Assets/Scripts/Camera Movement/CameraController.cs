using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

[RequireComponent(typeof(FreeCameraMovement), typeof(LockedCameraMovement))]
public class CameraController : MonoBehaviour
{
	private LockedCameraMovement lockedCameraMovement;
	private FreeCameraMovement freeCameraMovement;

	private bool cameraLocked = true;

	private void Awake()
	{
		lockedCameraMovement = GetComponent<LockedCameraMovement>();
		freeCameraMovement = GetComponent<FreeCameraMovement>();
	}

	public void TriggerCameraLocking()
	{
		if (lockedCameraMovement.cameraIsDamping) return;

		if (cameraLocked)
		{
			lockedCameraMovement.cameraIsDamping = true;
			StartCoroutine(freeCameraMovement.DampStick(UnlockCamera));
		}
		else
		{
			lockedCameraMovement.cameraIsDamping = true;

			lockedCameraMovement.enabled = true;
			freeCameraMovement.enabled = false;

			lockedCameraMovement.Initialize();		

			StartCoroutine(lockedCameraMovement.DampStick(LockCamera));
		}

		cameraLocked = !cameraLocked;
	}

	public void UnlockCamera()
	{
		lockedCameraMovement.enabled = false;
		freeCameraMovement.enabled = true;
		lockedCameraMovement.cameraIsDamping = false;
		freeCameraMovement.Initialize();		
	}

	public void LockCameraToPlayer(Transform player)
	{
		lockedCameraMovement.AssignObjective(player);
		lockedCameraMovement.Initialize();
		lockedCameraMovement.enabled = true;


		freeCameraMovement.enabled = false;
	}

	public void LockCamera()
	{
		lockedCameraMovement.enabled = true;
		freeCameraMovement.enabled = false;
		lockedCameraMovement.cameraIsDamping = false;		
	}
}
