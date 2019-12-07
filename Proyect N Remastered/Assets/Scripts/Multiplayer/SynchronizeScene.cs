using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SynchronizeScene : MonoBehaviour
{
    [PunRPC]
    public void StartGame()
    {
        SceneManager.LoadScene("FinalGameScene");
    } 
}
