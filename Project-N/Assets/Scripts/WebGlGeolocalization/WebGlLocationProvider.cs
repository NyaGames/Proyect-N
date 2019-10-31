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

		[DllImport("__Internal")]
		private static extern void StartTrackingLocalization();		

		Vector2d LatitudeLongitude
		{
			get
			{
				return new Vector2d(40.470198f, -3.63259f);
			}
		}		

		private void Awake()
		{
			_wait1sec = new WaitForSeconds(1);
			_wait5sec = new WaitForSeconds(5);
			_wait60sec = new WaitForSeconds(60);

            /*if (_pollLocation == null)
			{
				_pollLocation = StartCoroutine(locationRoutine());
			}*/

            StartTrackingLocalization();
		}

        /*private IEnumerator locationRoutine()
		{
			while (true)
			{
				_currentLocation.UserHeading = 0;
				float[] location = RetrieveLocation();
				Debug.Log(location);
				Vector2d latLon = new Vector2d(location[0], location[1]);
				Debug.Log(latLon);
				_currentLocation.LatitudeLongitude = latLon; ;				
				_currentLocation.Accuracy = 5;
				_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
				_currentLocation.IsLocationUpdated = true;
				_currentLocation.IsUserHeadingUpdated = true;			

				SendLocation(_currentLocation);



				yield return _wait1sec;
			}
		}*/

		public void LocationReceived(string location)
		{
			Debug.Log(location);
			Vector2d latLon = Conversions.StringToLatLon(location);
			Debug.Log(latLon);

			_currentLocation.UserHeading = 0;
			_currentLocation.LatitudeLongitude = latLon; ;
			_currentLocation.Accuracy = 5;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;

			SendLocation(_currentLocation);
		}
       
	}
}
