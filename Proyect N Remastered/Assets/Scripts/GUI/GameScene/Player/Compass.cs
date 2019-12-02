using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	[SerializeField] private RawImage compassImage;
	[SerializeField] private Texture2D compassSprite;
	[SerializeField] private Texture2D northLocked;
	[SerializeField] private Transform m_camera;

	private RectTransform rect;


	private void Awake()
	{
		rect = compassImage.GetComponent<RectTransform>();
	}

	public void LockNorth()
	{
		m_camera.GetComponent<LockedCameraMovement>().LockNorth();		
	}
	
	private void Update()
	{	
		rect.localRotation = Quaternion.Euler(new Vector3(0f, 0f, m_camera.transform.eulerAngles.y)); 

		if(Mathf.Round(rect.eulerAngles.z) == 0)
		{
			compassImage.texture = northLocked;
		}
		else
		{
			compassImage.texture = compassSprite;
		}
	}
}
