using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNavigationButton : MonoBehaviour
{
    public void OpenPanel(GameObject panel)
	{
		PanelNavigatioGUIController.Instance.OpenPanel(panel);
	}
}
