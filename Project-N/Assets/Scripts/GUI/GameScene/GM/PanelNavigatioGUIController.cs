using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNavigatioGUIController : MonoBehaviour
{
    public static PanelNavigatioGUIController Instance { get; private set; }

	[SerializeField] private GameObject[] panels;

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
	}

	public void OpenPanel(GameObject panel)
	{
		if (panel.activeSelf == true) return;

		ClosePanels();
		panel.SetActive(true);		
	}

	private void ClosePanels()
	{
		foreach (var gm in panels)
		{
			gm.SetActive(false);
		}
	}
}
