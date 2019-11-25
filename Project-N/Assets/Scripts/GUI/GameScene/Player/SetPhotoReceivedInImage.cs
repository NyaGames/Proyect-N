using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPhotoReceivedInImage : MonoBehaviour
{
    [SerializeField] private RawImage imageToBeShown;

    public void SetImage(Texture2D killCam)
    {
        imageToBeShown.texture = killCam;
    }
}
