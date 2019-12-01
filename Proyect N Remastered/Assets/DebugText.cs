using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour
{
    public static DebugText Instance { get; private set; }

	private TextMeshProUGUI debugText;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		debugText = GetComponent<TextMeshProUGUI>();
	}

	public void SetText(string text)
	{
		debugText.text = text;
	}


}
