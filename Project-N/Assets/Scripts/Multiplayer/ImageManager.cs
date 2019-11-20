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

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        sourceImage = GameObject.FindGameObjectWithTag("SourceImage").GetComponent<RawImage>();
        targetImage = GameObject.FindGameObjectWithTag("TargetImage").GetComponent<RawImage>();
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

        photonView.RPC("ReceiveImageFromPlayer", RpcTarget.MasterClient, byteArray as object);

    }

    [PunRPC]
    void ReceiveImageFromPlayer(byte[] byteArray)
    {
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

}
