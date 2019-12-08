using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Wilberforce;

public class ColorModeDropdown : MonoBehaviour
{	
	public void ChangeColorMode()
	{sett
		int value = GetComponent<TMP_Dropdown>().value;
		FindObjectOfType<Colorblind>().Type = value;
		PersistentData.cameraMode = value;
	}

	public void ChangeLanguage()
	{
		FindObjectOfType<LanguageControl>().SetSelectedLanguage(GetComponent<TMP_Dropdown>().value);
	}
}
