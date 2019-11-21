using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScriptUIController : MonoBehaviour
{
    [SerializeField] private Button gmButton;

    private void Awake()
    {
        if (!PersistentData.isGM)
        {
            gmButton.interactable = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RoomScreen");                
    }

    public void ChangeGMStatus()
    {
        PersistentData.isGM = !PersistentData.isGM;
    }

}
