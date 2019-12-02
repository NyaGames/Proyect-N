using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
	public RawImage image;
	[HideInInspector] public Texture2D textureReceived;
	[HideInInspector] public string snapper;
	[HideInInspector] public string snapped;

	private void Start()
	{
		image.texture = textureReceived;
	}

	public void OpenNotification()
	{
		PhotosNotificationsManager.Instance.OpenNotification(gameObject);
	}
}
