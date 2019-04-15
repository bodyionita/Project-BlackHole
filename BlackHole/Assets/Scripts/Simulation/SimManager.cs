using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MongoDB.Bson;

public class SimManager : MonoBehaviour
{
    public static SimManager ins;
    
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
        MainMenuUI.OnStartPressed += StartPreparing;
    }

    private void StartPreparing(int lYear, int rYear)
    {
        DataManager.ins.SetupStreaming(lYear, rYear);

        SceneLoader.LoadScene(SceneName.LoadingScreen);
    }

    

    

}
