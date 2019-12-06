using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class StartCountDownButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalSecsToStartGame;
    [SerializeField] private Button incrementSecsToStartGame;
    [SerializeField] private TextMeshProUGUI timeWarning;
    [SerializeField] private TextMeshProUGUI zoneWarning;

    public int incrementToStartGame;
    private Button button;

	int secs, mins;

    public GameObject startGameCountDownPrefab;

    private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Update()
	{
	
		int.TryParse(totalSecsToStartGame.text, out secs);

        if((GamemasterManager.Instance.provZone == null || !GamemasterManager.Instance.provZone.activeSelf) || (secs > 60 || mins > 60) ||( mins == 0 && secs == 0))
		{
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

		if(GamemasterManager.Instance.provZone == null || !GamemasterManager.Instance.provZone.activeSelf)
		{
			if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
			{
				zoneWarning.text = "There's no zone!";
			}
			else
			{
				zoneWarning.text = "¡No has puesto ninguna zona!";
			}
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
			if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
			{
				zoneWarning.text = "Time not valid!";
			}
			else
			{
				zoneWarning.text = "¡El tiempo no puede ser 0!";
			}
		}
	}

	public void StartGameCountdown()
	{
        GamemasterManager.Instance.SendZoneToOtherPlayers();

        GameObject countDown = PhotonNetwork.Instantiate(startGameCountDownPrefab.name, Vector3.zero, Quaternion.identity);

		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			countDown.GetComponent<StartGameCountDown>().Create(mins * 60 + secs, "Game starts in ", GameManager.Instance.StartGame);
		}
		else
		{
			countDown.GetComponent<StartGameCountDown>().Create(mins * 60 + secs, "El juego empieza en ", GameManager.Instance.StartGame);
		}
		
        countDown.GetComponent<StartGameCountDown>().StartCoundDown();

        totalSecsToStartGame.text = "0";
    }

    public void increaseTotalSecsToStartGame()
    {
        int actualSeconds;
        int.TryParse(totalSecsToStartGame.text, out actualSeconds);

        totalSecsToStartGame.text = (actualSeconds + incrementToStartGame).ToString();
    }

}
