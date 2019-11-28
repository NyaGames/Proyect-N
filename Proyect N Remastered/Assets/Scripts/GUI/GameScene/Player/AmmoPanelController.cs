using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPanelController : MonoBehaviour
{
    private Player myPlayer;
    public TMPro.TextMeshProUGUI ammoText;

    void Start()
    {
        myPlayer = GameManager.Instance.myPlayer.GetComponent<Player>();
    }

   
    void Update()
    {
        if(myPlayer.currentAmmo <= 0)
        {
            PhotosPanelGUIController.Instance.takePhotoButton.interactable = false;
            ammoText.text = "You are out of ammo! Try to find some drops";
        }
        else
        {
            ammoText.text = myPlayer.currentAmmo + "/" + myPlayer.maxAmmo;
            PhotosPanelGUIController.Instance.takePhotoButton.interactable = true;
        }
    }
}
