using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataManager : MonoBehaviour
{
    public static DataManager ins;

    private DataStreamer dataStreamer;
    private DataStorage dataStorage;

    public BsonDocument dateRange { get; private set; }

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ins != this)
        {
            Destroy(gameObject);
        }
        StreamRequest.OnStreamRequestFinished += StreamRequestFinished;
    }

    private void Start()
    {
        dataStreamer = transform.GetComponentInChildren<DataStreamer>();
        dataStorage = transform.GetComponentInChildren<DataStorage>();

        var dataStreamRequest = transform.GetComponentInChildren<StreamRequest>();
        dataStreamRequest.Request(StreamRequestType.DetailsRequest);
    }



    private void StreamRequestFinished(StreamRequestType srt)
    {
        if (srt == StreamRequestType.DetailsRequest)
        {
            Debug.Log("Streaming of symbols details ended");
        }
    }

    public void SetupStreaming(int lYear, int rYear)
    {
        BsonDateTime startDate = new BsonDateTime(new System.DateTime(lYear, 1, 15));
        BsonDateTime endDate = new BsonDateTime(new System.DateTime(rYear, 1, 14));

        dateRange = new BsonDocument
            {
                { "startDate", startDate },
                { "endDate", endDate }
            };

        Debug.Log("To be simulated date range: " + dateRange);

        dataStreamer.SetupStreamer(dateRange);
    }

}
