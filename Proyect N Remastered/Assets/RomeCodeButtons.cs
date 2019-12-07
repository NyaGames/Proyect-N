using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RomeCodeButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputText;

    public void AddToCode(string s)
    {
        inputText.text += s;             
    }

    public void Clear()
    {
        inputText.text = "";
    }
}
