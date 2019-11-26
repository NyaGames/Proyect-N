using UnityEngine;
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
    private Texture defaultBackground;

   
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
            frontCam.Play();
            background.texture = frontCam;
            return;
        }

        webCamTexture.Play();
        
        background.texture = webCamTexture;

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
        //float ratio = (float)frontCam.width / (float)frontCam.height;
        //fit.aspectRatio = ratio;

        float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -frontCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
       
    }
    
    /*public void TakePhoto()
    {
       
        
       
        var data2 = frontCam.GetPixels32();
    

        int aux = 0;
 
        Color32[] newData2 = new Color32[(128 * 128)];

      

        Color32[] newData = new Color32[newData2.Length];


        for (int j = 0; j < frontCam.height; j++)    
        {
          
            for (int i = 0; i < frontCam.width; i++)
            {
                if ((i > 96) && (i < 224) && (j > 56) && (j < 184))
                {
                   
                        newData2[aux] = data2[(j * frontCam.width) + i];     
                   
                    aux++;   
                }
            }
        }
      
        Texture2D tex = new Texture2D(128, 128);

        tex.SetPixels32(newData2);
        tex.Apply();
        snapTakenImage.texture = tex;
        

    }*/

    public void TakePhotoGuarra()
    {
        Canvas canvas = null;
        foreach (var can in FindObjectsOfType<Canvas>())
        {
            if(can.name == "GUI")
            {
                canvas = can;
            }
        }
        
        float h = canvas.GetComponent<RectTransform>().rect.height;
        float w = canvas.GetComponent<RectTransform>().rect.width;

        if (tamaño > w || tamaño > h)
        {
            Debug.LogError("Tamaño mayor que imagen");
            Debug.Log("Tamaño mayor que imagen");
            while (tamaño > w || tamaño > h)
            {
                tamaño = tamaño / 2;
            }
        }
            
  
        float x = ((w-tamaño)/2)/w;
        float y = ((h - tamaño) / 2) / h;

        float newW = tamaño / w;
        float newH = tamaño / h;

        
        Texture2D tex = new Texture2D(frontCam.width, frontCam.height, TextureFormat.RGBA32, false);
        tex.SetPixels32(frontCam.GetPixels32());
        tex.Apply();

        snapTakenImage.texture = tex;
        
        snapTakenImage.uvRect = new Rect(x, y, newW, newH);
     }

}
