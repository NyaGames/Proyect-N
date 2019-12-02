using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotosNotificationsManager : MonoBehaviour
{
    public static PhotosNotificationsManager Instance { get; private set; }	

	public List<Texture2D> imagesReceived { get; private set; }

	[SerializeField] private GameObject photoReceivedPanel;
 	[SerializeField] private GameObject notificationPrefab;
	[SerializeField] private TextMeshProUGUI senderText;
	[SerializeField] private TextMeshProUGUI playerToKillText;

	[SerializeField] private RectTransform rectTransform;

    private string PlayerToKill;
    private string Sender;

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

	public void OnImageRecived(Texture2D newImage, string sender, string playerToKill)
	{
		//notificationsRect.height += notificationPrefab.GetComponent<RectTransform>().rect.height + 10;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + rectTransform.rect.width + 10);

		imagesReceived.Add(newImage);

		GameObject notification = Instantiate(notificationPrefab);
		notification.transform.SetParent(transform, false);

		senderText.text = "Sent by: \n" + sender;
		playerToKillText.text = "Kill \n " + playerToKill + "?";
		//notification.GetComponent<Notification>().image.texture = newImage;
		notification.GetComponent<Notification>().textureReceived = newImage;

        PlayerToKill = playerToKill;
        Sender = sender;
    }


	public void OpenNotification(GameObject notification)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - rectTransform.rect.width + 10);

		Texture2D tex = (Texture2D)notification.GetComponent<Notification>().textureReceived;
		photoReceivedPanel.SetActive(true);
		GameSceneGUIController.Instance.targetImage.texture = tex;

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
