using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class CameraWebPhoto : MonoBehaviour
{
    private bool camAvailable;
    WebCamTexture webCamTexture;
    WebCamTexture frontCam;
    private Texture defaultBackground;

    public RawImage background;
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

    public void TakePhoto()
    {
        Texture2D photo = new Texture2D(frontCam.width, frontCam.height);
        photo.SetPixels(frontCam.GetPixels());
        photo.Apply();
        Debug.Log("Photo!!");

    }
    /*IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:

        
        
        //plano2.GetComponent<Renderer>().material.mainTexture = photo;
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "photo.png");
        File.WriteAllBytes("photo.png", bytes);
        Debug.Log("nada");

    }*/
}
