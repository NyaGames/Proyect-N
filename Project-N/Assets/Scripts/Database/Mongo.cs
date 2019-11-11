using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;

public class Mongo
{
    private const string MONGO_URI = "mongodb+srv://Chetok:123ddd456@lobbydb-tqpcq.mongodb.net/test?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "lobbydb";

    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<Model_Account> accounts;



    public void Init()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        accounts = db.GetCollection<Model_Account>("account");
        Debug.Log("Database has been initiliazed!");
    }
    
    public void ShutDown()
    {
        client = null;
        db = null;
    }

   

    #region Insert
    public bool InsertAccount(string username,string password,string email)
    {
        //Checkea si el email es valido
        if (!Utility.isEmail(email))
        {
            Debug.Log(email + "is not an email");
            return false;
        }
        //Checkea si el username es valido
        if (!Utility.IsUsername(username))
        {
            Debug.Log(username + "is not an username");
            return false;
        }
        //Checkea si la cuenta existe
        if (FindAccountByEmail(email) != null)
        {
            Debug.Log(email + "is already in use");
            return false;
        }

        Model_Account newAccount = new Model_Account();
        newAccount.Username = username;
        newAccount.ShaPassword = password;
        newAccount.Email = email;
        newAccount.Discriminator = "0000";

        //Generar Discriminator aleatorio
        int rollCount = 0;
        while(FindAccountByUsernameAndDiscriminator(newAccount.Username,newAccount.Discriminator) != null) //Mientras haya alguien con ese Discriminator, creamos uno nuevo y volvemos a buscar

        {
            newAccount.Discriminator = Random.Range(0, 9999).ToString("0000");

            rollCount++;
            if(rollCount > 1000)
            {
                Debug.Log("We rolled to many time, suggest username change!");
                return false;
            }
        }

        accounts.InsertOne(newAccount);
        Debug.Log("Nueva cuenta creada");
        //accounts.Insert(newAccount);

        return true;
    }

    public Model_Account LoginAccount(string usernameOrEmail, string password, int cnnId, string token)
    {
        Model_Account myAccount = null;
        bool emailUsed = false;
        string[] data = usernameOrEmail.Split('#');

        //Find my account
        if (Utility.isEmail(usernameOrEmail)) //Si me he loggeado usando el email 
        {
            myAccount = accounts.Find(u => u.Email.Equals(usernameOrEmail) && u.ShaPassword.Equals(password)).SingleOrDefault();
            emailUsed = true;
        }
        else //Si me he loggeado usando el username#Discriminator
        {
            if (data.Length > 1) //Si existe el Discriminator,continuamos 
            {
                myAccount = accounts.Find(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password)).SingleOrDefault();
            }
            else
            {
                return null;
            }
        }

        if(myAccount != null) //Si hemos encontrado la cuenta
        {
           /* myAccount.ActiveConnection = cnnId;
            myAccount.Token = token;
            myAccount.Status = 1; //Status indica si eta online(1) o no(0)
            myAccount.LastLogin = System.DateTime.Now;*/

            if (emailUsed) //Si me he logeado con el email
            {
                accounts.UpdateOne(u => u.Email.Equals(usernameOrEmail) && u.ShaPassword.Equals(password),Builders<Model_Account>.Update.Set(u => u.ActiveConnection, cnnId));
                accounts.UpdateOne(u => u.Email.Equals(usernameOrEmail) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.Token, token));
                accounts.UpdateOne(u => u.Email.Equals(usernameOrEmail) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.Status, 1));
                accounts.UpdateOne(u => u.Email.Equals(usernameOrEmail) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.LastLogin, System.DateTime.Now));
            }
            else //Si me he logeado con Username#Discriminator
            {
                accounts.UpdateOne(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.ActiveConnection, cnnId));
                accounts.UpdateOne(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.Token, token));
                accounts.UpdateOne(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.Status, 1));
                accounts.UpdateOne(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.LastLogin, System.DateTime.Now));
            }
            
        }
        else
        {
            //No hemos encontrado la cuenta
            Debug.Log("No se ha encontrado la cuenta");
        }

        return myAccount;
    }

    #endregion

    #region Fetch
    public Model_Account FindAccountByEmail(string email)
    {
        Model_Account modelUser = accounts.Find(user => user.Email.Equals(email)).SingleOrDefault(); //Cojo un documento que cumpla la condición(el email es igual al string que le paso)
        return modelUser;
    }
    public Model_Account FindAccountByUsernameAndDiscriminator(string username,string discriminator)
    {
        Model_Account userMA = null;
        List<Model_Account> usersList = accounts.Find(user => user.Username.Equals(username)).ToList();
        foreach(Model_Account u in usersList)
        {
            if(u.Discriminator == discriminator)
            {
                userMA = u;
                return userMA;
            }
        }

        return userMA;
    }
    #endregion

    #region Update

    #endregion

    #region Delete

    #endregion
}
