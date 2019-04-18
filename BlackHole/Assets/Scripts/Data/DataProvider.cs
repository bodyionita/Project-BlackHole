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

                // volume to price ratio
                slice[symbolName].AsBsonDocument.Add("vtp", (double)data["volume"].ToInt64() / data["close"].ToDouble());

                // change in price
                slice[symbolName].AsBsonDocument.Add("change", (data["close"].AsDouble / data["open"].AsDouble -1 ) * 100);


                // eps and pe ratio
                string usedEPS;
                double eps;                
                double latestEPS = slice[symbolName].AsBsonDocument["latestEPS"].ToDouble();
                double ttmEPS = slice[symbolName].AsBsonDocument["ttmEPS"].ToDouble();
                if (latestEPS != 0) { eps = latestEPS; usedEPS = "latestEPS"; }
                else { eps = ttmEPS; usedEPS = "ttmEPS"; }

                double pe = 0;
                if (eps != 0) pe = data["close"].AsDouble / eps;

                slice[symbolName].AsBsonDocument.Add("usedEPS", usedEPS);
                slice[symbolName].AsBsonDocument.Add("eps", eps);
                slice[symbolName].AsBsonDocument.Add("pe", pe);                              
            }
            else
            {
                //Debug.Log("Provider has found no data for: " + symbolName);
            }
        }
        OnSliceReady(slice);
    }

    

}
