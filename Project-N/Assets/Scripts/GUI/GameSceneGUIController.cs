using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneGUIController : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject GMPanel;
    [SerializeField] private GameObject playerPanel;

    private bool GM;

    private void Start()
    {
        GM = PersistentData.isGM;

        AdjustCamera();
        AdjustOrientation();
        AdjustGUI();
    }

    private void AdjustCamera()
    {
        if (GM) {            
            camera.rect = new Rect(0f, 0f, 0.66f, 1f);
        }
        else
        {
            camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }

    private void AdjustOrientation()
    {
        if (GM)
        {
            DeviceOrientation.Instance.ChangeOrientation(true);
        }
        else
        {
            DeviceOrientation.Instance.ChangeOrientation(false);
        }     
    }

    private void AdjustGUI()
    {
        if (GM)
        {
            GMPanel.SetActive(true);
            playerPanel.SetActive(false);
        }
        else
        {
            GMPanel.SetActive(false);
            playerPanel.SetActive(true);
        }
    }
}
