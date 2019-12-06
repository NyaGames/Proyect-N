using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject myPlayer { get; private set; }
    public bool gameStarted { get; private set; }

	[SerializeField] private TextMeshProUGUI gmCountDownText;
	[SerializeField] private TextMeshProUGUI playerCountDownText;
	[SerializeField] private Button playerPhotoButton;

	private int secsToGameStart;
    private string countDownText;
    private bool countdownActive = false;

    UnityAction onCountDownFinished;

    public GameObject outOfZonePanel;

    public GameObject winningPanel;
    [HideInInspector] public int playerCountAtStart;

	private void Awake()
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
        playerCountAtStart = PhotonNetwork.CurrentRoom.PlayerCount - 1;

    }


	public void StartGame()
	{
		gameStarted = true;

		//GameSceneGUIController.Instance.playerMessages.gameObject.SetActive(false);

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

        if (outOfZonePanel.activeSelf && myPlayer != null && !PhotonNetwork.LocalPlayer.IsMasterClient) {
            outOfZonePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "You have " + myPlayer.GetComponent<OutOfZoneInfo>().currentSecsOutOfZone + " seconds to return to game area! Run now!";
        }



	}

    public void CloseZone()
    {
        GamemasterManager.Instance.StartClosingZone();
        Debug.Log("Closing zone");
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("FinalMainMenu");
        Debug.Log("Dejaste la partida");
    }
}
