using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanelController : MonoBehaviour
{
    private AmmoInfo myPlayerAmmoinfo;
    public TMPro.TextMeshProUGUI ammoText;

	[SerializeField] private Button photoButton;

    void Start()
    {
        myPlayerAmmoinfo = GameManager.Instance.myPlayer.GetComponent<AmmoInfo>();
    }

   
    void Update()
    {
        if(myPlayerAmmoinfo.currentAmmo <= 0)
        {
			photoButton.interactable = false;
            ammoText.text = "You are out of ammo! Try to find some drops";
        }
        else
        {
            ammoText.text = myPlayerAmmoinfo.currentAmmo.ToString();
			photoButton.interactable = true;
        }
    }
}
