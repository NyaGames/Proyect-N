using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (cameraLocked)
		{ 
			StartCoroutine(freeCameraMovement.DampCamera(UnlockCamera));
		}
		else
		{
			lockedCameraMovement.enabled = true;
			freeCameraMovement.enabled = false;

			lockedCameraMovement.Initialize();


			StartCoroutine(lockedCameraMovement.DampCamera(LockCamera));
		}

		cameraLocked = !cameraLocked;
	}

	public void UnlockCamera()
	{
		lockedCameraMovement.enabled = false;
		freeCameraMovement.enabled = true;

		freeCameraMovement.Initialize();
	}

	public void LockCameraToPlayer(Transform player)
	{
		lockedCameraMovement.Initialize();
		lockedCameraMovement.enabled = true;
		lockedCameraMovement.AssignObjective(player);

		freeCameraMovement.enabled = false;
	}

	public void LockCamera()
	{
		lockedCameraMovement.enabled = true;
		freeCameraMovement.enabled = false;
	}
}
