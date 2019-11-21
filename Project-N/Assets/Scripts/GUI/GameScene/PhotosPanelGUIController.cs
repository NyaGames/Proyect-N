using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosPanelGUIController : MonoBehaviour
{
	[SerializeField] private GameObject photoConfirmationPanel;
	[SerializeField] private RawImage sourceImage;

	public void TakePhoto()
	{
		Debug.Log("Photo Taken!");
		OpenPhotoConfirmationPanel();
	}

	public void ConfirmPhotoToSend()
	{
		Debug.Log("Photo Confirmed!");
		GameManager.Instance.myPlayer.GetComponent<ImageManager>().SendImageToMaster(sourceImage);
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
