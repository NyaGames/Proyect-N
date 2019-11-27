using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartCountDownButton : MonoBehaviour
{
	[SerializeField] private Text minsText;
	[SerializeField] private Text secsText;

	[SerializeField] private Text timeWarning;
	[SerializeField] private Text zoneWarning;

	private Button button;

	int secs, mins;

    public GameObject countDownPrefab;

    private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Update()
	{
	
		int.TryParse(secsText.text, out secs);
		int.TryParse(minsText.text, out mins);

        if((GamemasterManager.Instance.provZone == null || !GamemasterManager.Instance.provZone.activeSelf) || (secs > 60 || mins > 60) ||( mins == 0 && secs == 0)){
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

		if(GamemasterManager.Instance.provZone == null || !GamemasterManager.Instance.provZone.activeSelf)
		{
			zoneWarning.text = "Zone not confirmed";
		}
		else
		{
			zoneWarning.text = "";
		}

		if(secs <= 60 && mins <= 60 && !(mins == 0 && secs == 0))
		{
			timeWarning.text = "";
		}
		else
		{			
			timeWarning.text = "Time not Valid";
		}
	}

	public void StartGameCountdown()
	{
        GamemasterManager.Instance.SendZoneToOtherPlayers();

        GameObject countDown = PhotonNetwork.Instantiate(countDownPrefab.name, Vector3.zero, Quaternion.identity);
        countDown.GetComponent<CountDown>().Create(mins * 60 + secs, "Game starts in", GameManager.Instance.StartGame);
        countDown.GetComponent<CountDown>().StartCoundDown();
    }
}
