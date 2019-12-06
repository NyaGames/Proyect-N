using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfZoneInfo : MonoBehaviour,IPunObservable
{
    public int maxSecsOutOfZone;
    [HideInInspector] public int currentSecsOutOfZone;
    private int sendcurrentSecsOutOfZone;
    [HideInInspector] public int receivedcurrentSecsOutOfZone;
    private bool outOfZoneActive = false;
    public bool insideZone;
    private PhotonView photonView;
    bool gm;

    public void Awake()
    {
        currentSecsOutOfZone = maxSecsOutOfZone;
        photonView = GetComponent<PhotonView>();
    }

    public void Start()
    {
        gm = GameManager.Instance.myPlayer.GetComponent<Player>().isGameMaster;
    }

    void Update()
    {
        if (!gm)
        {
            if (photonView.IsMine)
            {
                sendcurrentSecsOutOfZone = currentSecsOutOfZone;
            }
            else
            {
                currentSecsOutOfZone = receivedcurrentSecsOutOfZone;
            }

            if (GameManager.Instance.gameStarted) //Si la partida ha empezado, miro si estoy dentro de la zona
            {
                if (!insideZone)
                {
                    GameManager.Instance.outOfZonePanel.SetActive(true);
                    StartCoundDown();
                }
                else
                {
                    GameManager.Instance.outOfZonePanel.SetActive(false);
                    CancelInvoke("Countdown");
                    outOfZoneActive = false;
                }

            }
        }
    }

    public void StartCoundDown()
    {
        if (!outOfZoneActive)
        {
            outOfZoneActive = true;
            InvokeRepeating("Countdown", 1f, 1f);
        }
    }
    private void Countdown()
    {
        currentSecsOutOfZone--;
        if (currentSecsOutOfZone < 0)
        {
            CancelInvoke("Countdown");
            outOfZoneActive = false;
            Debug.Log("Te moriste por estar fuera de la zona");
            string s = "Zone kill";
            photonView.RPC("KillYourself",RpcTarget.All,new byte [0] ,s,WaysToKillAPlayer.Zone);
            
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sendcurrentSecsOutOfZone);
        }
        else if (stream.IsReading)
        {
            receivedcurrentSecsOutOfZone = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void ResetCurrentSecsOutOfZone()
    {
        currentSecsOutOfZone = maxSecsOutOfZone;
    }

}
