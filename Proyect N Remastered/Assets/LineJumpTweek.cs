using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineJumpTweek : MonoBehaviour
{

	TextMeshProUGUI text;
	public string spanish;
	public string english;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	void Start()
	{
		FindObjectOfType<LanguageControl>().LanguageChanged.AddListener(Tweek);
		Tweek();
	}

	private void Update()
	{
		//Tweek();
	}

	void Tweek()
	{
		if (FindObjectOfType<LanguageControl>().GetSelectedLanguage() == 0)
		{
			text.text = "Z" + "\n" + "o" + "\n" + "n" + "\n" + "e";
		}
		else
		{
			text.text = "Z" + "\n" + "o" + "\n" + "n" + "\n" + "a"; ;
		}
	}


}
