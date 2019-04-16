using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataManager : MonoBehaviour
{
    public static DataManager ins;

    public BsonDocument dateRange { get; private set; }

    // Event to announce the streamer is stopped and reset
    public delegate void StreamResetHandler();
    public static event StreamResetHandler OnStreamReset;    

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
    }

    private void Start()
    {
        var dataStreamRequest = transform.GetComponentInChildren<StreamRequest>();
        dataStreamRequest.Request(StreamRequestType.DetailsRequest);
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

        DataStreamer.ins.SetupStreamer(dateRange);
    }

    public void StartStreaming()
    {
        DataStreamer.ins.StartStreamer();
    }

    public void ResetStreaming()
    {
        DataStreamer.ins.StopAllCoroutines();

        if (OnStreamReset != null) OnStreamReset();
    }

    

}
