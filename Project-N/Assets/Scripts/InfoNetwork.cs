using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class InfoNetwork : MonoBehaviour,Photon.Pun.IPunObservable
{
    private bool newZoneCreated;
    public PhotonView photonViewInfo;

    public void Start()
    {
        newZoneCreated = false;
    }

    public void CreateNewZone()
    {
        if (photonViewInfo.IsMine)
        {
            newZoneCreated = true;
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (GetComponent<Player>().isGameMaster)
            {
                stream.SendNext(newZoneCreated);
            }
            
        }
        else
        {
            if (!GetComponent<Player>().isGameMaster)
            {
                newZoneCreated = (bool)stream.ReceiveNext();
            }
        }
    }

}
