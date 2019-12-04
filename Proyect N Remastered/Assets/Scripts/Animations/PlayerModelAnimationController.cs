using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;

[RequireComponent(typeof(Animator))]
public class PlayerModelAnimationController : MonoBehaviour
{
	private bool isWalking;

	Animator anim;

	Vector3 prevPos;

	private void Awake()
	{
		anim = GetComponent<Animator>();

		InvokeRepeating("CheckMovement", 0f, 0.5f);
	}	

	void CheckMovement()
	{
		if(prevPos == null)
		{
			prevPos = transform.position;
		}
		else
		{
			if (isWalking)
			{
				if(transform.position == prevPos)
				{
					isWalking = false;
					anim.SetBool("IsWalking", isWalking);
				}
			}
			else
			{
				if (transform.position != prevPos)
				{
					isWalking = true;
					anim.SetBool("IsWalking", isWalking);
				}
			}

			prevPos = transform.position;
		}
	}
}
