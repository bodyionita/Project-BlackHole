using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;


public class DbConnection : MonoBehaviour
{
    public string connectionString;

    private MongoClient client;
    private MongoDatabase database;
    private MongoCollection staticCollection, dynamicCollection;

    void Start()
    {
        client = new MongoClient(connectionString);
        database = client.GetServer().GetDatabase("project-blackhole");
        staticCollection = database.GetCollection("symbols-details");
        dynamicCollection = database.GetCollection("symbols-data");
    }

    
}
