using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public TMPro.TMP_InputField SignInUsernameInputText;
    public TMPro.TMP_InputField SignInPasswordInputText;
    public TMPro.TMP_InputField SignInRepeatPasswordInputText;

    private Mongo db;

    void Start()
    {
        db = new Mongo();
        db.Init();
    }

    public void InsertNewAccount()
    {
        string username = SignInUsernameInputText.text;
        string password = SignInPasswordInputText.text;
        string passwordRepeated = SignInRepeatPasswordInputText.text;
        if (password.Equals(passwordRepeated)) //Si ambas contraseñas están bien escritas, se intenta insertar
        {
            Model_Account userAccount = db.InsertAccount(username, password);
            if (userAccount != null)
            {
                Debug.Log("New account created, your username is: " + userAccount.Username +"#"+ userAccount.Discriminator + ". Use it to login!");
            }
            else
            {
                Debug.Log("Cannot create the account ");
            }
        }
        else{
            Debug.Log("Passwords mismatched, write it again!");
        }
        
    }

}
