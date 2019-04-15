using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;

public class DataStreamer : MonoBehaviour
{
    public DateTime startDate { get; private set; }
    public DateTime endDate { get; private set; }

    private StreamRequest stream;

    // Event to announce streaming has finished
    public delegate void StreamFinishedHandler();
    public static event StreamFinishedHandler OnStreamFinishedHandler;

    private IEnumerator Streamer()
    {
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            stream.Request(StreamRequestType.SliceRequest, new BsonDateTime(date));
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("Data gathered for: " + date);
        }
        yield return new WaitForSeconds(2f);
        OnStreamFinishedHandler();
    }

    public void SetupStreamer(BsonDocument dateRange)
    {
        stream = gameObject.GetComponent<StreamRequest>();

        startDate = dateRange["startDate"].ToUniversalTime();
        endDate = dateRange["endDate"].ToUniversalTime();

        StartCoroutine("Streamer");
    }
}
