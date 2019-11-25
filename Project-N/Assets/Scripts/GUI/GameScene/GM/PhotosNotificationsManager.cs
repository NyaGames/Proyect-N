using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosNotificationsManager : MonoBehaviour
{
    public static PhotosNotificationsManager Instance { get; private set; }	

	public List<Texture2D> imagesReceived { get; private set; }

	[SerializeField] private GameObject photoReceivedPanel;
 	[SerializeField] private GameObject notificationPrefab;
	[SerializeField] private Text senderText;
	[SerializeField] private Text playerToKillText;

	[SerializeField] private RectTransform rectTransform;

    private int PlayerToKill;
    private int Sender;

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
	}

	private void Start()
	{
		notificationsRect = rectTransform.rect;
	}

	public void OnImageRecived(Texture2D newImage, int sender, int playerToKill)
	{
		//notificationsRect.height += notificationPrefab.GetComponent<RectTransform>().rect.height + 10;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + notificationPrefab.GetComponent<RectTransform>().rect.height + 5);

		imagesReceived.Add(newImage);

		GameObject notification = Instantiate(notificationPrefab);
		notification.transform.SetParent(transform, false);

		senderText.text = "Sent by: " + sender.ToString();
		playerToKillText.text = "Do you want to kill " + playerToKill.ToString() + "?";
		notification.GetComponent<RawImage>().texture = newImage;

        PlayerToKill = playerToKill;
        Sender = sender;
    }


	public void OpenNotification(GameObject notification)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - (notificationPrefab.GetComponent<RectTransform>().rect.height + 5));

		Texture2D tex = (Texture2D)notification.GetComponent<RawImage>().texture;
		photoReceivedPanel.SetActive(true);
		photoReceivedPanel.GetComponentInChildren<RawImage>().texture = tex;

		imagesReceived.Remove(tex);
		Destroy(notification);
	}

	public void ConfirmDeath()
	{
		photoReceivedPanel.SetActive(false);
        GameManager.Instance.myPlayer.GetComponent<MessageSender>().ConfirmKill(Sender,PlayerToKill,true);

		Debug.Log("Player Killed!");
	}

	public void CancelDeath()
	{
		photoReceivedPanel.SetActive(false);
		Debug.Log("Player not killed!");
	}
}
