using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDown : MonoBehaviour,IPunObservable
{
    int secs;
    int secsSend;
    int secsReceived;
    string countDownString;
    UnityAction onCountDownFinished;
    bool countdownActive;

    PhotonView photonView;
    Text countDownText; 

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        countDownText = GameObject.FindGameObjectWithTag("CountDownText").GetComponent<Text>();
    }

    public void Create(int secs, string countDowntring, UnityAction onCountDownFinished)
    {
        countdownActive = false;

        this.secs = secs;
        this.countDownString = countDowntring;
        this.onCountDownFinished += onCountDownFinished;
        
    }

    public void StartCoundDown()
    {
        if (!countdownActive)
        {
            countdownActive = true;
            InvokeRepeating("Countdown", 1f, 1f);
        }
    }
    private void Countdown()
    {
        secs--;
        secsSend = secs;
        if (secsSend < 0)
        {
            PhotonNetwork.Destroy(photonView);
            CancelInvoke("Countdown");
            onCountDownFinished();
            countdownActive = false;
        }
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            secsSend = secs;
            countDownText.text = countDownString + secsSend;
        }
        else
        {
            countDownText.text = countDownString + secsReceived;
        }
       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (photonView.IsMine) //Si soy yo,mando mi info
            {
                stream.SendNext(secsSend);
            }
        }
        else if (stream.IsReading)
        {
            if (photonView != null && !photonView.IsMine) //Si no soy yo, updateo a quien me llegue
            {
                secsReceived = (int)stream.ReceiveNext();
            }
        }
    }
}
