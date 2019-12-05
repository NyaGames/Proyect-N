using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotosNotificationsManager : MonoBehaviour
{
    public static PhotosNotificationsManager Instance { get; private set; }	

	public List<Notification> notifications { get; private set; }

	[SerializeField] private GameObject photoReceivedPanel;
 	[SerializeField] private GameObject notificationPrefab;
	[SerializeField] private TextMeshProUGUI senderText;
	[SerializeField] private TextMeshProUGUI playerToKillText;

	[SerializeField] private RectTransform rectTransform;  

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

		notifications = new List<Notification>();
	}

	private void Start()
	{
		notificationsRect = rectTransform.rect;
	}

	public void OnImageRecived(Texture2D newImage, string sender, string playerToKill)
	{
		//notificationsRect.height += notificationPrefab.GetComponent<RectTransform>().rect.height + 10;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + rectTransform.rect.width + 10);

		

		GameObject notification = Instantiate(notificationPrefab);
		notification.transform.SetParent(transform, false);

		/*senderText.text = "Sent by: \n" + sender;
		playerToKillText.text = "Kill \n " + playerToKill + "?";*/

		Notification notif = notification.GetComponent<Notification>();
		//notification.GetComponent<Notification>().image.texture = newImage;
		notif.textureReceived = newImage;
		notif.snapped = playerToKill;
		notif.snapper = sender;
		notifications.Add(notif);
	}


	public void OpenNotification(GameObject notification)
	{
		Notification notif = notification.GetComponent<Notification>();
		photoReceivedPanel.SetActive(true);

		GameSceneGUIController.Instance.photoReceivedPanel.SetInfo(notif.textureReceived, notif.snapped, notif.snapper);
		RemoveNotification(notif);
	}

	private void RemoveNotification(Notification notification)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - (rectTransform.rect.width + 10));
		notifications.Remove(notification);
		Destroy(notification.gameObject);
	}

	public void ConfirmDeath(string Sender, string PlayerToKill, byte[] killImage)
	{
		photoReceivedPanel.SetActive(false);
        GameManager.Instance.myPlayer.GetComponent<MessageSender>().ConfirmKill(Sender, PlayerToKill, true, killImage);

		Debug.Log("Player Killed!");
	}

	public void CancelDeath()
	{
		photoReceivedPanel.SetActive(false);
		Debug.Log("Player not killed!");
	}

	public void PlayerDeathReceived(Photon.Realtime.Player otherPlayer)
	{
		for (int i = 0; i < notifications.Count; i++)
		{
			if(notifications[i].snapper == otherPlayer.NickName || notifications[i].snapped == otherPlayer.NickName)
			{
				RemoveNotification(notifications[i]);
				i--;
			}
		}
	}
}
