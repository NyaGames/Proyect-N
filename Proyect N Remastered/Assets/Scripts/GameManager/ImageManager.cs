using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance { get; private set; }

    public List<byte[]> imagesList;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            imagesList = new List<byte[]>();
        }
        else
        {
            Destroy(Instance);
        }
    }



}
