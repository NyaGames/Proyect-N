namespace Mapbox.Unity.Location
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using System.Runtime.InteropServices;
	using System.Globalization;

	public class WebGlLocationProvider : AbstractLocationProvider
	{	
		private float _userHeading = 0;

		[DllImport("__Internal")]
		private static extern void StartTrackingLocalization();
		[DllImport("__Internal")]
		private static extern void StartTrackingOrientation();

		private void Awake()
		{	
            StartTrackingLocalization();
			StartTrackingOrientation();
		}

		public void LocationReceived(string location)
		{
			Debug.Log(location);
			Vector2d latLon = Conversions.StringToLatLon(location);
			Debug.Log(latLon);

			_currentLocation.UserHeading = _userHeading;
			_currentLocation.LatitudeLongitude = latLon; ;
			_currentLocation.Accuracy = 5;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;

			SendLocation(_currentLocation);
		}

		public void OrientationReceived(string orientation)
		{			
			_userHeading = float.Parse(orientation, CultureInfo.InvariantCulture.NumberFormat);
		}


       
	}
}
