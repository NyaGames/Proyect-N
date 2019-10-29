using UnityEngine;
using System.Runtime.InteropServices;

public class WebGlGeolocalization : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void Hello();
	[DllImport("__Internal")]
	private static extern void WebGlGeolocation();

	private void Awake()
	{
		//WebGlGeolocation();
	}
}
