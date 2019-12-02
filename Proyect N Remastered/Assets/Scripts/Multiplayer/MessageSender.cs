using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	#region countDown
	public void SendCountdown(int countDown, string callbackMethod)
	{
		photonView.RPC(callbackMethod, RpcTarget.Others, countDown);
	}

	[PunRPC]
	public void ReceiveGameStartCountdown(int countDown)
	{
		//GameManager.Instance.OnCountDownReceived(countDown);
	}
    #endregion

    #region MasterResponse
    public void ConfirmKill(string sender,string playerToKill,bool killed){
        byte lastGroup = photonView.Group;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < GamemasterManager.Instance.playersViewsList.Length; i++)
            {
                if (GamemasterManager.Instance.playersViewsList[i].GetPhotonView().Owner.NickName == playerToKill)
                {
                    GamemasterManager.Instance.playersViewsList[i].GetPhotonView().RPC("KillYourself", RpcTarget.All, ImageManager.Instance.imagesList[0],sender,WaysToKillAPlayer.Image);
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
    public void ReceiveZoneClosingCountdown(int countDown)
    {
       
    }
    #endregion


    [PunRPC]
    public void KillYourself(byte [] image, string killer, WaysToKillAPlayer way)
    {
        switch (way)
        {
            case WaysToKillAPlayer.Image:
                Debug.Log("Matao");
                if (photonView.IsMine && !myPlayer.isGameMaster)
                {
                    PhotonNetwork.Destroy(gameObject);
                    PhotosPanelGUIController.Instance.PlayerKilled(getUncompressedTextureFromBytes(image), killer);
                }
                else
                {
                    
                    Debug.Log("No es mio,creado por: " + photonView.CreatorActorNr + " y lo controla:" + photonView.ControllerActorNr);
                }
                break;

            case WaysToKillAPlayer.Zone:
                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene("DeathByZone");
                    Debug.Log("TE MORISTE POR LA ZONA CRACK");
                }
                break;

            case WaysToKillAPlayer.GMChoice:
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("TE MATO EL GM CRACK");
                break;
        }

    }

    public override void OnLeftRoom()
    {
        
    }

    [PunRPC]
    public void KillReceived(string playerToKill,bool killed)
    {
        if(!PhotonNetwork.IsMasterClient)
        PhotosPanelGUIController.Instance.KillConfirmed(playerToKill);
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
}
