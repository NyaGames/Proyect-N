using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public void OpenNotification()
	{
		PhotosNotificationsManager.Instance.OpenNotification(gameObject);
	}
}
