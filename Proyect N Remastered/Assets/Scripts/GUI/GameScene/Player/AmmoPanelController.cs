using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanelController : MonoBehaviour
{
    private Player myPlayer;
    public TMPro.TextMeshProUGUI ammoText;

	[SerializeField] private Button photoButton;

    void Start()
    {
        myPlayer = GameManager.Instance.myPlayer.GetComponent<Player>();
    }

   
    void Update()
    {
        if(myPlayer.currentAmmo <= 0)
        {
			photoButton.interactable = false;
            ammoText.text = "You are out of ammo! Try to find some drops";
        }
        else
        {
            ammoText.text = myPlayer.currentAmmo + "/" + myPlayer.maxAmmo;
			photoButton.interactable = true;
        }
    }
}
