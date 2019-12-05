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

	float threshold = 2f;

	private void Awake()
	{
		anim = GetComponent<Animator>();

		InvokeRepeating("CheckMovement", 0f, 0.5f);
	}	

	void CheckMovement()
	{
		Vector3 pos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

		if (prevPos == null)
		{
			prevPos = pos;
		}
		else
		{		

			if (isWalking)
			{
				if((prevPos - pos).magnitude < threshold)
				{
					isWalking = false;
					anim.SetBool("IsWalking", isWalking);
				}
			}
			else
			{
				if ((prevPos - pos).magnitude >= threshold)
				{
					isWalking = true;
					anim.SetBool("IsWalking", isWalking);
				}
			}

			prevPos = pos;
		}
	}
}
