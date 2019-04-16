using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsManager : MonoBehaviour
{
    private int totalPlanets = -1;

    private int loadedPlanets = 0;

    private int _updatedPlanets = 0;
    private int updatedPlanets
    {
        get { return _updatedPlanets; }
        set
        {
            _updatedPlanets = value;
            if (_updatedPlanets == totalPlanets)
                if (OnPlanetsUpdated != null) OnPlanetsUpdated();
        }
    }

    // Planet container
    [SerializeField]
    private Transform container;

    // Event to announce all planets have been fully loaded
    public delegate void PlanetsLoadedHandler();
    public static event PlanetsLoadedHandler OnPlanetsLoaded;

    // Event to order status change for planets
    public delegate void StatusCommandHandler(bool new_status, int planetIndex=-1);
    public static event StatusCommandHandler OnStatusCommand;

    // Event to announce planets have been updated
    public delegate void PlanetsUpdatedHandler();
    public static event PlanetsUpdatedHandler OnPlanetsUpdated;

    // Keep track of the planets controller by their assigned indexes
    private Dictionary<int, PlanetController> IndexToControllerMap = new Dictionary<int, PlanetController>();    

    public void UpdatePlanets(Dictionary<int, Planet> planets)
    {
        updatedPlanets = 0;
        foreach (var planet in planets)
        {
            PlanetController pc = IndexToControllerMap[planet.Key];
            pc.UpdatePlanet(planet.Value);
        }
    }

    public void SpawnRequest(int requested_number)
    {
        loadedPlanets = 0;
        totalPlanets = requested_number;
        for (int i = 1; i <= requested_number; i++)
            PlanetFactory.ins.SpawnPlanet(container);
        if (OnPlanetsLoaded != null) OnPlanetsLoaded();
    }

    public void SetPlanets(bool active)
    {
        if (OnStatusCommand != null) OnStatusCommand(active);
    }    

    private void OnEnable()
    {
        PlanetController.OnComponentsGathered += PlanetLoaded;
        PlanetController.OnPlanetUpdated += PlanetUpdated;
    }

    private void OnDisable()
    {
        PlanetController.OnComponentsGathered -= PlanetLoaded;
        PlanetController.OnPlanetUpdated -= PlanetUpdated;

    }

    private int PlanetLoaded(PlanetController pc)
    {
        IndexToControllerMap[loadedPlanets+1] = pc;
        loadedPlanets += 1;
        if (loadedPlanets == totalPlanets)
            if (OnPlanetsLoaded != null) OnPlanetsLoaded();
        return loadedPlanets;
    }

    private void PlanetUpdated()
    {
        updatedPlanets += 1;
    }

}
