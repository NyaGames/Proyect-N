using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum WaysToKillAPlayer
{
    Image, Zone, GMChoice
}

[RequireComponent(typeof(PhotonView))]
public class MessageSender : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    Player myPlayer;

    private Photon.Realtime.Player lastPlayerReference;

	private RawImage imageReceived;

    private byte[] imageOpened;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        myPlayer = GetComponent<Player>();

    }

	private void Start()
	{
        GameObject t = gameObject;
		imageReceived = GameSceneGUIController.Instance.targetImage;
        photonView.Group = (byte)myPlayer.id;
        if (myPlayer.isGameMaster) //Si eres game master, tus grupos de interes son todos los jugadores
        {
            for (byte i = 1;i <= PhotonNetwork.PlayerList.Length;i++)
            {
                PhotonNetwork.SetInterestGroups(i, true);
                PhotonNetwork.SetSendingEnabled(i, true);
            }
            
        }
        else //SI eres jugador normal, solo te interesa tu grupo, el de tu id
        {
            PhotonNetwork.SetInterestGroups((byte)myPlayer.id, true);
            PhotonNetwork.SetSendingEnabled((byte)myPlayer.id, true);
        }
    }

	#region photos
	public void SendImageToMaster(RawImage image, string playerNickname)
    {
        Texture2D sourceTexture = (Texture2D)image.texture;
        Color[] data = sourceTexture.GetPixels();
        DownSampledImage downSampledImage = DownSampling.CompressImage(data);

        byte[] byteArray = new byte[downSampledImage.data.Length + 1];

        for (int i = 0; i < byteArray.Length; i++)
        {

            if (i == byteArray.Length - 1)
            {
                byteArray[i] = downSampledImage.comppresionRate;
                break;
            }

            byteArray[i] = downSampledImage.data[i];           
        }        

        photonView.RPC("ReceiveImageFromPlayer", RpcTarget.MasterClient, byteArray as object, playerNickname);

    }

    [PunRPC]
    void ReceiveImageFromPlayer(byte[] byteArray, string playerToKill,PhotonMessageInfo info)
    {
        string sender = info.Sender.NickName;

        ImageManager.Instance.imagesList.Add(byteArray);
        Texture2D tex = getUncompressedTextureFromBytes(byteArray);

		PhotosNotificationsManager.Instance.OnImageRecived(tex, sender, playerToKill);	
    }
	#endregion

    #region MasterResponse
    public void ConfirmKill(string sender,string playerToKill, bool killed, byte[] image){
        byte lastGroup = photonView.Group;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < GamemasterManager.Instance.playersViewsList.Length; i++)
            {
                if (GamemasterManager.Instance.playersViewsList[i].GetPhotonView().Owner.NickName == playerToKill)
                {
                    GamemasterManager.Instance.playersViewsList[i].GetPhotonView().RPC("KillYourself", RpcTarget.All, image, sender, WaysToKillAPlayer.Image);
                }
                if(GamemasterManager.Instance.playersViewsList[i].GetPhotonView().Owner.NickName == sender)
                {
                    GamemasterManager.Instance.playersViewsList[i].GetPhotonView().RPC("KillReceived", RpcTarget.All, playerToKill, killed);
                }
            }
        }
        else
        {
            Debug.Log("NO ERES MASTER CLIENT");
        }

    }
    [PunRPC]
    public void ActivateCountdownText()
    {
        if (!PersistentData.isGM)
        {
            GameObject g = GameManager.Instance.playerCountDown;
            g.gameObject.SetActive(true);
        }
        else{
            GameObject g = GameManager.Instance.gmCountdown;
            g.gameObject.SetActive(true);
        }
      

    }
    [PunRPC]
    public void ReceiveZoneClosingCountdown(int currentTime,int maxTime)
    {
        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("CountDownText").GetComponentInChildren<TextMeshProUGUI>();

        int secondsLeft = maxTime - currentTime;

        string mins = Mathf.FloorToInt(secondsLeft / 60).ToString();
        if (Mathf.FloorToInt(secondsLeft / 60) < 10)
        {
            mins = "0" + mins;
        }

        string secs = Mathf.FloorToInt(secondsLeft % 60).ToString();

        if (Mathf.FloorToInt(secondsLeft % 60) < 10)
        {
            secs = "0" + secs;
        }

        text.text = mins + ":" + secs; 
    }
    [PunRPC]
    public void DeactivateCountdownText()
    {
        if (!PersistentData.isGM)
        {
            GameObject g = GameManager.Instance.playerCountDown;
            g.gameObject.SetActive(false);
        }
        else
        {
            GameObject g = GameManager.Instance.gmCountdown;
            g.gameObject.SetActive(false);
        }
    }
    #endregion


    [PunRPC]
    public void KillYourself(byte [] image, string killer, WaysToKillAPlayer way)
    {
        spawnMobile();
        switch (way)
        {
            case WaysToKillAPlayer.Image:
                Debug.Log("Matao");
                if (photonView.IsMine && !myPlayer.isGameMaster)
                {
                    PhotonNetwork.LeaveRoom();
                    GameSceneGUIController.Instance.photosPanel.GetComponent<PhotosPanelGUIController>().PlayerKilled(getTextureFromBytes(image), killer);
                    //TODO: TRANSFERIR OWNERSHIP
                }
                break;

            case WaysToKillAPlayer.Zone:
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene("DeathByZone");
                    Debug.Log("TE MORISTE POR LA ZONA CRACK");
                }
                break;

            case WaysToKillAPlayer.GMChoice:
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene("DeathByGM");
                    Debug.Log("TE MORISTE POR LA ZONA CRACK");
                }
                Debug.Log("TE MATO EL GM CRACK");
                break;
        }

        PhotonNetwork.Destroy(gameObject);

    }

    public void spawnMobile()
    {
        GameObject mobile = PhotonNetwork.Instantiate(GamemasterManager.Instance.mobilePrefab.name, gameObject.transform.position, Quaternion.identity);
        mobile.GetPhotonView().TransferOwnership(PhotonNetwork.MasterClient);
    }

    public override void OnLeftRoom()
    {
        
    }

    [PunRPC]
    public void KillReceived(string playerToKill,bool killed)
    {
        if (!PhotonNetwork.IsMasterClient)
            GameSceneGUIController.Instance.photosPanel.GetComponent<PhotosPanelGUIController>().KillConfirmed(playerToKill);
        //PhotosPanelGUIController.Instance.KillConfirmed(playerToKill);
    }		

    private Texture2D getUncompressedTextureFromBytes(byte[] byteArray)
    {
        byte compressionRate = byteArray[byteArray.Length - 1];
        byteArray = byteArray.Take(byteArray.Count() - 1).ToArray();        

        DownSampledImage image = new DownSampledImage(byteArray, compressionRate);

        byte[] uncompressedData = DownSampling.DecompressImage(image);

        int rowSize = (int)Mathf.Sqrt(byteArray.Length) * compressionRate;
        Texture2D tex = new Texture2D(rowSize, rowSize, TextureFormat.RGBA32, false);
        tex.LoadRawTextureData(uncompressedData);
        tex.Apply();

        return tex;
    }

	private Texture2D getTextureFromBytes(byte[] byteArray)
	{		
		int rowSize = (int) Mathf.Sqrt(byteArray.Length) / 2;
		Texture2D tex = new Texture2D(rowSize, rowSize, TextureFormat.RGBA32, false);
		tex.LoadRawTextureData(byteArray);
		tex.Apply();

		return tex;
	}
}
