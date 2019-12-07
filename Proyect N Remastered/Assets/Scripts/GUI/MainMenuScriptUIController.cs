using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScriptUIController : MonoBehaviour
{
    [SerializeField] private Button gmButton;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject gmPanel;

    private void Awake()
    {
        /*if (!PersistentData.account.isGameMaster)
        {
            gmButton.interactable = false;
        }*/

		if(Application.platform == RuntimePlatform.WebGLPlayer)
		{
			gmButton.interactable = false;
		}

        ChangeGMText();
    }

    public void StartGame()
    {
        ConnectToPhoton.Instance.Connect();
        //SceneManager.LoadScene("RoomScreen");                
    }

    public void ChangeGMText()
    {
        if (PersistentData.isGM)
        {
			playerPanel.SetActive(false);
			gmPanel.SetActive(true);
		}
        else
        {
			playerPanel.SetActive(true);
			gmPanel.SetActive(false);
		}
    }

    public void ChangeGMStatus()
    {
        PersistentData.isGM = !PersistentData.isGM;
        ChangeGMText();
    }

}
