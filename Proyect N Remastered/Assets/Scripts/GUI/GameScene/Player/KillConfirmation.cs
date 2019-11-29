using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillConfirmation : MonoBehaviour
{

    public Text usernameKilled;

    public void SetPlayerKilled(string playerKilled,int numKills)
    {
        usernameKilled.text = playerKilled + ". You have killed " + numKills + " people!";
    }
}
