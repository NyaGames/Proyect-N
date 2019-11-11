using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPC : MonoBehaviour
{

    public Text Log;


    public void sendMessage()
    {
        string s = "\nSe ha creado una nueva zona";
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("NewZoneCreated", RpcTarget.All,s);
    }

    [PunRPC] //Se marca el metodo que va a llamarse en todos los clientes
    public void NewZoneCreated(string s)
    {
        Log.text += s;
    }

}
