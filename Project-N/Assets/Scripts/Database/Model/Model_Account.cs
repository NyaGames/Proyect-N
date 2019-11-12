
using MongoDB.Bson;
using System;

public class Model_Account
{

    public ObjectId _id;

    //public int ActiveConnection { set; get; } //Conexión con la base de datos
    public string Username { set; get; }
    public string Discriminator { set; get; }
    //public string Email { set; get; }
    public string ShaPassword { set; get; }
    public bool isGameMaster { set; get; }

    //public byte Status { set; get; } //Estás o no online
    //public string Token { set; get; }
    public DateTime LastLogin;  //última vez que se conectó

}
