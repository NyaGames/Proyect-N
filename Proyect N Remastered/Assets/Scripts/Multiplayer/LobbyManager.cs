using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

	[SerializeField] private LobbySceneGUIController playerLobby;
	[SerializeField] private LobbySceneGUIController gmLobby;

	private void Awake()
	{
		if (PersistentData.isGM)
		{
			playerLobby.gameObject.SetActive(false);
			gmLobby.gameObject.SetActive(true);
		}
		else
		{
			playerLobby.gameObject.SetActive(true);
			gmLobby.gameObject.SetActive(false);
		}
	}
	// Start is called before the first frame update
	void Start()
    {

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /*private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) //Si no soy gm, compruebo si este ha llegado a la sala de juego para cargarla yo
        {
            ExitGames.Client.Photon.Hashtable table = PhotonNetwork.MasterClient.CustomProperties;
            if(table.ContainsKey("Ready"))
            {
                SceneManager.LoadScene("FinalGameScene");
            }

        }
    }*/

    public void ReturnToRoomScene()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("FinalRoomScene");
        Debug.Log("Dejaste la sala");
    }
}
