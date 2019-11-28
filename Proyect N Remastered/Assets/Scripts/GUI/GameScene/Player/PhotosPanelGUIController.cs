using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotosPanelGUIController : MonoBehaviour
{
	public static PhotosPanelGUIController Instance { get; private set; }
	[SerializeField] private GameObject photoConfirmationPanel;	
	[SerializeField] private GameObject usersPanel;

    [SerializeField] private GameObject killedPanel;
    [SerializeField] private GameObject killConfirmedPanel;

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

    public void PlayerKilled(Texture2D killCam,string killer)
    {
        //killedPanel.SetActive(true);
        //killedPanel.GetComponent<SetPhotoReceivedInImage>().SetImage(killCam);
        PersistentData.killcam = killCam;
        PersistentData.killer = killer;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("DeathScene");
    }

    public void KillConfirmed(string playerKilled)
    {
        killConfirmedPanel.SetActive(true);
        killConfirmedPanel.GetComponent<KillConfirmation>().SetPlayerKilled(playerKilled);
    }
}
