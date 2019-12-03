using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }
	[HideInInspector] public bool isEditingZone = false;
	[HideInInspector] public bool isEditingDrops = false;

	[SerializeField] private GameObject zoneMod;

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

	public void TriggerZoneMode()
	{
		if (isEditingZone)
		{
			isEditingZone = false;
			zoneMod.SetActive(false);
			GamemasterManager.Instance.HideProvZone();
			GameSceneGUIController.Instance.gmHelp.SetMessage("");
		}
		else
		{
			isEditingZone = true;
			zoneMod.SetActive(true);
			GameSceneGUIController.Instance.gmHelp.SetMessage("Tap to create a zone");
		}		
	}

	public void TriggerDropsMode()
	{
		if (isEditingDrops)
		{
			isEditingDrops = false;
			zoneMod.SetActive(false);
			GamemasterManager.Instance.DeleteProvDrops();
			GameSceneGUIController.Instance.gmHelp.SetMessage("");
		}
		else
		{
			isEditingDrops = true;
			zoneMod.SetActive(true);			
			GameSceneGUIController.Instance.gmHelp.SetMessage("Tap to create a drop");
		}
	}
}
