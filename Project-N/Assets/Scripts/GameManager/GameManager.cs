using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject myPlayer { get; private set; }

    public GameObject[] playersViewsList;

    // Start is called before the first frame update
    void Awake()
    {
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

        myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
       
    }

    public void Start()
    {
        playersViewsList = GameObject.FindGameObjectsWithTag("Player");
        SetGroups();
    }

    public void SetGroups()
    {
        byte gmGroup = 255;
        if (myPlayer.GetComponent<Player>().isGameMaster) //Si soy gm me suscribo a todos los canales de los jugadores y al del gm
        {
           
            byte[] enableGroups = new byte[playersViewsList.Length + 1]; //Todos los jugadores  + canal gm
            for(byte i = 1;i <= playersViewsList.Length; i++)
            {
                enableGroups[i] = i;
            }
            enableGroups[enableGroups.Length - 1] = gmGroup;
            PhotonNetwork.SetInterestGroups(new byte[0], enableGroups);
        }
        else //Si soy jugador normal solo estoy suscrito al canal del gm
        {
            PhotonNetwork.SetInterestGroups(gmGroup, true);
        }
    }

 
}
