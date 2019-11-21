using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosNotificationsManager : MonoBehaviour
{
    public static PhotosNotificationsManager Instance { get; private set; }

	public List<Texture2D> imagesReceived { get; private set; }

	[SerializeField] private GameObject notificationPrefab;

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

	public void OnImageRecived(Texture2D newImage)
	{
		imagesReceived.Add(newImage);

		GameObject notification = Instantiate(notificationPrefab);
		notificationPrefab.transform.SetParent(transform, false);

		notification.GetComponent<RawImage>().texture = newImage;
	}

	public void ImageOpened(Texture2D image)
	{
		imagesReceived.Remove(image);
	}
}
