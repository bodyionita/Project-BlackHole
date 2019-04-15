using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataStorage : MonoBehaviour
{
    private BsonDocument storage;
    public BsonArray symbolNames { get; private set; }
    public BsonArray datesStored { get; private set; }

    // Event to announce when the static data of all symbols has been stored
    public delegate void StaticDataStored();
    public static event StaticDataStored OnStaticDataStored;

    // Event to announce when the dynamic data of all symbols for one date has been stored
    public delegate void DynamicDataStored(BsonDateTime date);
    public static event DynamicDataStored OnDynamicDataStored;


    void Awake()
    {
        storage = new BsonDocument
            {
                { "static",new BsonDocument()},
                { "dynamic",new BsonDocument()}
            };

        symbolNames = new BsonArray();
        datesStored = new BsonArray();
    }

    public IEnumerator AddStatic(BsonArray data)
    {
        foreach (var symbol in data)
        {
            var name = symbol.AsBsonDocument["symbol"].AsString;
            symbolNames.Add(name);

            storage["static"].AsBsonDocument.Add(name, symbol);
            yield return new WaitForSeconds(0.2f);

            storage["dynamic"].AsBsonDocument.Add(name, new BsonDocument());
            yield return new WaitForSeconds(0.2f);
        }
        if (OnStaticDataStored != null) OnStaticDataStored();
    }

    public IEnumerator AddSlice(BsonDocument data, BsonDateTime date)
    {
        var symbols = data["symbols"].AsBsonArray;
        var dateString = date.ToUniversalTime().ToShortDateString();
        foreach (var symbol in symbols)
        {
            var name = symbol.AsBsonDocument["name"].AsString;
            var has_data = symbol.AsBsonDocument["has_data"].AsBoolean;
            if (has_data)
            {
                var one_day_slice = symbol.AsBsonDocument["data"].AsBsonDocument;
                var symbol_data = storage["dynamic"].AsBsonDocument;

                symbol_data[name].AsBsonDocument.Add(dateString, one_day_slice);
                yield return new WaitForSeconds(0.5f);
            }
        }
        datesStored.Add(date);
        if (OnDynamicDataStored != null) OnDynamicDataStored(date);
    }

    public BsonDocument GetSymbolStatic(string symbol)
    {
        return storage["static"].AsBsonDocument[symbol].AsBsonDocument;
    }

    public BsonDocument GetSymbolDynamic(string symbol, BsonDateTime date)
    {
        return storage["dynamic"].AsBsonDocument[symbol].AsBsonDocument[date.AsString].AsBsonDocument;
    }

    
}
