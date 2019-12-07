using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundControl : MonoBehaviour
{
	private void Awake()
	{
		SetVolume(1);
	}

	public void SetVolume(float volume)
	{
		AudioSource[] sources = FindObjectsOfType<AudioSource>();

		foreach (AudioSource source in sources)
		{
			source.volume = volume;
		}
	}
}
