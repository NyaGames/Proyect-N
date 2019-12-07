using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Slider>().value = 1;
	}

	public void SetVolume(float value)
	{
		FindObjectOfType<soundControl>().SetVolume(value);
	}
}
