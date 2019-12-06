using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

public class CloseZoneButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalSecsToStartClosing;
    [SerializeField] private TextMeshProUGUI totalSecsCloseDuration;
    [SerializeField] private Button incrementSecsToStartClosing;
    [SerializeField] private Button incrementSecsCloseDuration;
    private Button button;
    int secsToStartClosing;
    int secsCloseDuration;

    public int incrementToStartClosing;
    public int incrementCloseDuration;
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

			if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
			{
				countDown.GetComponent<CountDown>().Create(secsToStartClosing, "NEW ZONE! Closes in ", GameManager.Instance.CloseZone);
			}
			else
			{
				countDown.GetComponent<CountDown>().Create(secsToStartClosing, "NUEVA ZONA! Cierra en ", GameManager.Instance.CloseZone);
			}
			
            countDown.GetComponent<CountDown>().StartCoundDown();

            totalSecsToStartClosing.text = "0";
            totalSecsCloseDuration.text = "0";
        }
    }

    private void Update()
    {
        int.TryParse(totalSecsToStartClosing.text, out secsToStartClosing);
        int.TryParse(totalSecsCloseDuration.text, out secsCloseDuration);

        if ((secsCloseDuration <= 60  && !( secsCloseDuration == 0)) && (secsToStartClosing <= 60  && !( secsToStartClosing == 0)) && GameManager.Instance.gameStarted && (GamemasterManager.Instance.provZone != null && GamemasterManager.Instance.provZone.activeSelf))
        {
            GamemasterManager.Instance.secsClosingZone = secsCloseDuration;
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void increaseTotalSecsToStartClosing()
    {
        int actualSeconds;
        int.TryParse(totalSecsToStartClosing.text,out actualSeconds);

        totalSecsToStartClosing.text = (actualSeconds + incrementToStartClosing).ToString();
    }

    public void increaseTotalSecsCloseDuration()
    {
        int actualSeconds;
        int.TryParse(totalSecsCloseDuration.text, out actualSeconds);

        totalSecsCloseDuration.text = (actualSeconds + incrementCloseDuration).ToString();
    }

}
