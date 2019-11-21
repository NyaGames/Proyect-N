using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOrientation : MonoBehaviour
{
    public static DeviceOrientation Instance { get; private set; }

	public bool portrait = true;

	private void Awake()
	{
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
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
