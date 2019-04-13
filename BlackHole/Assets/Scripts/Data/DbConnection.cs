using System.Collections.Generic;
using System.Security.Authentication;

using MongoDB.Bson;
using MongoDB.Driver;


public class DbConnection
{
    private string connectionString;
    private MongoCollection<BsonDocument> staticCollection;
    private MongoCollection<BsonDocument> dynamicCollection;

    public DbConnection(string connString)
    {
        connectionString = connString;

        MongoClientSettings settings = MongoClientSettings.FromUrl( new MongoUrl(connectionString) );
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

        var client = new MongoClient(settings);
        var database = client.GetServer().GetDatabase("blackhole_data");

        staticCollection = database.GetCollection("symbols_details");
        dynamicCollection = database.GetCollection("symbols_data");
    }

    public BsonDocument GetSlice(BsonDateTime date)
    {
        var query = dynamicCollection.Find(new QueryDocument("date", date));
        var result = new BsonDocument();
        foreach (var doc in query)
        {
            result = doc;
        }
        return result;
    }

    public BsonArray GetAll()
    {
        var result = new BsonArray();

        foreach (var doc in staticCollection.FindAll())
        {
            result.Add(doc);
        }
        return result;
    }

    
}
