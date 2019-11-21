using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    PhotonView photonView;

    private RawImage sourceImage;
    private RawImage targetImage;

    private Photon.Realtime.Player lastPlayerReference;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

    }
    //FOTOS
    public void SendImageToMaster()
    {
        Texture2D sourceTexture = (Texture2D)sourceImage.texture;
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

        int playerToKill = 5; //ELEGIR A QUIEN QUIERES MATAR -> actorNumber

        photonView.RPC("ReceiveImageFromPlayer", RpcTarget.MasterClient, byteArray as object, playerToKill);

    }

    [PunRPC]
    void ReceiveImageFromPlayer(byte[] byteArray ,int playerToKill,PhotonMessageInfo info)
    {
        lastPlayerReference = info.Sender;

        byte compressionRate = byteArray[byteArray.Length - 1];
        byteArray = byteArray.Take(byteArray.Count() - 1).ToArray();

        DownSampledImage image = new DownSampledImage(byteArray, compressionRate);

        byte[] uncompressedData = DownSampling.DecompressImage(image);

        int rowSize = (int) Mathf.Sqrt(byteArray.Length) * compressionRate;
        Texture2D tex = new Texture2D(rowSize, rowSize, TextureFormat.RGBA32, false);
        tex.LoadRawTextureData(uncompressedData);
        tex.Apply();

        targetImage.texture = tex;
        
    }

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
