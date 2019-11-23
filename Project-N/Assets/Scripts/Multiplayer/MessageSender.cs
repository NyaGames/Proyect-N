using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class MessageSender : MonoBehaviour
{
    PhotonView photonView;    

    private Photon.Realtime.Player lastPlayerReference;

	private RawImage imageReceived;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

    }

	private void Start()
	{
		imageReceived = GameObject.FindGameObjectWithTag("TargetImage").GetComponent<GameSceneGUIController>().targetImage;
	}

	#region photos
	public void SendImageToMaster(RawImage image, int playerInPhotoID)
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

        photonView.RPC("ReceiveImageFromPlayer", RpcTarget.MasterClient, byteArray as object, playerInPhotoID);

    }

    [PunRPC]
    void ReceiveImageFromPlayer(byte[] byteArray ,int playerToKill,PhotonMessageInfo info)
    {
        int sender = info.Sender.ActorNumber;

        byte compressionRate = byteArray[byteArray.Length - 1];
        byteArray = byteArray.Take(byteArray.Count() - 1).ToArray();

        DownSampledImage image = new DownSampledImage(byteArray, compressionRate);

        byte[] uncompressedData = DownSampling.DecompressImage(image);

        int rowSize = (int) Mathf.Sqrt(byteArray.Length) * compressionRate;
        Texture2D tex = new Texture2D(rowSize, rowSize, TextureFormat.RGBA32, false);
        tex.LoadRawTextureData(uncompressedData);
        tex.Apply();

		PhotosNotificationsManager.Instance.OnImageRecived(tex, sender, playerToKill);	
    }
	#endregion

	#region countDown
	public void SendCountdown(int countDown)
	{
		photonView.RPC("ReceiveCountdown", RpcTarget.Others, countDown);
	}

	[PunRPC]
	public void ReceiveCountDown(int countDown)
	{
		GameManager.Instance.OnCountDownReceived(countDown);
	}
	#endregion



	public void DestroyPlayer(int id)
    {
        foreach(GameObject p in GamemasterManager.Instance.playersViewsList)
        {
            if(p.GetComponent<Player>().id == id)
            {
                PhotonNetwork.Destroy(p);
            }
        }

    }

}
