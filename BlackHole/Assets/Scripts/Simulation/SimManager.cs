using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MongoDB.Bson;

public class SimManager : MonoBehaviour
{
    public static SimManager ins;
    public PlanetsManager pm;


    public ActiveController simulationUpdate = new ActiveController();
    public bool pause = false;

    public DateTime startDate;
    public DateTime endDate;
    public DateTime currentDate;

    private BsonDocument _currentSlice;
    public BsonDocument currentSlice
    {
        get { return _currentSlice; }
        set { _currentSlice = value; SimulateNewSlice(); }
    }

    [SerializeField, Range(1f, 10f)]
    private float _secondsPerSlice = 1f;
    public float secondsPerSlice
    {
        get
        {
            return _secondsPerSlice;
        }
        set
        {
            _secondsPerSlice = Mathf.Clamp(value, 1f, 10f);
        }
    }

    [SerializeField, Range(2, 100)]
    public static int smoothingStepsPerSlice = 100;
    

    // Event to announce another one day slice of data is needed
    public delegate void SliceNeededHandler(BsonDateTime date);
    public static event SliceNeededHandler OnSliceNeeded;

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
        DataProvider.OnSliceReady += SliceReceived;
        PlanetsManager.OnPlanetsLoaded += StartSimulation;
    }

    private void OnDisable()
    {
        DataProvider.OnSliceReady -= SliceReceived;
        PlanetsManager.OnPlanetsLoaded -= StartSimulation;
    }

    public void PrepareSimulation()
    {
        startDate = DataManager.ins.dateRange["startDate"].ToUniversalTime();
        endDate = DataManager.ins.dateRange["endDate"].ToUniversalTime();
        currentDate = startDate;
        pm.SpawnRequest(DataStorage.ins.symbolNames.Count);
        DataStreamer.ins.streamFrequency = 1f;
    }

    private void SimulateNewSlice()
    {
        var data = DataTranslator.TranslateSlice(currentSlice);
        //StartCoroutine(DataSmoothner.ins.SmoothUpdate(pm, data));
        pm.UpdatePlanets(data);
        if (pause == false)  simulationUpdate.Activate();
    }

    private void StartSimulation()
    {
        pm.SetPlanets(true);
        simulationUpdate.Activate();
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        while (currentDate <= endDate)
        {
            if (simulationUpdate.isActive && !pause && DataStorage.ins.datesStored.Contains(new BsonDateTime(currentDate)))
            {
                simulationUpdate.Deactivate();
                RequestSlice();
            }
            yield return new WaitForSeconds(secondsPerSlice);
        }
    }

    private void RequestSlice()
    {
        Debug.Log("Slice requested by SimManager for date: " + currentDate);
        if (OnSliceNeeded != null) OnSliceNeeded(currentDate);
    }

    private void SliceReceived(BsonDocument slice)
    {
        currentDate = currentDate.AddDays(1);
        currentSlice = slice;   
    }

    public void ToggleSimulation()
    {
        pm.SetPlanets(pause);
        pause = !pause;
    }

    public void StopSimulation()
    {
        pm.SetPlanets(pause);
        pause = true;
        StopAllCoroutines();
        SceneLoader.LoadScene(SceneName.MainMenu);
    }

}
