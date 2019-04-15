using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB.Bson;
public class StreamRequest : MonoBehaviour
{
    private static DbConnection dbConnection;
    public DataStorage dataStorage;

    // Event to announce the stream request has completed
    public delegate void StreamRequestFinished(StreamRequestType srt);
    public static event StreamRequestFinished OnStreamRequestFinished;

    void Awake()
    {
        dbConnection = new DbConnection(@"mongodb://project-blackhole:4X8OeAasgEu20h6rV86JLFeXKZ7ApSq8UoZVE9kPChXCg6sR705luvze7EIZsf0wsFJq9z8AEOhZxRjnFN2oOA==@project-blackhole.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
    }

    void Start()
    {
        dataStorage = transform.parent.GetComponentInChildren<DataStorage>();
    }

    public async void Request(StreamRequestType srt, BsonDateTime date = null)
    {
        if (srt == StreamRequestType.DetailsRequest)
        {
            var symbols_data = await dbConnection.GetAll();
            StartCoroutine(dataStorage.AddStatic(symbols_data));
        }
        else if (srt == StreamRequestType.SliceRequest && dataStorage.datesStored.IndexOf(date) == -1)
        {
            var slice_data = await dbConnection.GetSlice(date);
            StartCoroutine(dataStorage.AddSlice(slice_data, date));
        }

        if (OnStreamRequestFinished != null) OnStreamRequestFinished(srt);
    }
}

public enum StreamRequestType
{
    DetailsRequest,
    SliceRequest
}