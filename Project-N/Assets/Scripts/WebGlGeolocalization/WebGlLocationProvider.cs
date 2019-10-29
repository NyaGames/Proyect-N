namespace Mapbox.Unity.Location
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using System.Runtime.InteropServices;

	public class WebGlLocationProvider : AbstractLocationProvider
	{

		private WaitForSeconds _wait1sec;
		private WaitForSeconds _wait5sec;
		private WaitForSeconds _wait60sec;

		private Coroutine _pollLocation;

		Vector2d LatitudeLongitude
		{
			get
			{
				return new Vector2d(40.470198f, -3.63259f);
			}
		}

		/*[DllImport("__Internal")]
		private static extern void Hello();*/

		private void Awake()
		{
			_wait1sec = new WaitForSeconds(1);
			_wait5sec = new WaitForSeconds(5);
			_wait60sec = new WaitForSeconds(60);

			if (_pollLocation == null)
			{
				_pollLocation = StartCoroutine(locationRoutine());
			}
		}	

		private IEnumerator locationRoutine()
		{
			_currentLocation.UserHeading = 0;
			_currentLocation.LatitudeLongitude = LatitudeLongitude;
			_currentLocation.Accuracy = 5;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;
			_currentLocation.IsLocationServiceEnabled = true;

			SendLocation(_currentLocation);

			yield return _wait1sec;
		}
	}
}
