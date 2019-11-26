using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPhotoReceivedInImage : MonoBehaviour
{
    [SerializeField] private RawImage imageToBeShown;

    public void Start()
    {
        SetImage();
    }

    public void SetImage()
    {
        imageToBeShown.texture = KillCamImageInfo.killcamImage;
    }
}
