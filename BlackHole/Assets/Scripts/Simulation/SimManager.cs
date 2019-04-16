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

    public DateTime startDate;
    public DateTime endDate;
    public DateTime currentDate;

    private BsonDocument _currentSlice;
    public BsonDocument currentSlice
    {
        get { return _currentSlice; }
        set { _currentSlice = value; SimulateNewSlice(); }
    }

    [SerializeField, Range(1f, 5f)]
    private float _secondsPerSlice = 3f;
    public float secondsPerSlice
    {
        get
        {
            return _secondsPerSlice;
        }
        set
        {
            _secondsPerSlice = Mathf.Clamp(value, 1f, 5f);
        }
    }

    [SerializeField, Range(2, 10)]
    private int _smoothingStepsPerSlice = 2;
    public int smoothingStepsPerSlice
    {
        get
        {
            return _smoothingStepsPerSlice;
        }
        set
        {
            _smoothingStepsPerSlice = Mathf.Clamp(value, 2, 10);
        }
    }

    // Event to announce another one day slice of data is needed
    public delegate void SliceNeededHandler(BsonDateTime date);
    public static event SliceNeededHandler OnSliceNeeded;

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
        pm.SpawnRequest(DataStorage.ins.numberOfStocks);
    }

    private void SimulateNewSlice()
    {
        simulationUpdate.Activate();
        pm.UpdatePlanets(DataTranslator.TranslateSlice(currentSlice));       

    }

    private void StartSimulation()
    {
        pm.SetPlanets(true);
        if (simulationUpdate.isActive == false) simulationUpdate.Activate();
        Debug.Log("Simulation coroutine start with status: " + simulationUpdate.isActive);
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        while (currentDate <= endDate)
        {
            Debug.Log(currentDate + " with simulationUpdate: " + simulationUpdate.isActive + " and date stored: " + DataStorage.ins.datesStored.Contains(new BsonDateTime(currentDate)));
            if (simulationUpdate.isActive && DataStorage.ins.datesStored.Contains(new BsonDateTime(currentDate)))
            {
                RequestSlice();
                simulationUpdate.Deactivate();
            }
            yield return new WaitForSeconds(secondsPerSlice);
        }        
    }

    private void RequestSlice()
    {
        Debug.Log("Slice requested");
        if (OnSliceNeeded != null) OnSliceNeeded(currentDate);
    }

    private void SliceReceived(BsonDocument slice)
    {
        currentDate = currentDate.AddDays(1);
        currentSlice = slice;
    }



}
