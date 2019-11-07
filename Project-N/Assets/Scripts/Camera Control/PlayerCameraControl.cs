using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PlayerCameraControl : CameraControl
{	
	public override void CenterCamera()
	{
		throw new System.NotImplementedException();
	}

	protected override void PanCamera(Touch touch)
    {    
        Vector2 touchDeltaPosition = touch.deltaPosition;
		float speed = Mathf.Lerp(panSpeed.x, panSpeed.y, zoom);

		stickRb.AddForce(-touchDeltaPosition.x * speed, 0f, -touchDeltaPosition.y * speed);
    }

	protected override void ZoomCamera(float delta)
    {      
        zoom = Mathf.Clamp01(zoom + delta * zoomSpeed);

        float distance = Mathf.Lerp(zoomClamping.x, zoomClamping.y, zoom);
        stick.localPosition = new Vector3(stick.localPosition.x, stick.localPosition.y, distance);
        
        float angle = Mathf.Lerp(swivelZoomClamping.x, swivelZoomClamping.y, zoom);  
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);

		if(zoom == 0)
		{
			ChangeCameraMode(CameraMode.Ortographic);
		}
		else
		{
			if (camera.orthographic)
			{
				ChangeCameraMode(CameraMode.Perspective);
			}
		}

    }


	protected override void RotateCamera(Touch firstTouch, Touch secondTouch)
	{
		print("Rotating Camera");

	}

	private void ChangeCameraMode(CameraMode mode)
	{
		switch (mode)
		{
			case CameraMode.Perspective:
				camera.orthographic = false;
				camera.fieldOfView = 60;
				break;
			case CameraMode.Ortographic:
				camera.orthographic = true;
				camera.orthographicSize = 45;
				break;
		}
	}

	private enum CameraMode
	{
		Ortographic, Perspective
	}
}*/
