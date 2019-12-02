﻿using UnityEngine;
using System.Collections;
using Unity.Collections;
using System.IO;
using UnityEngine.UI;
using System;

public class CameraWebPhoto : MonoBehaviour
{
    private bool camAvailable;
    WebCamTexture webCamTexture;
    WebCamTexture frontCam;
    WebCamTexture finalCam;
    private Texture defaultBackground;
    public AspectRatioFitter fit;
   
    public RawImage background;
    public RawImage snapTakenImage;

    public int tamaño = 512;


    void Start()
    {
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera");
            camAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices.Length);
            if (!devices[i].isFrontFacing)
            {
                webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            else
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }
        if (webCamTexture == null)
        {
            Debug.Log("BackCamera Unable");
            finalCam = frontCam;
        }
        else
        {
            finalCam = webCamTexture;
        }


        finalCam.Play();
        background.texture = finalCam;


        camAvailable = true;

        /*webCamTexture = new WebCamTexture();

        if(devices.Length > 1)
        {
            webCamTexture.deviceName = devices[1].name;
            webCamTexture.Play();
        }
        else
        {
            webCamTexture.deviceName = devices[0].name;
            webCamTexture.Play();
        }
        GetComponent<Renderer>().material.mainTexture = webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        //webCamTexture.Play();*/
    }
    private void Update()
    {
        if (!camAvailable)
            return;
        float ratio = (float)finalCam.width / (float)finalCam.height;
        fit.aspectRatio = ratio;
        
        float scaleY = finalCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -finalCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
       
    }

    public void takePhotoGuarra()
    {
        Texture2D tex = new Texture2D(finalCam.width, finalCam.height, TextureFormat.RGBA32, false);
        tex.SetPixels32(finalCam.GetPixels32());
        tex.Apply();
        if (tamaño > finalCam.width || tamaño > finalCam.height)
        {
            Debug.LogError("Tamaño mayor que imagen");
            Debug.Log("Tamaño mayor que imagen");
            while (tamaño > finalCam.width || tamaño > finalCam.height)
            {
                tamaño = tamaño / 2;
            }
        }
        Debug.Log("Height: " + tex.height + "  Width: " + tex.width);
        Texture2D photoTakenTex = ResampleAndCrop(tex, tamaño, tamaño);
        int orient = -finalCam.videoRotationAngle;
        if (orient != 0)
            photoTakenTex = rotate(photoTakenTex);
        Debug.Log("Height: "+ photoTakenTex.height + "  Width: "+ photoTakenTex.width);
        snapTakenImage.texture = photoTakenTex;
    }

    public static Texture2D ResampleAndCrop(Texture2D source, int targetWidth, int targetHeight)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;
        int xOffset = (sourceWidth - targetWidth) / 2;
        int yOffset = (sourceHeight - targetHeight) / 2;


        Color32[] data = source.GetPixels32();
        Color32[] data2 = new Color32[targetWidth * targetHeight];
        for (int y = yOffset; y < (targetHeight + yOffset); y++)
        {
            for (int x = xOffset; x < targetWidth + xOffset; x++)
            {
                Color32 color = data[x + sourceWidth * y];
                data2[(x - xOffset) + ((y - yOffset) * targetWidth)] = color;
            }
        }
        var tex = new Texture2D(targetWidth, targetHeight);
        tex.SetPixels32(data2);
        tex.Apply(true);
        return tex;
    }

    public static Texture2D rotate(Texture2D t)
    {     
            Texture2D newTexture = new Texture2D(t.height, t.width, t.format, false);

            for (int i = 0; i < t.width; i++)
            {
                for (int j = 0; j < t.height; j++)
                {
                    newTexture.SetPixel(j, i, t.GetPixel(t.width - i, j));
                }
            }
            newTexture.Apply();
            return newTexture;   
    }
   
}
