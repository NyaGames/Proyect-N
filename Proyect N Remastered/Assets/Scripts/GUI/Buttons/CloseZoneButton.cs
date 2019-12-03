using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

public class CloseZoneButton : MonoBehaviour
{
    [SerializeField] private Text minsForClosingText;
    [SerializeField] private Text secsForClosingText;
    [SerializeField] private TextMeshProUGUI minsText;
    [SerializeField] private TextMeshProUGUI secsText;

    private Button button;

    int secs, mins;
    int secsForClosing, minsForClosing;

    public GameObject countDownPrefab;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void StartCountdown()
    {
        if (GameManager.Instance.gameStarted)
        {
            GamemasterManager.Instance.SendZoneToOtherPlayers();
            GameObject countDown = PhotonNetwork.Instantiate(countDownPrefab.name,Vector3.zero,Quaternion.identity);
            countDown.GetComponent<CountDown>().Create(mins * 60 + secs, "NEW ZONE! Closes in ", GameManager.Instance.CloseZone);
            countDown.GetComponent<CountDown>().StartCoundDown();
        }
    }

    private void Update()
    {
        int.TryParse(secsText.text, out secs);
        int.TryParse(minsText.text, out mins);

        int.TryParse(secsForClosingText.text, out secsForClosing);
        int.TryParse(minsForClosingText.text, out minsForClosing);

        if ((secsForClosing <= 60 && minsForClosing <= 60 && !(minsForClosing == 0 && secsForClosing == 0)) && (secs <= 60 && mins <= 60 && !(mins == 0 && secs == 0)) && GameManager.Instance.gameStarted && (GamemasterManager.Instance.provZone != null && GamemasterManager.Instance.provZone.activeSelf))
        {
            GamemasterManager.Instance.secsClosingZone = minsForClosing * 60 + secsForClosing;
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

}
