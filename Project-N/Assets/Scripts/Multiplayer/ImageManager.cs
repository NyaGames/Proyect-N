using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    //FOTOS
    public void SendImageToMaster()
    {
        byte[] byteArray = new byte[10];
        photonView.RPC("ReceiveImageFromPlayer", RpcTarget.MasterClient, byteArray as object);

    }

    [PunRPC]
    void ReceiveImageFromPlayer(byte[] byteArray)
    {
        for(int i = 0;i < byteArray.Length; i++)
        {
            Debug.Log(string.Format("Info " + i + ": " + byteArray[i]));
        }
        
    }

}
