using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;

public class WebGlLocationDebug : MonoBehaviour
{
	private AbstractLocationProvider _locationProvider = null;

	[SerializeField] Text text;

	void Start()
	{
		if (null == _locationProvider)
		{
			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
		}
	}

	private void Update()
	{
		text.text = _locationProvider.name;
	}
}
