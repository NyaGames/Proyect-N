using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatScene : MonoBehaviour
{
    private MusicController musicController;

    private void Awake()
    {
        musicController = FindObjectOfType<MusicController>();
        musicController.Defeat();
    }

    public void returnToMainMenu() {
        musicController.returnToMainMenu();
    }
}
