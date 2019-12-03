using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GmHelpMessages : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI message;

	public void SetMessage(string text)
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		message.text = text;


	}

	private void Update()
	{
		if(message.text == "")
		{
			gameObject.SetActive(false);
		}
	}


}
