using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForBackground : MonoBehaviour
{
	[SerializeField] private GameObject bgPrefab;

	private void Start()
	{
		GameObject bg = GameObject.FindGameObjectWithTag("MenusBackground");

		if(bg == null)
		{
			Instantiate(bgPrefab);
		}
	}
}
