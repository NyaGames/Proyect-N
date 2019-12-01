using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonText : MonoBehaviour
{
	[TextArea(2, 10)]
	public string[] texts;

	private int index = 0;

	private TMPro.TextMeshProUGUI text;

	private void Awake()
	{
        GameObject g = gameObject;
		text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
	}

	public void ChangeText()
	{
		index = (index + 1) % texts.Length;
		text.text = texts[index];
	}
}
