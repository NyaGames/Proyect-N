using MongoDB.Driver;
using System.Collections.Generic;
using UnityEngine;
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
    public Model_Account InsertAccount(string username,string password)
    {
        Model_Account newAccount = null;
        //Checkea si el username es valido
        if (!Utility.IsUsername(username))
        {
            Debug.Log(username + "is not an username");
            return newAccount;
        }

        //Checkea si la cuenta existe
        newAccount = FindAccountByUsername(username);
        if (newAccount.Discriminator == "failed")
        {
            Debug.Log(username + "has no discriminator avaiable, please change your username");
            return newAccount;
        }
        newAccount.ShaPassword = Utility.Sha256FromString(password);
        newAccount.isGameMaster = false;
        newAccount.LastLogin = System.DateTime.Now;
        //newAccount.Email = email;
        accounts.InsertOne(newAccount);
        //accounts.Insert(newAccount);

        return newAccount;
    }

    public Model_Account LoginAccount(string usernameOrEmail, string password, int cnnId, string token)
    {
        Model_Account myAccount = null;
        string[] data = usernameOrEmail.Split('#');

        //Find my account
        if (!Utility.isEmail(usernameOrEmail)) //Si me he loggeado usando el username#Discriminator
        {
            if (data[1] != null) //Si existe el Discriminator,continuamos 
            {
                myAccount = accounts.Find(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password)).SingleOrDefault();
            }
        }

        if(myAccount != null) //Si hemos encontrado la cuenta
        {
                accounts.UpdateOne(u => u.Username.Equals(data[0]) && u.Discriminator.Equals(data[1]) && u.ShaPassword.Equals(password), Builders<Model_Account>.Update.Set(u => u.LastLogin, System.DateTime.Now));
        }
        else
        {
            //No hemos encontrado la cuenta
        }

        return myAccount;
    }

    #endregion

    #region Fetch
    public Model_Account FindAccountByUsername(string username)
    {
        List<Model_Account> usersList = accounts.Find(user => user.Username.Equals(username)).ToList();
        //Generar Discriminator aleatorio
        Model_Account newAccount = new Model_Account();
        newAccount.Username = username;
        newAccount.Discriminator = "0000";
        int rollCount = 0;
        while (FindAccountByUsernameAndDiscriminator(newAccount.Username, newAccount.Discriminator) != null) //Mientras haya alguien con ese Discriminator, creamos uno nuevo y volvemos a buscar
        {
            newAccount.Discriminator = Random.Range(0, 9999).ToString("0000");
            rollCount++;
            if (rollCount > 1000)
            {
                Debug.Log("We rolled to many time, suggest username change!");
                newAccount.Discriminator = "failed";
                return newAccount;
            }
        }

        return newAccount;
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
