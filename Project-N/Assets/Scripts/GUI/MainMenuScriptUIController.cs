﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScriptUIController : MonoBehaviour
{
    [SerializeField] private Button gmButton;

    private void Awake()
    {
        if (!PersistentData.account.isGameMaster)
        {
            gmButton.interactable = false;
        }
        ChangeGMText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RoomScreen");                
    }

    public void ChangeGMText()
    {
        if (PersistentData.isGM)
        {
            gmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Deactivate GM";
        }
        else
        {
            gmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Activate GM";
        }
    }

    public void ChangeGMStatus()
    {
        PersistentData.isGM = !PersistentData.isGM;
        ChangeGMText();
    }

}
