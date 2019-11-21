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

 
}
