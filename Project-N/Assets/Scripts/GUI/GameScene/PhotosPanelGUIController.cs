using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ImageManager))]
public class PhotosPanelGUIController : MonoBehaviour
{
	[SerializeField] private GameObject photoConfirmationPanel;
	[SerializeField] private RawImage sourceImage;

	private ImageManager imageManager;

	private void Awake()
	{
		imageManager = GetComponent<ImageManager>();
	}

	public void TakePhoto()
	{
		Debug.Log("Photo Taken!");
		OpenPhotoConfirmationPanel();
	}

	public void ConfirmPhotoToSend()
	{
		Debug.Log("Photo Confirmed!");
		imageManager.SendImageToMaster(sourceImage);
		photoConfirmationPanel.SetActive(false);
	}

	public void CancelPhotoToSend()
	{
		Debug.Log("Photo Canceled!");
		photoConfirmationPanel.SetActive(false);
	}

	private void OpenPhotoConfirmationPanel()
	{
		photoConfirmationPanel.SetActive(true);
		

	}
}
