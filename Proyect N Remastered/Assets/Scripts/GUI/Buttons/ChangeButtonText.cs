using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeButtonText : MonoBehaviour
{
	[TextArea(2, 10)]
	public string[] englishTexts;
	[TextArea(2, 10)]
	public string[] spanishTexts;

	private int index = 0;

	private TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		FindObjectOfType<LanguageControl>().LanguageChanged.AddListener(LanguageChange);

		LanguageChange();
	}

	public void ChangeText()
	{
		if(FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			index = (index + 1) % englishTexts.Length;
			text.text = englishTexts[index];
		}
		else
		{
			index = (index + 1) % spanishTexts.Length;
			text.text = spanishTexts[index];
		}
		
	}

	private void LanguageChange()
	{
		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{		
			text.text = englishTexts[index];
		}
		else
		{
			index = (index + 1) % spanishTexts.Length;			
		}
	}
}
