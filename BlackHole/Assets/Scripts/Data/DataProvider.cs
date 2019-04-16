using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataProvider : MonoBehaviour
{
    public static DataProvider ins;

    // Event to announce the slice of data is ready
    public delegate void SliceReadyHandler(BsonDocument slice);
    public static event SliceReadyHandler OnSliceReady;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else if (ins != this)
        {
            Destroy(gameObject);
            return;
        }
        
    }

    private void OnEnable()
    {
        SimManager.OnSliceNeeded += RequestSlice;
    }

    private void OnDisable()
    {
        SimManager.OnSliceNeeded -= RequestSlice;
    }

    private void RequestSlice(BsonDateTime date)
    {
        StartCoroutine(ProvideSlice(date));
    }

    private IEnumerator ProvideSlice(BsonDateTime date)
    {
        var slice = DataStorage.ins.storage["static"].DeepClone().AsBsonDocument;
        yield return new WaitForSeconds(0.1f);

        var dyn = DataStorage.ins.GetDynamic(date).AsBsonArray;
        foreach (var doc in dyn)
        {
            var symbolName = doc.AsBsonDocument["name"].AsString;
            var hasData = doc.AsBsonDocument["has_data"].AsBoolean;

            slice[symbolName].AsBsonDocument.Add("has_data", hasData);
            if (hasData)
            {
                var data = doc.AsBsonDocument["data"].AsBsonDocument;
                slice[symbolName].AsBsonDocument.Add("open", data["open"]);
                slice[symbolName].AsBsonDocument.Add("close", data["close"]);
                slice[symbolName].AsBsonDocument.Add("high", data["high"]);
                slice[symbolName].AsBsonDocument.Add("low", data["low"]);
                slice[symbolName].AsBsonDocument.Add("volume", data["volume"]);

                slice[symbolName].AsBsonDocument.Add("change", (data["close"].AsDouble / data["open"].AsDouble-1) * 100);
                //slice[symbolName].AsBsonDocument.Add("pe", data["close"].AsDouble / slice[symbolName].AsBsonDocument["ttmEPS"].AsDouble);                              
            }
        }
        Debug.Log("Slice ready: " + slice);
        OnSliceReady(slice);
    }

    

}
