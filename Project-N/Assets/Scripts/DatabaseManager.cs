using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public TMPro.TMP_InputField signInUsernameInput;
    public TMPro.TMP_InputField signInPasswordInput;
    public TMPro.TMP_InputField loginUsernameInput;
    public TMPro.TMP_InputField loginPasswordInput;

    private Mongo db;

    private void Start()
    {
        db = new Mongo();
        db.Init();
    }

    public void CreateAccount()
    {
        string username = signInUsernameInput.text;
        string password = signInPasswordInput.text;
        if(username != "" && (password != null && password != ""))
        {
            password = Utility.Sha256FromString(password); //Encriptamos la contraseña
            string email = "urjc@urjc.es";
            db.InsertAccount(username, password, email);
        }
        else
        {
            Debug.Log("Usuario o contraseña no validos");
        }
        
    }
    public void LogIn()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;
        Model_Account myAccount = db.LoginAccount(username, password, 0, "0");
        if(myAccount != null)
        {
            Debug.Log("Te has logeado, " + myAccount.Username + myAccount.Discriminator);
        }
        else
        {
            Debug.Log("Credenciales inválidas");
        }
       
    }

}
