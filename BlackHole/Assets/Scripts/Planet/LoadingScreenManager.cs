using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    [Range(0, 1000)]
    public int numberOfPlanets;
    public PlanetsManager planetsManager;

    void Start()
    {
        planetsManager.SpawnRequest(numberOfPlanets);      

    }

    void ModifyPlanets()
    {
        planetsManager.SetPlanets(true);

        var pl = new Dictionary<int, Planet>();
        for (int i = 1; i <= numberOfPlanets; i++)
        {
            var r = (float)i / (float)numberOfPlanets;
            var planet = new Planet(Random.ColorHSV(), r * 10 + 1, r * 40 + 5, Mathf.Abs(r - 0.5f) * 25 + 5, r * 10);
            pl.Add(i, planet);
        }
        planetsManager.UpdatePlanets(pl);
    }

    private void OnEnable()
    {
        PlanetsManager.OnPlanetsLoaded += ModifyPlanets;
    }
    private void OnDisable()
    {
        PlanetsManager.OnPlanetsLoaded -= ModifyPlanets;
    }
}
