using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public TMPro.TMP_InputField SignInUsernameInputText;
    public TMPro.TMP_InputField SignInPasswordInputText;
    public TMPro.TMP_InputField SignInRepeatPasswordInputText;

    public TMPro.TMP_InputField LogInInUsernameInputText;
    public TMPro.TMP_InputField LogInPasswordInputText;

    private Mongo db;
    private Model_Account userAccount;

    void Start()
    {
        db = new Mongo();
        db.Init();
    }
    //Inserta una nueva cuenta en la base de datos
    public void InsertNewAccount()
    {
        string username = SignInUsernameInputText.text;
        string password = SignInPasswordInputText.text;
        string passwordRepeated = SignInRepeatPasswordInputText.text;
        if (password.Equals(passwordRepeated)) //Si ambas contraseñas están bien escritas, se intenta insertar
        {
            userAccount = db.InsertAccount(username, password);
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

    public void LogIn()
    {
        string username = LogInInUsernameInputText.text;
        string password = Utility.Sha256FromString(LogInPasswordInputText.text);

        userAccount = db.LoginAccount(username,password);
        if(userAccount != null) //SI hemos encontrado la cuenta
        {
            Debug.Log("Login as: " + userAccount.Username + "#" + userAccount.Discriminator + ". Welcome back!");
        }
        else
        {
            Debug.Log("Wrong username or password, please try to log in with other credentials");
        }

    }

}
