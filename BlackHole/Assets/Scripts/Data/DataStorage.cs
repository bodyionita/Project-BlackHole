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

    public static double marketcapDiv = Mathf.Pow(10, 6);
    [HideInInspector]
    public static float marketcapMin = (float)(Mathf.Pow(10, 8) / marketcapDiv);
    [HideInInspector]
    public static float marketcapMax = (float)(2 * Mathf.Pow(10, 12) / marketcapDiv);
    [HideInInspector]
    public static float epsMin = -2f;
    [HideInInspector]
    public static float epsMax = 10f;
    [HideInInspector]
    public static float peMin = -10f;
    [HideInInspector]
    public static float peMax = 40f;
    [HideInInspector]
    public static float volumeMin = 0f;
    [HideInInspector]
    public static float volumeMax = Mathf.Pow(10,5);
    [HideInInspector]
    public static float changeMin = -5f;
    [HideInInspector]
    public static float changeMax = 5f;

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
