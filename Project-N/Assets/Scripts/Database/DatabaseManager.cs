using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public TMPro.TMP_InputField SignInUsernameInputText;
    public TMPro.TMP_InputField SignInPasswordInputText;
    public TMPro.TMP_InputField SignInRepeatPasswordInputText;

    public TMPro.TMP_InputField LogInUsernameInputText;
    public TMPro.TMP_InputField LogInPasswordInputText;

    public GameObject LoginError;
    public GameObject SignInPasswordError;
    public GameObject SignInUsernameError;

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
                Debug.LogError("MongoDB: Cannot create the account");
                SignInUsernameError.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("MongoDB: Password mismatched. Write it again!");
            SignInPasswordError.SetActive(true);
        }
        
    }

    public void LogIn() {
        string username = LogInUsernameInputText.text;
        string password = LogInPasswordInputText.text;
        Model_Account userAccount = db.FindAccountByUsername(username);
        if (userAccount != null)
        {
            if (userAccount.ShaPassword.Equals(password))
            {
                Debug.Log("Letsgoooooooooooooo");
            }
            else
            {
                Debug.LogError("MongoDB: Password not valid!");
                LoginError.SetActive(false);
            }
        }
        else {
            Debug.LogError("MongoDB: Username not valid!");
            LoginError.SetActive(false);
        }
    }
}
