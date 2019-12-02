using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeButtonText : MonoBehaviour
{
	[TextArea(2, 10)]
	public string[] texts;

	private int index = 0;

	private TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void ChangeText()
	{
		index = (index + 1) % texts.Length;
		text.text = texts[index];
	}
}
