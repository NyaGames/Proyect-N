using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }
	[HideInInspector] public bool isEditingZone = false;

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

	public void EnterZoneMode()
	{
		isEditingZone = true;
		zoneMod.SetActive(true);
	}

	public void ExitZoneMode()
	{
		isEditingZone = false;
		zoneMod.SetActive(false);

	}
}
