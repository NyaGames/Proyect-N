using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ReturnToRoomScene()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("RoomScreen");
        Debug.Log("Dejaste la sala");
    }
}
