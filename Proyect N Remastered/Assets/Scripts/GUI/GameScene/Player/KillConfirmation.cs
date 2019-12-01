using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillConfirmation : MonoBehaviour
{
    public TextMeshProUGUI usernameKilled;

    public void SetPlayerKilled(string playerKilled,int numKills)
    {
        usernameKilled.text = playerKilled + ". You have killed " + numKills + " people!";
    }
}
