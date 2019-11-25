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
       
        Debug.Log("Photo!!");
        Debug.Log(frontCam.width + " " + frontCam.height);
       
        var data2 = frontCam.GetPixels32();
    

        int aux = 0;
        int aux2 = 0;
        Color32[] newData2 = new Color32[(128 * 128)];

        byte[] sentColors3 = new byte[newData2.Length * 3];

        byte[] receivedColors2 = new byte[newData2.Length * 4];

        Color32[] newData = new Color32[newData2.Length];
        Color32 color;

        for (int j = 0; j < frontCam.height; j++)    
        {
            if (j > 56 && j < 184)
                aux2++;
            for (int i = 0; i < frontCam.width; i++)
            {
                if ((i > 96) && (i < 224) && (j > 56) && (j < 184))
                {
                    if (i < 96 + aux2)
                    {
                        newData2[aux] = data2[(j * frontCam.width) + (i+127)];       
                    }
                    else
                    {
                        newData2[aux] = data2[(j * frontCam.width) + i];     
                    }
                    aux++;   
                }
            }
        }
        
   
        Debug.Log(aux);
        Debug.Log(aux2);
        newData2[16380] = new Color32(0, 0, 255, 255);
        newData2[8180] = new Color32(255, 0, 0, 255);
        Debug.Log(newData2[8191]);


        ////////////ENVIAR DATOS//////////////
        for (int i = 0, j = 0; j < newData2.Length; i += 3, j++)
        {
            sentColors3[i] = newData2[j].r;
            sentColors3[i + 1] = newData2[j].g;
            sentColors3[i + 2] = newData2[j].b;
        }
        
        ///////////RECIBIR DATOS/////////////   
        for (int i = 0, j = 0; j < sentColors3.Length; i += 4, j += 3)
        {
            receivedColors2[i] = sentColors3[j];
            receivedColors2[i + 1] = sentColors3[j + 1];
            receivedColors2[i + 2] = sentColors3[j + 2];
            receivedColors2[i + 3] = 255;

        }

        
        
        ///////PASARLOS A COLOR32 AGAIN////////////////////
        for (int i = 0, j = 0; i < receivedColors2.Length; i += 4, j++)
        {
            color = new Color32(receivedColors2[i], receivedColors2[i + 1], receivedColors2[i + 2], receivedColors2[i + 3]);
            newData[j] = color;
        }

        Debug.Log(newData[8191]);
        Texture2D tex = new Texture2D(128, 128);
        tex.SetPixels32(newData);
        tex.Apply();
        photoPlane.texture = tex;
        

    }*/
    public void TakePhotoGuarra()
    {
        float h = snapTakenImage.texture.height;
        float w = snapTakenImage.texture.width;
        
        float x = ((w-128)/2)/w;
        float y = ((h - 128) / 2) / h;

        float newW = 128 / w;
        float newH = 128 / h;

        snapTakenImage.texture = background.texture;
        snapTakenImage.uvRect = new Rect(x, y, newW, newH);
     }

}
