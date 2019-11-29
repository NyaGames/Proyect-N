using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSceneGUIController : MonoBehaviour
{
	public static PlayerSceneGUIController Instance { get; private set; }

	[SerializeField] private GameObject _changeCameraModeButton;
	[SerializeField] private GameObject _centerCameraButton;

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
	}	
}
