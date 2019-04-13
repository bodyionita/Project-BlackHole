using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager ins;

    private DataStreamer dataStreamer;
    private DataStorage dataStorage;

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
        dataStreamer = transform.GetComponentInChildren<DataStreamer>();
        dataStorage = transform.GetComponentInChildren<DataStorage>();

        dataStreamer.StreamRequest(StreamRequestType.DetailsRequest);
    }

    private void StreamRequestFinished()
    {
        Debug.Log("Stream ended");
    }

    private void StaticDataStored()
    {
        Debug.Log("Static Data Stored");
    }

    private void OnEnable()
    {
        DataStreamer.OnStreamRequestFinished += StreamRequestFinished;
        DataStorage.OnStaticDataStored += StaticDataStored;
    }

}
