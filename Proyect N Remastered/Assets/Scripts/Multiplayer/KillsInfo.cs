using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillsInfo : MonoBehaviour,IPunObservable
{
    [HideInInspector] public int currentKills;
    private int sendKills;
    [HideInInspector] public int receivedKills;
    PhotonView photonView;

    public void Awake()
    {
        currentKills = 0;
        photonView = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            sendKills = currentKills;
        }
        else
        {
            currentKills = receivedKills;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (photonView.IsMine)
            {
                stream.SendNext(sendKills);
            }

        }
        else if (stream.IsReading)
        {
            if (!photonView.IsMine)
            {
                receivedKills = (int)stream.ReceiveNext();
            }

        }
    }
}
