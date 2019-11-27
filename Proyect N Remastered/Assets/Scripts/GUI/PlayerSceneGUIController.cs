using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSceneGUIController : MonoBehaviour
{
	public static PlayerSceneGUIController Instance { get; private set; }

	[SerializeField] private GameObject _changeCameraModeButton;
	[SerializeField] private GameObject _centerCameraButton;

	private PlayerCameraControl cameraControl;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}

		_centerCameraButton.SetActive(false);
		cameraControl = FindObjectOfType<PlayerCameraControl>();
	}

	public void ChangeCameraControl()
	{
		if (cameraControl.cameraMovement == CameraControl.CameraMovementType.Centering) return;

		cameraControl.ChangeCameraControlType();

		if(cameraControl.cameraControlType == PlayerCameraControl.CameraControlType.Ortho)
		{
			_centerCameraButton.SetActive(true);
			_changeCameraModeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lock";
		}
		else
		{
			_centerCameraButton.SetActive(false);
			_changeCameraModeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Free";
		}
	}

	public void CenterCamera()
	{
		StartCoroutine(cameraControl.CenterCameraToPlayer());
	}
}
