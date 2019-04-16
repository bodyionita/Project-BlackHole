using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataStreamer : MonoBehaviour
{
    public static DataStreamer ins;

    public DateTime startDate { get; private set; }
    public DateTime endDate { get; private set; }

    private StreamRequest stream;

    // Event to announce streaming has finished
    public delegate void StreamFinishedHandler();
    public static event StreamFinishedHandler OnStreamFinished;

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

    private IEnumerator Streamer()
    {
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            stream.Request(StreamRequestType.SliceRequest, new BsonDateTime(date));
            yield return new WaitForSecondsRealtime(0.5f);
            //Debug.Log("Data gathered for: " + date);
        }
        yield return new WaitForSecondsRealtime(2f);
        if (OnStreamFinished!=null) OnStreamFinished();
    }

    public void StartStreamer()
    {
        StartCoroutine("Streamer");
    }

    public void SetupStreamer(BsonDocument dateRange)
    {
        stream = gameObject.GetComponent<StreamRequest>();

        startDate = dateRange["startDate"].ToUniversalTime();
        endDate = dateRange["endDate"].ToUniversalTime();
    }
}
