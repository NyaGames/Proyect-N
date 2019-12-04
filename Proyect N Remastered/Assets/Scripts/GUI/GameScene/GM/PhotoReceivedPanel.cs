using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PhotoReceivedPanel : MonoBehaviour
{
	[SerializeField] private RawImage snap;
	[SerializeField] private TextMeshProUGUI snappedText;
	[SerializeField] private TextMeshProUGUI snapperText;

	public void SetInfo(Texture2D image, string snapped, string snapper)
	{
		snap.texture = image;
		snappedText.text = "Snapped: " + snapped;
		snapperText.text = "Snapper: " + snapper;
	}

	public void KillPlayer()
	{
		Texture2D tex = snap.texture as Texture2D;
		byte[] bytes = tex.GetRawTextureData();
		PhotosNotificationsManager.Instance.ConfirmDeath(snapperText.text, snappedText.text, bytes);
	}
	
}
