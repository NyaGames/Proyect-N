﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PhotosNotificationsManager : MonoBehaviour
{
    public static PhotosNotificationsManager Instance { get; private set; }	

	public List<Texture2D> imagesReceived { get; private set; }

	[SerializeField] private GameObject photoReceivedPanel;
 	[SerializeField] private GameObject notificationPrefab;
	[SerializeField] private Text senderText;
	[SerializeField] private Text playerToKillText;

	private Rect notificationsRect;

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

		imagesReceived = new List<Texture2D>();
		notificationsRect = GetComponent<RectTransform>().rect;
	}

	public void OnImageRecived(Texture2D newImage, int sender, int playerToKill)
	{

		notificationsRect.height += notificationPrefab.GetComponent<RectTransform>().rect.height + 10; 

		imagesReceived.Add(newImage);

		GameObject notification = Instantiate(notificationPrefab);
		notification.transform.SetParent(transform, false);

		senderText.text = "Sent by: " + sender.ToString();
		playerToKillText.text = "Do you want to kill " + playerToKill.ToString() + "?";
		notification.GetComponent<RawImage>().texture = newImage;
	}


	public void OpenNotification(GameObject notification)
	{
		notificationsRect.height += notificationPrefab.GetComponent<RectTransform>().rect.height - 10;

		Texture2D tex = (Texture2D)notification.GetComponent<RawImage>().texture;
		photoReceivedPanel.SetActive(true);
		photoReceivedPanel.GetComponentInChildren<RawImage>().texture = tex;

		imagesReceived.Remove(tex);
		Destroy(notification);
	}

	public void ConfirmDeath()
	{
		photoReceivedPanel.SetActive(false);
		Debug.Log("Player Killed!");
        ImageManager.Instance.photonView.RPC("ReceiveConfirmationFromGM", RpcTarget.Others,true);
	}

	public void CancelDeath()
	{
		photoReceivedPanel.SetActive(false);
        Debug.Log("Player not killed!");
        ImageManager.Instance.photonView.RPC("ReceiveConfirmationFromGM", RpcTarget.Others, false);
    }
}
