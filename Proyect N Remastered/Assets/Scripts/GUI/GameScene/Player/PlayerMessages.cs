using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class PlayerMessages : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    private List<PlayerMessage> messages = new List<PlayerMessage>();

    private PlayerMessage currentMessage;


    private void Update()
    {
        if (messages.Count <= 0)
        {
            gameObject.SetActive(false);
        }       
    }

    public void AddMessage(PlayerMessage newMessage)
    {      

        if(messages.Count == 0)
        {
            messages.Add(newMessage);
            ShowMessage();
        }
        else
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if(i == messages.Count - 1)
                {
                    if(messages[i].preference <= newMessage.preference)
                    {
                        messages.Add(newMessage);
                        return;
                    }
                }
                else
                {
                    if (messages[i].preference <= newMessage.preference)
                    {
                        if(messages[i + 1].preference > newMessage.preference)
                        {
                            messages.Insert(i + 1, newMessage);
                            return;
                        }
                    }
                }              
            }
        }
    }

   

    private void ShowMessage()
    {
        currentMessage = messages[0];
        messageText.text = currentMessage.message;
        Invoke("NextMesage", currentMessage.duration);
    }

    private void NextMesage()
    {
        messages.Remove(currentMessage);
        if(messages.Count > 0)
        {
            ShowMessage();
        }
    }
}

public struct PlayerMessage
{
    public string message;
    public int preference;
    public float duration;

    public PlayerMessage(string message, int preference, float duration)
    {
        this.message = message;
        this.preference = preference;
        this.duration = duration;
    }
}
