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

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Update()
	{
	
		int.TryParse(secsText.text, out secs);
		int.TryParse(minsText.text, out mins);

		if (GamemasterManager.Instance.staticZone != null && secs <= 60 && mins <= 60 && !(mins == 0 && secs == 0))
		{			
			button.interactable = true;					
		}
		else
		{
			button.interactable = false;
		}	

		if(GamemasterManager.Instance.staticZone == null)
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
		GameManager.Instance.StartGameCountdown(mins * 60 + secs);
	}
}
