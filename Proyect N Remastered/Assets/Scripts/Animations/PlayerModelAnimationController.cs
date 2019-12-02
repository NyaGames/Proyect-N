using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;

[RequireComponent(typeof(Animator))]
public class PlayerModelAnimationController : MonoBehaviour
{
	private bool isWalking;

	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (!isWalking)
		{
			if(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.SpeedMetersPerSecond > 0f)
			{
				isWalking = true;
				anim.SetBool("IsWalking", isWalking);
			}
		}
		else
		{
			if (LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.SpeedKmPerHour <= 0f)
			{
				isWalking = false;
				anim.SetBool("IsWalking", isWalking);
			}
		}
	}
}
