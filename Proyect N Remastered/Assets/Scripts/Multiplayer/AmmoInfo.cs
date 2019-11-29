using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInfo : MonoBehaviour,IPunObservable
{
    public int maxAmmo;
    [HideInInspector] public int currentAmmo;
    private int sendAmmo;
    [HideInInspector] public int receivedAmmo;
    PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
        currentAmmo = maxAmmo;
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            sendAmmo = currentAmmo;
        }
        else
        {
            currentAmmo = receivedAmmo;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (photonView.IsMine)
            {
                stream.SendNext(sendAmmo);
            }
            
        }else if (stream.IsReading)
        {
            if (!photonView.IsMine)
            {
                receivedAmmo = (int)stream.ReceiveNext();
            }
           
        }
    }
}
