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

	private string snapped;
	private string snapper;

	public void SetInfo(Texture2D image, string snapped, string snapper)
	{
		snap.texture = image;
		snappedText.text = "Snapped: " + snapped;
		snapperText.text = "Snapper: " + snapper;

		this.snapped = snapped;
		this.snapper = snapper;
	}

	public void KillPlayer()
	{
		Texture2D tex = snap.texture as Texture2D;
		byte[] bytes = tex.GetRawTextureData();
		PhotosNotificationsManager.Instance.ConfirmDeath(snapper, snapped, bytes);
	}
	
}
