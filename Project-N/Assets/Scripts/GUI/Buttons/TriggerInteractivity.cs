using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerInteractivity : MonoBehaviour
{
    public void Trigger(GameObject button)
	{
		button.GetComponent<Button>().interactable = !button.GetComponent<Button>().interactable;
	}
}
