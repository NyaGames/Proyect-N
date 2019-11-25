using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject myPlayer { get; private set; }
    public bool gameStarted { get; private set; }

	[SerializeField] private Text gmCountDownText;
	[SerializeField] private Text playerCountDownText;
	[SerializeField] private Button playerPhotoButton;

	private int secsToGameStart;
    private string countDownText;
    private bool countdownActive = false;

    UnityAction onCountDownFinished;

    public Text actorNumberText;

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
        gameStarted = false;
    }

	public void SetCountDown(int secs, string countDownText, UnityAction onCountDownFinished)
	{
        if (!countdownActive)
        {
            this.onCountDownFinished += onCountDownFinished;
            secsToGameStart = secs;
            this.countDownText = countDownText;
            countdownActive = true;
            InvokeRepeating("Countdown", 1f, 1f);
        }
    
	}

	public void StartGame()
	{
		Debug.Log("Game Started!");
        gameStarted = true;

        if (!PersistentData.isGM)
        {
            playerPhotoButton.interactable = true;
            playerCountDownText.text = "";
        }
        else
        {
            gmCountDownText.text = "";
        }
	}

	private void Update()
	{
        if(myPlayer != null)
        {
            actorNumberText.text = "Actor number: " + PhotonNetwork.LocalPlayer.ActorNumber + "/ id:" + myPlayer.GetComponent<Player>().id;
        }

        if (secsToGameStart != 0)
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
				gmCountDownText.text = countDownText + ": " + mins + " : " + secs;
			}
			else
			{
				playerCountDownText.text = countDownText + ": " + mins + " : " + secs;
			}
		}
	}

	private void Countdown()
	{
		secsToGameStart--;
		myPlayer.GetComponent<MessageSender>().SendCountdown(secsToGameStart, "ReceiveGameStartCountdown");
		if(secsToGameStart <= 0)
		{
			CancelInvoke("Countdown");
            onCountDownFinished();
            countdownActive = false;
		}
	}

	public void OnCountDownReceived(int countDown)
	{
		secsToGameStart = countDown;
	}

    public void CloseZone()
    {
        GamemasterManager.Instance.StartClosingZone();
        Debug.Log("Closing zone");
    }
}
