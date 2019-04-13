using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
public class DataStreamer : MonoBehaviour
{
    private static DbConnection dbConnection;
    public DataStorage dataStorage;

    // Event to announce the stream request has completed
    public delegate void StreamRequestFinished();
    public static event StreamRequestFinished OnStreamRequestFinished;

    void Awake()
    {
        dbConnection = new DbConnection(@"mongodb://project-blackhole:4X8OeAasgEu20h6rV86JLFeXKZ7ApSq8UoZVE9kPChXCg6sR705luvze7EIZsf0wsFJq9z8AEOhZxRjnFN2oOA==@project-blackhole.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
    }

    void Start()
    {
        dataStorage = transform.parent.GetComponentInChildren<DataStorage>();
    }

    public void StreamRequest(StreamRequestType srt, BsonDateTime date = null)
    {
        if (srt == StreamRequestType.DetailsRequest)
        {
            var symbols_data = dbConnection.GetAll();
            dataStorage.AddStatic(symbols_data);
        }
        else if (srt == StreamRequestType.SliceRequest)
        {
            var slice_data = dbConnection.GetSlice(date);
            dataStorage.AddSlice(slice_data, date);
        }

        if (OnStreamRequestFinished != null) OnStreamRequestFinished();
    }
}

public enum StreamRequestType
{
    DetailsRequest,
    SliceRequest
}