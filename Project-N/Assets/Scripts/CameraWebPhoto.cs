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
    public RawImage photoPlane;
    //public AspectRatioFitter fit;

    //public GameObject plano2;
    //public Camera camera;
    //private float distance = 0.0f;


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
        if (Input.GetMouseButtonDown(0))
            TakePhoto();
    }
    public static string colorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
    public static byte[] StringToByteArray(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    public void TakePhoto()
    {
        //Texture2D photo = new Texture2D(frontCam.width, frontCam.height);
        //photo.SetPixels(frontCam.GetPixels());
        //photo.Apply();
        Debug.Log("Photo!!");
        Texture2D photo = new Texture2D(frontCam.width, frontCam.height);
        photo.SetPixels(frontCam.GetPixels());
        photo.Apply();
        
        var data = photo.GetRawTextureData<Color32>();
        string[] hexData= new string[data.Length];
        Debug.Log("Enviado" + data.Length);
        NativeArray<Color32> rgbData = new NativeArray<Color32>(data.Length, Allocator.Temp);
        byte[] sentColors = new byte[data.Length * 3];
        byte[] sentColors2 = new byte[data.Length * 3];


        ////////////ENVIAR DATOS//////////////
        /*for (int i=0; i < data.Length; i+=3)
        {
            sentColors[i] = data[i].r;
            sentColors[i + 1] = data[i].g;
            sentColors[i + 2] = data[i].b;
            
            //hexData[i] = colorToHex(data[i]);

            //rgbData[i] = hexToColor(hexData[i]);          

        }*/

        byte[] receivedColors = new byte[data.Length * 4];
        for (int i = 0, j=0; j < data.Length; i+=3, j++)
        {
            sentColors2[i] = data[j].r;
            sentColors2[i + 1] = data[j].g;
            sentColors2[i + 2] = data[j].b;
           
        }
        ///////////RECIBIR DATOS/////////////     
        /*for (int i = 0, j = 0; i < receivedColors.Length; i+=4, j+=3)
        {
            receivedColors[i] = sentColors[j];
            receivedColors[i + 1] = sentColors[j + 1];
            receivedColors[i + 2] = sentColors[j + 2];
            receivedColors[i + 3] = 255;

        }*/
        for (int i = 0, j = 0; j < sentColors2.Length; i += 4, j += 3)
        {
            receivedColors[i] = sentColors2[j];
            receivedColors[i + 1] = sentColors2[j + 1];
            receivedColors[i + 2] = sentColors2[j + 2];
            receivedColors[i + 3] = 255;

        }
        Texture2D tex = new Texture2D(frontCam.width, frontCam.height);
        tex.LoadRawTextureData(receivedColors);
        tex.Apply();
        photoPlane.texture = tex;
        Debug.Log(data.Length);

    }
    
}
