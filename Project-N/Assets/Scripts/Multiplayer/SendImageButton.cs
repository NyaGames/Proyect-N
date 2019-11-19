using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendImageButton : MonoBehaviour
{
    GameObject player;

    public void Update()
    {
        if(AutoLobby.Instance.myPlayer != null)
        {
            player = AutoLobby.Instance.myPlayer;
        }
        
    }

    public void sendImage()
    {
        if (!player.GetComponent<Player>().isGameMaster)
        {
            player.GetComponent<ImageManager>().SendImageToMaster();
        }
    }

}
