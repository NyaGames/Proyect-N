using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDown : MonoBehaviour,IPunObservable
{
    int secs;
    int secsSend;
    int secsReceived;
    string countDownString = "NEW ZONE! Closes in ";
    UnityAction onCountDownFinished;
    bool countdownActive;

    PhotonView photonView;
    TextMeshProUGUI countDownText;
    public void Awake()
    {
        secs = 99;
        photonView = GetComponent<PhotonView>();
        GameObject g = GameObject.FindGameObjectWithTag("CountDownText");    
        countDownText = g.GetComponentInChildren<TextMeshProUGUI>();
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
        if (secsSend < -1)
        {
            GameManager.Instance.StartGame();
            CancelInvoke("Countdown");
            onCountDownFinished();
            countdownActive = false;
            PhotonNetwork.Destroy(photonView);
        }
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            secsSend = secs;
        }
        else
        {
            secs = secsReceived;
            if (secs < 0)
            {
                GameManager.Instance.StartGame();
            }
        }

        if (secs < 0)
        {
            countDownText.text = "CLOSING!";
        }
        else
        {
            countDownText.text = countDownString + secs + "...";
        }

    }


    public void OnDestroy()
    {
        //GameObject.FindGameObjectWithTag("CountDownText").transform.GetChild(0).gameObject.SetActive(false);
       // GameObject.FindGameObjectWithTag("CountDownText").transform.GetChild(1).gameObject.SetActive(false);
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
