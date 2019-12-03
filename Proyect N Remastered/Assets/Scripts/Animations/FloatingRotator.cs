using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 20f;
	[SerializeField] private float amplitude = 0.5f;
	[SerializeField] private float frequency = 1f;

	// Position Storage Variables
	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

	private float time = 0f;

	private void OnEnable()
	{
		posOffset = transform.position;

		time = 0f;
	}

	private void FixedUpdate()
	{
		
		transform.Rotate(new Vector3(0f, rotationSpeed * Time.fixedDeltaTime, 0f), Space.World);

		// Float up/down with a Sin()
		time += Time.fixedDeltaTime;
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(time * Mathf.PI * frequency) * amplitude;

		transform.position = tempPos;
	}

	public void AccelerateRotation(float acc)
	{
		rotationSpeed *= acc;
	}
}
