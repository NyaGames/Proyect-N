using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KillConfirmation : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI usernameKilled;
    private Image background;

    public Color killConfirmedColor;
    public Color killDeniedColor;

    public void Awake()
    {
        background = GetComponent<Image>();
    }


    public void SetPlayerKilled(string playerKilled,int numKills)
    {
        background.color = killConfirmedColor;

		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			labelText.text = "You Snapped: ";
		}
		else
		{
			labelText.text = "Has snapedo a: ";
		}

		
		usernameKilled.text = playerKilled;//+ ". You have killed " + numKills + " people!";
    }

    public void SetPlayerNotKilled(string playerKilled, int numKills)
    {
        background.color = killDeniedColor;


		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			labelText.text = "Snap denied: ";
		}
		else
		{
			labelText.text = "Snap denegado: ";
		}

		
        usernameKilled.text = playerKilled;
    }
}
