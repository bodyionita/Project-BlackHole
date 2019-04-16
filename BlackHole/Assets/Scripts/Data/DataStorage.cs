using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
using System.Linq;

public class DataStorage : MonoBehaviour
{
    public static DataStorage ins;

    public BsonDocument storage;
    public int numberOfStocks;
    public BsonArray symbolNames { get; private set; }
    public BsonArray datesStored { get; private set; }

    public long marketcapMin = 0;
    public long marketcapMax = 21000000000000;

    public float epsMin = -2f;
    public float epsMax = 50f;

    public float peMin = 0f;
    public float peMax = 200f;

    public float changeMin = -50f;
    public float changeMax = 50f;

    // Event to announce when the static data of all symbols has been stored
    public delegate void StaticDataStored();
    public static event StaticDataStored OnStaticDataStored;

    // Event to announce when the dynamic data of all symbols for one date has been stored
    public delegate void DynamicDataStored(BsonDateTime date);
    public static event DynamicDataStored OnDynamicDataStored;


    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }

        storage = new BsonDocument
            {
                { "static",new BsonDocument()},
                { "dynamic",new BsonDocument()}
            };

        symbolNames = new BsonArray();
        datesStored = new BsonArray();
    }

    public void AddStatic(BsonArray data)
    {
        foreach (var symbol in data)
        {
            var name = symbol.AsBsonDocument["symbol"].AsString;
            symbolNames.Add(name);

            storage["static"].AsBsonDocument.Add(name, symbol);
        }
        numberOfStocks = symbolNames.Count();
        if (OnStaticDataStored != null) OnStaticDataStored();
    }

    public void AddSlice(BsonDocument data, BsonDateTime date)
    {
        var symbols = data["symbols"].AsBsonArray;
        var dateString = date.ToUniversalTime().ToShortDateString();

        storage["dynamic"].AsBsonDocument.Add(dateString, symbols);

        datesStored.Add(date);
        if (OnDynamicDataStored != null) OnDynamicDataStored(date);
    }

    public BsonDocument GetSymbolStatic(string symbol)
    {
        return storage["static"].AsBsonDocument[symbol].AsBsonDocument;
    }

    public BsonArray GetDynamic(BsonDateTime date)
    {
        var dateString = date.ToUniversalTime().ToShortDateString();
        return storage["dynamic"].AsBsonDocument[dateString].AsBsonArray;
    }

    
}
