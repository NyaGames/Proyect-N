using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundControl : MonoBehaviour
{
   public void SetVolume(float volume)
	{
		AudioSource[] sources = FindObjectsOfType<AudioSource>();

		foreach (AudioSource source in sources)
		{
			source.volume = volume;
		}
	}
}
