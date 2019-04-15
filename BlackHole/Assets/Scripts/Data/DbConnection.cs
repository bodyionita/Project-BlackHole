using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;

using UnityEngine;

public class DbConnection
{
    private readonly string connectionString;
    private IMongoCollection<BsonDocument> staticCollection;
    private IMongoCollection<BsonDocument> dynamicCollection;

    public DbConnection(string connString)
    {
        connectionString = connString;

        MongoClientSettings settings = MongoClientSettings.FromUrl( new MongoUrl(connectionString) );
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

        var client = new MongoClient(settings); 
        var database = client.GetDatabase("blackhole_data");

        staticCollection = database.GetCollection<BsonDocument>("symbols_details");
        dynamicCollection = database.GetCollection<BsonDocument>("symbols_data");
    }

    public async Task<BsonDocument> GetSlice(BsonDateTime date)
    {
        var query = new BsonDocument { { "date", date } };
        var it = await dynamicCollection.FindSync(query).ToListAsync();       
        return it[0];
    }

    public async Task<BsonArray> GetAll()
    {
        var result = new BsonArray();
        await staticCollection.Find(FilterDefinition<BsonDocument>.Empty)
            .ForEachAsync(doc => result.Add(doc));
        return result;
    }

    
}
