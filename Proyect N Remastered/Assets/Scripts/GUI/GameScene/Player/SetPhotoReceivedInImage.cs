using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SetPhotoReceivedInImage : MonoBehaviour
{
    [SerializeField] private RawImage imageToBeShown;
    [SerializeField] private TextMeshProUGUI killerName;


    public void Start()
    {
        SetImage();
    }

    public void SetImage()
    {
        killerName.text = "You have been killed by " + PersistentData.killer;
        imageToBeShown.texture = PersistentData.killcam;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("FinalMainMenu");
    }

}
