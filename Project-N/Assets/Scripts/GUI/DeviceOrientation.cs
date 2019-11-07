using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOrientation : MonoBehaviour
{
	[SerializeField] private ScreenOrientation orientation;

	private void Awake()
	{
		Screen.orientation = orientation;
	}
}
