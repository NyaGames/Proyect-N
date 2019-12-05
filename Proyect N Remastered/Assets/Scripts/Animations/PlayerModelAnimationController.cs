using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;

[RequireComponent(typeof(Animator))]
public class PlayerModelAnimationController : MonoBehaviour
{
	private bool isWalking = false;

	Animator anim;

	Vector3 prevPos;

	float threshold = 2f;

	private void Awake()
	{
		anim = GetComponent<Animator>();

		InvokeRepeating("CheckMovement", 0f, 0.5f);
	}	

	void CheckMovement()
	{

		if (prevPos == null)
		{
			prevPos = transform.position;
		}
		else
		{		

			if (isWalking)
			{
				if(prevPos == transform.position)
				{
					isWalking = false;
					anim.SetBool("IsWalking", isWalking);
				}
			}
			else
			{
				if (prevPos != transform.position)
				{
					isWalking = true;
					anim.SetBool("IsWalking", isWalking);
				}
			}

			prevPos = transform.position;
		}
	}
}
