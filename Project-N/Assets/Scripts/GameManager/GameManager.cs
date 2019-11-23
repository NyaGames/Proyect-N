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

	[SerializeField] private Text gmCountDownText;
	[SerializeField] private Text playerDownText;
	private int secsToGameStart;

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

	public void StartGameCountdown(int secs)
	{
		secsToGameStart = secs;
		InvokeRepeating("Countdown", 1f, 1f);
	}

	public void StartGame()
	{
		Debug.Log("Game Started!");
	}

	private void Update()
	{
		if(secsToGameStart != 0)
		{
			string mins = Mathf.FloorToInt(secsToGameStart / 60).ToString();
			if (Mathf.FloorToInt(secsToGameStart / 60) < 10)
			{
				mins = "0" + mins;
			}

			

			string secs = Mathf.FloorToInt(secsToGameStart % 60).ToString();

			if (Mathf.FloorToInt(secsToGameStart % 60) < 10)
			{
				secs = "0" + secs;
			}

			if (PersistentData.isGM)
			{
				gmCountDownText.text = "Game starts in: " + mins + " : " + secs;
			}
			else
			{
				playerDownText.text = "Game starts in: " + mins + " : " + secs;
			}
		}
	}

	private void Countdown()
	{
		secsToGameStart--;
		myPlayer.GetComponent<MessageSender>().SendCountdown(secsToGameStart);
		if(secsToGameStart <= 0)
		{
			CancelInvoke("Countdown");
			StartGame();
		}
	}

	public void OnCountDownReceived(int countDown)
	{
		secsToGameStart = countDown;
	}
}
