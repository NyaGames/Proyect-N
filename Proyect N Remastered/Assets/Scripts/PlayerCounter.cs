using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerCounter : MonoBehaviour
{
    private TextMeshProUGUI counter;

    private void Awake()
    {
        counter = GetComponent<TextMeshProUGUI>();

        InvokeRepeating("UpdateText", 0f, 1f);

    }

    private void UpdateText()
    {
        counter.text = (PhotonNetwork.CurrentRoom.PlayerCount - 1).ToString();
    }
}
