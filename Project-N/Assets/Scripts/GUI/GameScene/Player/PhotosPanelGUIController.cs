using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosPanelGUIController : MonoBehaviour
{
	public static PhotosPanelGUIController Instance { get; private set; }
	[SerializeField] private GameObject photoConfirmationPanel;	
	[SerializeField] private GameObject usersPanel;

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

	public void TakePhoto()
	{
		Debug.Log("Photo Taken!");
		OpenPhotoConfirmationPanel();
	}

	public void ConfirmPhotoToSend()
	{
		Debug.Log("Photo Confirmed!");	
		usersPanel.SetActive(true);		
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
