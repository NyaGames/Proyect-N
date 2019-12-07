using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class UpdateColorMode : MonoBehaviour
{
	private void Start()
	{
		FindObjectOfType<Colorblind>().Type = PersistentData.cameraMode;
	}
}
