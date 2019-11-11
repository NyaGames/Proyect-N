using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOrientation : MonoBehaviour
{
	[SerializeField] private ScreenOrientation orientation;

	public bool portrait = true;

	private void Awake()
	{
		Screen.orientation = orientation;
	}

	public void ChangeOrientation()
	{
		portrait = !portrait;

		if (portrait)
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
		}
		else
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}
	}
}
