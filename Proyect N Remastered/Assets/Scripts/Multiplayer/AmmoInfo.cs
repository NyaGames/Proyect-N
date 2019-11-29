using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInfo : MonoBehaviour,IPunObservable
{
    public int maxAmmo;
    [HideInInspector] public int currentAmmo;

    public void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentAmmo);
        }else if (stream.IsReading)
        {
            currentAmmo = (int)stream.ReceiveNext();
        }
    }
}
