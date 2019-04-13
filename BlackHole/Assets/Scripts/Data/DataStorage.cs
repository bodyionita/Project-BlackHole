using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataStorage : MonoBehaviour
{
    private BsonDocument storage;
    public BsonArray symbolNames { get; private set; }
    public BsonDocument dateRange { get; private set; }

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

        dateRange = new BsonDocument
            {
                {"startDate", new BsonDateTime(new DateTime(2014,1,15))},
                {"endDate", new BsonDateTime(new DateTime(2019,1,14))}
            };
    }

    public void AddStatic(BsonArray data)
    {
        foreach (var symbol in data)
        {
            var name = symbol.AsBsonDocument["symbol"].AsString;
            symbolNames.Add(name);

            storage["dynamic"].AsBsonDocument.Add(name, symbol.DeepClone());
        }

        if (OnStaticDataStored != null) OnStaticDataStored();
    }

    public void AddSlice(BsonDocument data, BsonDateTime date)
    {
        var symbols = data["symbols"].AsBsonArray;
        foreach (var symbol in symbols)
        {
            var name = symbol.AsBsonDocument["name"].AsString;
            var has_data = symbol.AsBsonDocument["has_data"].AsBoolean;
            if (has_data)
            {
                var one_day_slice = symbol.AsBsonDocument["data"].AsBsonDocument;
                storage["dynamic"].AsBsonDocument[name].AsBsonDocument.Add(date.AsString, data.DeepClone());
            }
        }

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

    public IEnumerable<BsonDateTime> EachDay(BsonDateTime from, BsonDateTime thru)
    {
        for (var day = from.ToUniversalTime().Date; day.Date <= thru.ToUniversalTime().Date; day = day.AddDays(1))
            yield return new BsonDateTime(day);
    }
}
