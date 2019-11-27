using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

public class CloseZoneButton : MonoBehaviour
{
    [SerializeField] private Text minsText;
    [SerializeField] private Text secsText;

    private Button button;

    int secs, mins;

    public GameObject countDownPrefab;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void StartCountdown()
    {
        if (GameManager.Instance.gameStarted)
        {
            GamemasterManager.Instance.CreateNewPosZone();
            GameObject countDown = PhotonNetwork.Instantiate(countDownPrefab.name,Vector3.zero,Quaternion.identity);
            countDown.GetComponent<CountDown>().Create(mins * 60 + secs, "Zone close in", GameManager.Instance.CloseZone);
            countDown.GetComponent<CountDown>().StartCoundDown();
        }
    }

    private void Update()
    {
        int.TryParse(secsText.text, out secs);
        int.TryParse(minsText.text, out mins);

        if (secs <= 60 && mins <= 60 && !(mins == 0 && secs == 0) && GameManager.Instance.gameStarted && (GamemasterManager.Instance.provZone != null && GamemasterManager.Instance.provZone.activeSelf))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

}
