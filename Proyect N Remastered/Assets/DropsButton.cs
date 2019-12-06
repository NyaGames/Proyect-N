using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropsButton : MonoBehaviour
{
	private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Update()
	{
		if(GamemasterManager.Instance.provDropList.Count == 0)
		{
			button.interactable = false;
		}
		else
		{
			button.interactable = true;
		}
	}
}
